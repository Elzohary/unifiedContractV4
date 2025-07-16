import { Component, EventEmitter, Inject, Input, Output, Optional, AfterViewInit, ViewChild, ChangeDetectorRef } from '@angular/core';
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
import { MatStepper } from '@angular/material/stepper';

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
export class SiteReportFormComponent implements AfterViewInit {
  @Input() workOrder!: WorkOrder;
  @Output() submitted = new EventEmitter<any>();
  @Output() updated = new EventEmitter<any>();
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
  @ViewChild('stepper') stepper!: MatStepper;
  startStep: number = 0;
  constructor(
    private fb: FormBuilder,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
    @Optional() private dialogRef: MatDialogRef<SiteReportFormComponent>,
    private userService: UserService,
    private workOrderService: WorkOrderService,
    private cdr: ChangeDetectorRef
  ) {
    if (data && data.workOrder) {
      this.workOrder = data.workOrder;
    }
    if (data && typeof data.startStep === 'number') {
      this.startStep = data.startStep;
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

    // If editing an existing report, pre-fill all form groups
    if (data && data.report) {
      const report = data.report;
      // Step 1: Safety Photos
      this.safetyPhotos = (report.photos || []).filter((p: any) => p.category === 'safety').map((p: any) => ({ ...p, preview: p.url }));
      this.step1FormGroup.get('safetyPhotos')?.setValue(this.safetyPhotos);
      // Step 2: Work Done, Materials, Progress Photos
      this.step2FormGroup.patchValue({
        workDone: report.workDone,
        workDoneOther: report.workDoneOther || '',
        actualQuantity: report.actualQuantity,
        notes: report.notes || '',
        progressPhotos: (report.photos || []).filter((p: any) => p.category === 'progress').map((p: any) => ({ ...p, preview: p.url }))
      });
      this.progressPhotosList = (report.photos || []).filter((p: any) => p.category === 'progress').map((p: any) => ({ ...p, preview: p.url }));
      // Materials Used
      const materialsArray = this.step2FormGroup.get('materialsUsed') as FormArray;
      materialsArray.clear();
      (report.materialsUsed || []).forEach((m: any) => {
        materialsArray.push(this.fb.group({
          materialId: m.materialId,
          quantity: m.quantity
        }));
      });
      // Step 3: Housekeeping Photos
      this.housekeepingPhotos = new HousekeepingPhotosArray(...((report.photos || []).filter((p: any) => p.category === 'housekeeping').map((p: any) => ({ ...p, preview: p.url }))));
      this.step3FormGroup.get('housekeepingPhotos')?.setValue(this.housekeepingPhotos);
      // Main form
      this.form.patchValue({
        date: report.date ? new Date(report.date) : new Date(),
        foremanName: report.foremanName || this.foremanName
      });
    }
  }

  ngAfterViewInit() {
    if (this.stepper && typeof this.startStep === 'number') {
      setTimeout(() => {
        this.stepper.selectedIndex = this.startStep;
        this.cdr.detectChanges();
      });
    }
    // Debug logs for dropdowns
    console.log('[DEBUG] workDoneOptions:', this.workDoneOptions);
    console.log('[DEBUG] materialOptions:', this.materialOptions);
  }

  onSafetyPhotosChange(event: File[]) {
    // Always update both the local array and the form control
    this.safetyPhotos = Array.isArray(event) ? [...event] : [];
    this.step1FormGroup.get('safetyPhotos')?.setValue(this.safetyPhotos);
    this.step1FormGroup.get('safetyPhotos')?.markAsTouched();
    this.step1FormGroup.get('safetyPhotos')?.updateValueAndValidity();
    console.log('[DEBUG] onSafetyPhotosChange event:', event);
    console.log('[DEBUG] safetyPhotos local:', this.safetyPhotos);
    console.log('[DEBUG] safetyPhotos form value:', this.step1FormGroup.get('safetyPhotos')?.value);
    console.log('[DEBUG] safetyPhotos form valid:', this.step1FormGroup.get('safetyPhotos')?.valid);
  }

  onHousekeepingPhotosChange(event: File[]) {
    this.housekeepingPhotos = Array.isArray(event) ? new HousekeepingPhotosArray(...event) : new HousekeepingPhotosArray();
    this.step3FormGroup.get('housekeepingPhotos')?.setValue([...event]);
    this.step3FormGroup.get('housekeepingPhotos')?.markAsTouched();
    this.step3FormGroup.get('housekeepingPhotos')?.updateValueAndValidity();
    console.log('[DEBUG] onHousekeepingPhotosChange event:', event);
    console.log('[DEBUG] housekeepingPhotos local:', this.housekeepingPhotos);
    console.log('[DEBUG] housekeepingPhotos form value:', this.step3FormGroup.get('housekeepingPhotos')?.value);
    console.log('[DEBUG] housekeepingPhotos form valid:', this.step3FormGroup.get('housekeepingPhotos')?.valid);
  }

  onProgressPhotosChange(event: File[]) {
    this.progressPhotosList = Array.isArray(event) ? [...event] : [];
    this.step2FormGroup.get('progressPhotos')?.setValue(this.progressPhotosList);
    this.step2FormGroup.get('progressPhotos')?.markAsTouched();
    this.step2FormGroup.get('progressPhotos')?.updateValueAndValidity();
    console.log('[DEBUG] onProgressPhotosChange event:', event);
    console.log('[DEBUG] progressPhotosList local:', this.progressPhotosList);
    console.log('[DEBUG] progressPhotos form value:', this.step2FormGroup.get('progressPhotos')?.value);
    console.log('[DEBUG] progressPhotos form valid:', this.step2FormGroup.get('progressPhotos')?.valid);
  }
  saveReport(status: 'Open' | 'Closed', shouldCloseDialog: boolean = true) {
    let currentStep = 0;
    if (this.stepper) {
      currentStep = this.stepper.selectedIndex;
    }
    this.onSubmit(status, currentStep, shouldCloseDialog);
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

  onSubmit(statusOverride?: 'Open' | 'Closed', currentStep?: number, shouldCloseDialog: boolean = true) {
    // Determine which form group to validate
    let valid = false;
    if (typeof currentStep === 'number') {
      if (currentStep === 0) valid = this.step1FormGroup.valid;
      else if (currentStep === 1) valid = this.step2FormGroup.valid;
      else if (currentStep === 2) valid = this.step3FormGroup.valid;
      else valid = this.step2FormGroup.valid;
    } else {
      valid = this.step2FormGroup.valid;
    }
    if (valid) {
      // Use the same logic as before, but only require the current step to be valid
      const rawValue = this.step2FormGroup.getRawValue();
      // Merge all photo types into a single array with category
      const safetyPhotos = (this.step1FormGroup.get('safetyPhotos')?.value || []).map((file: any) => ({
        url: file.url || file,
        caption: file.caption || '',
        category: 'safety'
      }));
      const progressPhotos = (this.step2FormGroup.get('progressPhotos')?.value || []).map((file: any) => ({
        url: file.url || file,
        caption: file.caption || '',
        category: 'progress'
      }));
      const housekeepingPhotos = (this.step3FormGroup.get('housekeepingPhotos')?.value || []).map((file: any) => ({
        url: file.url || file,
        caption: file.caption || '',
        category: 'housekeeping'
      }));
      const allPhotos = [...safetyPhotos, ...progressPhotos, ...housekeepingPhotos];
      // Convert actualQuantity to number if it exists
      const value: any = {
        ...rawValue,
        actualQuantity: rawValue.actualQuantity ? Number(rawValue.actualQuantity) : null,
        date: new Date().toISOString(),
        foremanName: this.foremanName || 'Foreman',
        photos: allPhotos,
        status: statusOverride === 'Open' ? this.SiteReportStatus.Open : this.SiteReportStatus.Closed
      };
      // If editing, include the report id
      if (this.data && this.data.report && this.data.report.id) {
        value.id = this.data.report.id;
      }
      console.log('[DEBUG] Emitting updated report value:', value);
      setTimeout(() => {
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
        this.updated.emit(value); // Always emit the report object for update
        if (shouldCloseDialog && this.dialogRef) {
          this.dialogRef.close(value);
        } else if (!shouldCloseDialog) {
          // Do not close dialog, just emit update
        } else {
          this.submitted.emit(value);
        }
        // Properly reset all photo arrays and FormArrays only if closing dialog
        if (shouldCloseDialog) {
          this.step1FormGroup.get('safetyPhotos')?.setValue([]);
          this.safetyPhotos = [];
          this.step2FormGroup.get('progressPhotos')?.setValue([]);
          this.progressPhotosList = [];
          this.step3FormGroup.get('housekeepingPhotos')?.setValue([]);
          this.housekeepingPhotos = new HousekeepingPhotosArray();
          const materialsArray = this.step2FormGroup.get('materialsUsed') as FormArray;
          materialsArray?.clear();
        }
        // Remove invalid this.photos.clear();
      });
    }
  }

  // Auto-save draft and move to next step for progress photos step
  autoSaveAndNext() {
    if (this.step2FormGroup.valid) {
      this.saveReport('Open', false); // Do not close dialog
      if (this.stepper) {
        setTimeout(() => this.stepper.next(), 0);
      }
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

