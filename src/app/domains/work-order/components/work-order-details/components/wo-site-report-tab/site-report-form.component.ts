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
import { WorkOrder } from '../../../../models/work-order.model';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

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
    MatDividerModule
  ]
})
export class SiteReportFormComponent {
  @Input() workOrder!: WorkOrder;
  @Output() submitted = new EventEmitter<any>();

  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
    @Optional() private dialogRef: MatDialogRef<SiteReportFormComponent>
  ) {
    this.form = this.fb.group({
      date: [new Date(), Validators.required],
      foremanName: ['', Validators.required],
      notes: [''],
      materialsUsed: this.fb.array([]),
      photos: this.fb.array([])
    });
    if (data && data.workOrder) {
      this.workOrder = data.workOrder;
    }
  }

  get materialsUsed(): FormArray {
    return this.form.get('materialsUsed') as FormArray;
  }

  addMaterial() {
    this.materialsUsed.push(this.fb.group({
      materialName: ['', Validators.required],
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
      if (this.dialogRef) {
        this.dialogRef.close(this.form.value);
      } else {
        this.submitted.emit(this.form.value);
      }
      this.form.reset({ date: new Date() });
      this.materialsUsed.clear();
      this.photos.clear();
    }
  }
} 