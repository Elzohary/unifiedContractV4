import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { WorkOrderService } from '../../services/work-order.service';
import { WorkOrderItemService } from '../../services/work-order-item.service';
import { NotificationService } from '../../../../shared/services/notification.service';
import {
  WorkOrder,
  WorkOrderStatus,
  WorkOrderPriority,
  Iitem
} from '../../models/work-order.model';

@Component({
  selector: 'app-work-order-form',
  templateUrl: './work-order-form.component.html',
  styleUrls: ['./work-order-form.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule
  ],
  providers: [
    WorkOrderService,
    WorkOrderItemService,
    NotificationService
  ]
})
export class WorkOrderFormComponent implements OnInit, OnDestroy {
  form!: FormGroup;
  loading = false;
  private destroy$ = new Subject<void>();

  // Available items for dropdown
  availableItems: Iitem[] = [];

  // Category options
  readonly categoryOptions: string[] = [
    'Repair',
    'Installation',
    'Inspection',
    'Maintenance',
    'Emergency',
    'Upgrade',
    'Other'
  ];

  constructor(
    private fb: FormBuilder,
    private workOrderService: WorkOrderService,
    private workOrderItemService: WorkOrderItemService,
    private notificationService: NotificationService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.initializeForm();
  }

  ngOnInit(): void {
    this.loadAvailableItems();
  }

  private initializeForm(): void {
    this.form = this.fb.group({
      // Work Order Details
      workOrderNumber: ['', Validators.required],
      client: ['', Validators.required],
      location: ['', Validators.required],
      receivedDate: [new Date(), Validators.required],
      category: ['', Validators.required],
      
      // Work Order Items
      items: this.fb.array([])
    });
  }

  // Form Array Getter
  get items() {
    return this.form.get('items') as FormArray;
  }

  // Add Item Method
  addItem(): void {
    const itemGroup = this.fb.group({
      itemNumber: ['', Validators.required],
      shortDescription: ['', Validators.required],
      UOM: [{ value: '', disabled: true }],
      managementArea: ['', Validators.required],
      estimatedQuantity: [1, [Validators.required, Validators.min(1)]],
      unitPrice: [{ value: 0, disabled: true }],
      estimatedPrice: [{ value: 0, disabled: true }]
    });

    // Subscribe to changes in item number
    itemGroup.get('itemNumber')?.valueChanges.subscribe(itemNumber => {
      const selectedItem = this.availableItems.find(item => item.itemNumber === itemNumber);
      if (selectedItem) {
        itemGroup.patchValue({
          shortDescription: selectedItem.shortDescription,
          UOM: selectedItem.UOM,
          managementArea: selectedItem.managementArea,
          unitPrice: selectedItem.unitPrice
        }, { emitEvent: false });
        this.updatePrice(itemGroup);
      }
    });

    // Subscribe to changes in estimated quantity
    itemGroup.get('estimatedQuantity')?.valueChanges.subscribe(() => {
      this.updatePrice(itemGroup);
    });

    this.items.push(itemGroup);
  }

  // Remove Item Method
  removeItem(index: number): void {
    this.items.removeAt(index);
  }

  // Load available items for dropdown
  private loadAvailableItems(): void {
    // If you have a workOrderId, pass it here. Otherwise, use a placeholder or refactor as needed.
    this.workOrderItemService.getItems('')
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (items: Iitem[]) => {
          this.availableItems = items;
        },
        error: (error: Error) => {
          console.error('Error loading items:', error);
          this.snackBar.open('Error loading available items', 'Close', { duration: 3000 });
        }
      });
  }

  // Update price calculation
  private updatePrice(itemGroup: FormGroup): void {
    const quantity = itemGroup.get('estimatedQuantity')?.value || 0;
    const unitPrice = itemGroup.get('unitPrice')?.value || 0;
    const total = quantity * unitPrice;

    itemGroup.patchValue({
      estimatedPrice: total
    }, { emitEvent: false });
  }

  // Form submission
  onSubmit(): void {
    if (this.form.valid) {
      this.loading = true;
      const formValue = this.form.value;

      // Generate internal order number
      const internalOrderNumber = `INT-${Date.now()}`;

      const newWorkOrder: Partial<WorkOrder> = {
        details: {
          workOrderNumber: formValue.workOrderNumber,
          internalOrderNumber: internalOrderNumber,
          title: `${formValue.category} - ${formValue.client}`,
          description: `${formValue.category} work for ${formValue.client} at ${formValue.location}`,
          priority: 'medium' as WorkOrderPriority,
          category: formValue.category,
          startDate: formValue.receivedDate,
          dueDate: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString(),
          targetEndDate: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString(),
          location: formValue.location,
          client: formValue.client,
          status: 'pending' as WorkOrderStatus,
          createdDate: new Date().toISOString(),
          createdBy: 'current-user',
          completionPercentage: 0,
          receivedDate: formValue.receivedDate
        },
        items: formValue.items || [],
        remarks: [],
        issues: [],
        materials: [],
        permits: [],
        tasks: [],
        manpower: [],
        actions: [],
        photos: [],
        forms: [],
        expenses: [],
        invoices: [],
        expenseBreakdown: {
          materials: 0,
          labor: 0,
          other: 0
        }
      };

      this.workOrderService.createWorkOrder(newWorkOrder)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (workOrder: WorkOrder) => {
            // If there are items in the work order, create them in the item service
            if (newWorkOrder.items && newWorkOrder.items.length > 0) {
              this.workOrderItemService.createItemsFromWorkOrder(
                newWorkOrder.items,
                workOrder.id
              ).subscribe(createdItems => {
                console.log(`Created ${createdItems.length} items for work order ${workOrder.id}`);
              });
            }

            this.snackBar.open('Work order created successfully', 'Close', { duration: 3000 });
            // Redirect to the work order list page instead of details
            this.router.navigate(['/work-orders']);
          },
          error: (error: Error) => {
            console.error('Error creating work order:', error);
            this.snackBar.open('Error creating work order', 'Close', { duration: 3000 });
            this.loading = false;
          }
        });
    } else {
      this.markFormGroupTouched(this.form);
    }
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();

      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  hasError(controlPath: string, errorName: string): boolean {
    const control = this.form.get(controlPath);
    return control ? control.hasError(errorName) && control.touched : false;
  }

  // Get unique items for dropdowns
  getUniqueItems(): Iitem[] {
    const uniqueItems = new Map<string, Iitem>();
    this.availableItems.forEach(item => {
      if (!uniqueItems.has(item.itemNumber)) {
        uniqueItems.set(item.itemNumber, item);
      }
    });
    return Array.from(uniqueItems.values());
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
} 