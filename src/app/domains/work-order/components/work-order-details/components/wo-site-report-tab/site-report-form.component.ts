import { Component, EventEmitter, Inject, Input, Output, Optional } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormArray } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { WorkOrder, workOrderItem, materialAssignment } from '../../../../models/work-order.model';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserService } from '../../../../../../shared/services/user.service';
import { WorkOrderService } from '../../../../services/work-order.service';

@Component({
  selector: 'app-site-report-form',
  templateUrl: './site-report-form.component.html',
  styleUrls: ['./site-report-form.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatChipsModule,
    MatDividerModule,
    MatSelectModule,
    MatOptionModule
  ]
})
export class SiteReportFormComponent {
  @Input() workOrder!: WorkOrder;
  @Output() submitted = new EventEmitter<any>();
  @Output() updated = new EventEmitter<void>();
  @Output() reportDeleted = new EventEmitter<{ workDoneId: string }>();

  form: FormGroup;
  foremanName: string = '';
  materialOptions: { id: string, name: string }[] = [];
  workDoneOptions: { id: string, label: string }[] = [];

  constructor(
    private fb: FormBuilder,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
    @Optional() private dialogRef: MatDialogRef<SiteReportFormComponent>,
    private userService: UserService,
    private workOrderService: WorkOrderService
  ) {
    if (data && data.workOrder) {
      this.workOrder = data.workOrder;
    }
    this.foremanName = this.userService.getCurrentUserName();
    this.materialOptions = (this.workOrder?.materials || []).map((m: materialAssignment) => {
      if (m.materialType === 'purchasable' && m.purchasableMaterial) {
        return { id: m.id, name: m.purchasableMaterial.name };
      } else if (m.materialType === 'receivable' && m.receivableMaterial) {
        return { id: m.id, name: m.receivableMaterial.name };
      }
      return { id: m.id, name: 'Unknown Material' };
    });
    this.workDoneOptions = (this.workOrder?.items || []).map((item: workOrderItem) => ({
      id: item.id,
      label: item.itemDetail.shortDescription
    }));
    this.workDoneOptions.push({ id: 'other', label: 'Other (specify)' });

    this.form = this.fb.group({
      date: [new Date(), Validators.required],
      foremanName: [{ value: this.foremanName, disabled: true }, Validators.required],
      workDone: ['', Validators.required],
      workDoneOther: [''],
      actualQuantity: [null],
      notes: [''],
      materialsUsed: this.fb.array([]),
      photos: this.fb.array([])
    });

    // Add conditional validator for actualQuantity
    this.form.get('workDone')?.valueChanges.subscribe((val) => {
      const actualQuantityControl = this.form.get('actualQuantity');
      if (val && val !== 'other') {
        actualQuantityControl?.setValidators([Validators.required, Validators.pattern('^[0-9]+(\\.[0-9]+)?$')]);
      } else {
        actualQuantityControl?.clearValidators();
      }
      actualQuantityControl?.updateValueAndValidity();
    });

    // Ensure actualQuantity is always a number
    this.form.get('actualQuantity')?.valueChanges.subscribe((val) => {
      if (val && typeof val === 'string') {
        const numVal = Number(val);
        if (!isNaN(numVal)) {
          this.form.patchValue({ actualQuantity: numVal }, { emitEvent: false });
        }
      }
    });
  }

  get materialsUsed(): FormArray {
    return this.form.get('materialsUsed') as FormArray;
  }

  addMaterial() {
    this.materialsUsed.push(this.fb.group({
      materialId: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]]
    }));
  }

  removeMaterial(index: number) {
    this.materialsUsed.removeAt(index);
  }

  get photos(): FormArray {
    return this.form.get('photos') as FormArray;
  }

  addPhoto() {
    this.photos.push(this.fb.group({
      url: ['', Validators.required],
      caption: ['']
    }));
  }

  removePhoto(index: number) {
    this.photos.removeAt(index);
  }

  onSubmit() {
    if (this.form.valid) {
      const rawValue = this.form.getRawValue();
      console.log('[DEBUG] Form raw value:', rawValue);
      
      // Convert actualQuantity to number if it exists
      const value = {
        ...rawValue,
        actualQuantity: rawValue.actualQuantity ? Number(rawValue.actualQuantity) : null
      };
      
      console.log('[DEBUG] Processed value:', value);
      
      setTimeout(() => {
        // Prepare updated items if needed
        let updatedItems = [...this.workOrder.items];
        if (value.workDone && value.workDone !== 'other') {
          const itemIndex = updatedItems.findIndex((item: any) => item.id === value.workDone);
          if (itemIndex !== -1) {
            updatedItems[itemIndex] = {
              ...updatedItems[itemIndex],
              actualQuantity: value.actualQuantity
            };
          }
        }
        this.updated.emit(value); // Pass the new report to the parent
        if (this.dialogRef) {
          this.dialogRef.close(value);
        } else {
          this.submitted.emit(value);
        }
        this.form.reset({ date: new Date(), foremanName: this.foremanName });
        this.materialsUsed.clear();
        this.photos.clear();
      });
    }
  }

  // Call this method from parent when a report is deleted
  onReportDeleted(workDoneId: string) {
    // Remove actualQuantity from the corresponding item (set to 0)
    const updatedItems = this.workOrder.items.map(item =>
      item.id === workDoneId ? { ...item, actualQuantity: 0 } : item
    );
    this.workOrderService.updateWorkOrder(this.workOrder.id, { items: updatedItems }).subscribe(() => {
      this.updated.emit();
    });
  }
} 