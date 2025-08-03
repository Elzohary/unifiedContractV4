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
  isLoadingItems = false;

  // Category options
  readonly categoryOptions: string[] = [
    'Repair',
    'Installation',
    'Inspection',
    'Maintenance',
    'Emergency',
    'Upgrade',
    'Consumer',
    'Project',
    'Other'
  ];

  // Project type options
  readonly projectTypeOptions: string[] = [
    '801',
    '802',
    '803',
    '804',
    '404',
    '442',
    '403',
    'Manual TYPE'
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
      projectType: [''],
      po: [''],
      d1: [''],
      
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
      id: [''], // Add ID field to store the selected item's ID
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
          id: selectedItem.id, // Store the item ID
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
    // Load all available items for the dropdown
    console.log('Loading available items...');
    this.isLoadingItems = true;
    this.workOrderItemService.getAvailableItems()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (items: Iitem[]) => {
          console.log('Received items from API:', items);
          this.availableItems = items;
          this.isLoadingItems = false;
        },
        error: (error: Error) => {
          console.error('Error loading items:', error);
          this.snackBar.open('Error loading available items', 'Close', { duration: 3000 });
          this.isLoadingItems = false;
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

      // Create the payload that matches the backend CreateWorkOrderDto
      const createWorkOrderPayload = {
        workOrderNumber: formValue.workOrderNumber,
        internalOrderNumber: internalOrderNumber,
        title: `${formValue.category} - ${formValue.client}`,
        description: `${formValue.category} work for ${formValue.client} at ${formValue.location}`,
        location: formValue.location,
        category: formValue.category,
        type: formValue.projectType || '',
        class: '',
        projectType: formValue.projectType || '',
        po: formValue.po || '',
        d1: formValue.d1 || '',
        completionPercentage: 0,
        receivedDate: new Date(formValue.receivedDate).toISOString(),
        startDate: new Date(formValue.receivedDate).toISOString(),
        dueDate: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString(),
        targetEndDate: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString(),
        estimatedCost: 0,
        workOrderStatusId: '11111111-1111-1111-1111-111111111111', // Pending status from seed data
        priorityLevelId: '33333333-3333-3333-3333-333333333333', // Normal priority from seed data
        clientId: this.getClientIdFromName(formValue.client), // Map client name to ID
        engineerInChargeId: null,
        createdBy: 'current-user'
      };

      console.log('=== DEBUG: Form values ===');
      console.log('PO value from form:', formValue.po);
      console.log('D1 value from form:', formValue.d1);
      console.log('Sending payload to backend:', createWorkOrderPayload);
      
      this.workOrderService.createWorkOrder(createWorkOrderPayload)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (response: any) => {
            console.log('=== DEBUG: Work order creation response ===');
            console.log('Full response:', response);
            console.log('Response type:', typeof response);
            console.log('Response keys:', Object.keys(response || {}));
            
            // The response is already the work order object (WorkOrderDetailsDto)
            // No need to access response.data since WorkOrderService already extracts it
            const workOrder = response;
            
            if (!workOrder || !workOrder.id) {
              console.error('Cannot find work order data in response');
              this.snackBar.open('Error: Invalid response format', 'Close', { duration: 3000 });
              this.loading = false;
              return;
            }
            
            console.log('Extracted workOrder:', workOrder);
            console.log('WorkOrder ID:', workOrder?.id);
            
            // If there are items in the form, assign them to the work order
            if (formValue.items && formValue.items.length > 0) {
              console.log('=== DEBUG: Items to assign ===');
              console.log('Form items:', formValue.items);
              console.log('Work order ID:', workOrder.id);
              
              this.workOrderItemService.createItemsFromWorkOrder(
                formValue.items,
                workOrder.id
              ).subscribe({
                next: (createdItems) => {
                  console.log(`=== DEBUG: Assigned ${createdItems.length} items to work order ${workOrder.id} ===`);
                  console.log('Assigned items:', createdItems);
                },
                error: (error) => {
                  console.error('=== DEBUG: Error assigning items ===', error);
                }
              });
            } else {
              console.log('=== DEBUG: No items to assign ===');
            }

            this.snackBar.open('Work order created successfully', 'Close', { duration: 3000 });
            this.loading = false;
            
            // Navigate to the newly created work order details page
            console.log('=== DEBUG: About to navigate to work order details ===');
            console.log('=== DEBUG: Work Order ID for navigation:', workOrder.id);
            this.router.navigate(['/work-orders/details', workOrder.id]).then(() => {
              console.log('=== DEBUG: Navigation to work order details completed ===');
            }).catch(err => {
              console.error('=== DEBUG: Navigation to work order details failed ===', err);
              // Fallback to work orders list if navigation fails
              this.router.navigate(['/work-orders']);
            });
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

  onCancel(): void {
    this.router.navigate(['/work-orders']);
  }

  // Map client name to client ID
  private getClientIdFromName(clientName: string): string {
    console.log('=== DEBUG: getClientIdFromName called with:', clientName);
    
    // For now, map to the demo client. In a real application, this would query the database
    const clientMappings: { [key: string]: string } = {
      'SEC': 'cccccccc-cccc-cccc-cccc-cccccccccccc',
      'Demo Client': 'cccccccc-cccc-cccc-cccc-cccccccccccc',
      'demo client': 'cccccccc-cccc-cccc-cccc-cccccccccccc',
      'Demo': 'cccccccc-cccc-cccc-cccc-cccccccccccc',
      'demo': 'cccccccc-cccc-cccc-cccc-cccccccccccc'
    };
    
    const mappedId = clientMappings[clientName] || 'cccccccc-cccc-cccc-cccc-cccccccccccc';
    console.log('=== DEBUG: Mapped client ID:', mappedId);
    return mappedId; // Default to demo client
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