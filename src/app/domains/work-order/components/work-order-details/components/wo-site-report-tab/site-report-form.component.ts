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
import { MatStepperModule } from '@angular/material/stepper';
import { PhotoUploadComponent } from '../../../../../../shared/components/photo-upload/photo-upload.component';
import { WorkOrder, workOrderItem, materialAssignment } from '../../../../models/work-order.model';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserService } from '../../../../../../shared/services/user.service';
import { WorkOrderService } from '../../../../services/work-order.service';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'workDoneLabel', standalone: true })
export class WorkDoneLabelPipe implements PipeTransform {
  transform(id: string, options: { id: string, label: string }[]): string {
    return options.find(opt => opt.id === id)?.label || id;
  }
}

@Pipe({ name: 'materialName', standalone: true })
export class MaterialNamePipe implements PipeTransform {
  transform(id: string, options: { id: string, name: string }[]): string {
    return options.find(opt => opt.id === id)?.name || id;
  }
}

class HousekeepingPhotosArray extends Array<any> {
  invalid = false;
}

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
    MatOptionModule,
    MatStepperModule,
    PhotoUploadComponent,
    WorkDoneLabelPipe,
    MaterialNamePipe
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

  // --- Add missing form groups and properties for template compatibility ---
  step1FormGroup: FormGroup;
  step2FormGroup: FormGroup;
  step3FormGroup: FormGroup;
  progressPhotos = { controls: [] };
  safetyPhotos: any[] = [];
  housekeepingPhotos = new HousekeepingPhotosArray();
  progressPhotosList: File[] = [];
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
    // Debug: Log all materials and filtered options
    console.log('[DEBUG] workOrder.materials:', this.workOrder?.materials);
    // Filter and map as before
    this.materialOptions = (this.workOrder?.materials || []).filter((m: materialAssignment) => {
      if (m.materialType === 'purchasable' && m.purchasableMaterial) {
        return m.purchasableMaterial.status === 'in-use';
      } else if (m.materialType === 'receivable' && m.receivableMaterial) {
        return m.receivableMaterial.status === 'received';
      }
      return false;
    }).map((m: materialAssignment) => {
      if (m.materialType === 'purchasable' && m.purchasableMaterial) {
        return { id: m.id, name: m.purchasableMaterial.name };
      } else if (m.materialType === 'receivable' && m.receivableMaterial) {
        return { id: m.id, name: m.receivableMaterial.name };
      }
      return { id: m.id, name: 'Unknown Material' };
    });
    console.log('[DEBUG] Filtered materialOptions:', this.materialOptions);
    this.workDoneOptions = (this.workOrder?.items || []).map((item: workOrderItem) => ({
      id: item.id,
      label: item.itemDetail.shortDescription
    }));
    this.workDoneOptions.push({ id: 'other', label: 'Other (specify)' });

    // Dedicated FormGroups for each step
    this.step1FormGroup = this.fb.group({
      safetyPhotos: [[], Validators.required]
    });
    this.step2FormGroup = this.fb.group({
      workDone: ['', Validators.required],
      workDoneOther: [''],
      actualQuantity: [null],
      notes: [''],
      materialsUsed: this.fb.array([]),
      progressPhotos: [[], Validators.required]
    });
    this.step3FormGroup = this.fb.group({
      housekeepingPhotos: [[], Validators.required]
    });

    // Remove safetyPhotos and housekeepingPhotos from main form
    this.form = this.fb.group({
      date: [new Date(), Validators.required],
      foremanName: [{ value: this.foremanName, disabled: true }, Validators.required]
    });

    // Add conditional validator for actualQuantity
    this.step2FormGroup.get('workDone')?.valueChanges.subscribe((val) => {
      const actualQuantityControl = this.step2FormGroup.get('actualQuantity');
      if (val && val !== 'other') {
        actualQuantityControl?.setValidators([Validators.required, Validators.pattern('^[0-9]+(\\.[0-9]+)?$')]);
      } else {
        actualQuantityControl?.clearValidators();
      }
      actualQuantityControl?.updateValueAndValidity();
    });

    // Ensure actualQuantity is always a number
    this.step2FormGroup.get('actualQuantity')?.valueChanges.subscribe((val) => {
      if (val && typeof val === 'string') {
        const numVal = Number(val);
        if (!isNaN(numVal)) {
          this.step2FormGroup.patchValue({ actualQuantity: numVal }, { emitEvent: false });
        }
      }
    });
    this.housekeepingPhotos = new HousekeepingPhotosArray();
  }

  onSafetyPhotosChange(event: File[]) {
    this.safetyPhotos = event;
    this.step1FormGroup.get('safetyPhotos')?.setValue(event);
    this.step1FormGroup.get('safetyPhotos')?.markAsTouched();
    this.step1FormGroup.get('safetyPhotos')?.updateValueAndValidity();
    console.log('[DEBUG] onSafetyPhotosChange event:', event);
    console.log('[DEBUG] safetyPhotos form value:', this.step1FormGroup.get('safetyPhotos')?.value);
    console.log('[DEBUG] safetyPhotos form valid:', this.step1FormGroup.get('safetyPhotos')?.valid);
  }
  onHousekeepingPhotosChange(event: File[]) {
    this.housekeepingPhotos = new HousekeepingPhotosArray(...event);
    this.step3FormGroup.get('housekeepingPhotos')?.setValue(this.housekeepingPhotos);
    this.step3FormGroup.get('housekeepingPhotos')?.markAsTouched();
    this.step3FormGroup.get('housekeepingPhotos')?.updateValueAndValidity();
  }
  onProgressPhotosChange(event: File[]) {
    this.progressPhotosList = event;
    this.step2FormGroup.get('progressPhotos')?.setValue(event);
    this.step2FormGroup.get('progressPhotos')?.markAsTouched();
    this.step2FormGroup.get('progressPhotos')?.updateValueAndValidity();
  }
  saveReport(status: any) {
    this.onSubmit();
  }
  SiteReportStatus = { Open: 'Open', Closed: 'Closed' };

  get materialsUsed(): FormArray {
    return this.step2FormGroup.get('materialsUsed') as FormArray;
  }

  get materialsUsedArray() {
    const control = this.step2FormGroup.get('materialsUsed');
    return control && (control as any).controls ? (control as any).controls : [];
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
    return this.step2FormGroup.get('progressPhotos') as FormArray;
  }

  addPhoto(type?: string) {
    this.photos.push(this.fb.group({
      url: ['', Validators.required],
      caption: ['']
    }));
  }

  removePhoto(type: string, i: number) {
    this.photos.removeAt(i);
  }

  onSubmit() {
    if (this.step2FormGroup.valid) { // Validate the step2 form
      const rawValue = this.step2FormGroup.getRawValue();
      console.log('[DEBUG] Step 2 Form raw value:', rawValue);
      // Convert actualQuantity to number if it exists
      const value = {
        ...rawValue,
        actualQuantity: rawValue.actualQuantity ? Number(rawValue.actualQuantity) : null,
        date: new Date().toISOString(),
        foremanName: this.foremanName || 'Foreman'
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
        this.step2FormGroup.reset({ date: new Date(), foremanName: this.foremanName });
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

