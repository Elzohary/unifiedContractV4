import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { Iitem } from '../../models/work-order-item.model';
import { WorkOrderItemService } from '../../services/work-order-item.service';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-assign-work-order-item-dialog',
  templateUrl: './assign-work-order-item-dialog.component.html',
  styleUrls: ['./assign-work-order-item-dialog.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatCardModule
  ]
})
export class AssignWorkOrderItemDialogComponent implements OnInit {
  assignForm: FormGroup;
  dialogTitle: string;
  availableItems: Iitem[] = [];
  filteredItems: Iitem[] = [];
  selectedItem: Iitem | null = null;
  assignedItemIds: string[] = [];
  fetchError: string | null = null;
  canOverridePrice: boolean = false;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<AssignWorkOrderItemDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private itemService: WorkOrderItemService,
    private authService: AuthService
  ) {
    this.dialogTitle = data.title || 'Assign Work Order Item';
    this.assignedItemIds = data.assignedItemIds || [];
    this.canOverridePrice = this.authService.hasPermission('work-orders.override-price');
    this.assignForm = this.fb.group({
      itemId: ['', Validators.required],
      estimatedQuantity: [1, [Validators.required, Validators.min(1)]],
      overrideUnitPrice: [{ value: '', disabled: !this.canOverridePrice }]
    });
  }

  ngOnInit(): void {
    this.itemService.getItems(this.data.workOrderId).subscribe({
      next: items => {
        this.availableItems = items;
        this.filteredItems = items.filter(item => !this.assignedItemIds.includes(item.id));
        this.fetchError = null;
      },
      error: err => {
        this.fetchError = 'Failed to load items. Please try again.';
        this.availableItems = [];
        this.filteredItems = [];
      }
    });
  }

  onItemSelect(itemId: string): void {
    this.selectedItem = this.availableItems.find(i => i.id === itemId) || null;
    if (this.canOverridePrice && this.selectedItem) {
      this.assignForm.get('overrideUnitPrice')?.setValue(this.selectedItem.unitPrice);
    }
  }

  onFilterChange(filter: string): void {
    const f = (filter || '').toLowerCase();
    this.filteredItems = this.availableItems.filter(item =>
      !this.assignedItemIds.includes(item.id) &&
      (item.itemNumber.toLowerCase().includes(f) ||
      item.shortDescription.toLowerCase().includes(f) ||
      item.longDescription.toLowerCase().includes(f))
    );
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.assignForm.valid && this.selectedItem) {
      const formValue = this.assignForm.value;
      this.dialogRef.close({
        ...this.selectedItem,
        estimatedQuantity: formValue.estimatedQuantity,
        unitPrice: this.canOverridePrice && formValue.overrideUnitPrice ? formValue.overrideUnitPrice : this.selectedItem.unitPrice
      });
    }
  }
} 