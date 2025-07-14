import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { Iitem } from '../../../models/work-order-item.model';

@Component({
  selector: 'app-manual-work-order-item-dialog',
  templateUrl: './manual-work-order-item-dialog.component.html',
  styleUrls: ['./manual-work-order-item-dialog.component.scss'],
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
export class ManualWorkOrderItemDialogComponent {
  itemForm: FormGroup;
  dialogTitle: string;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<ManualWorkOrderItemDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.dialogTitle = data?.title || 'Add Work Order Item';
    this.itemForm = this.fb.group({
      itemNumber: ['', Validators.required],
      lineType: ['Description', Validators.required],
      shortDescription: ['', Validators.required],
      longDescription: [''],
      UOM: ['', Validators.required],
      currency: ['SAR', Validators.required],
      unitPrice: [0, [Validators.required, Validators.min(0)]],
      paymentType: ['', Validators.required],
      managementArea: ['', Validators.required],
      estimatedQuantity: [1, [Validators.required, Validators.min(1)]]
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.itemForm.valid) {
      this.dialogRef.close(this.itemForm.value as Iitem);
    }
  }
} 