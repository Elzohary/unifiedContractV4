import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTabsModule } from '@angular/material/tabs';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { materialAssignment, PurchasableMaterial, ReceivableMaterial, SiteUsageRecord, UsageRecord } from '../../../../work-order/models/work-order.model';

export interface MaterialUsageDialogData {
  material: materialAssignment;
  workOrderId: string;
  action: 'update-usage' | 'view-usage';
}

export interface MaterialUsageResult {
  status: 'used';
  quantityUsed: number;
  quantityWasted?: number;
  quantityReturned?: number;
  wasteReason?: string;
  returnReason?: string;
  disposition: 'waste' | 'warehouse' | 'reserve-for-workorder';
  photos?: File[];
}

@Component({
  selector: 'app-material-usage-dialog',
  templateUrl: './material-usage-dialog.component.html',
  styleUrls: ['./material-usage-dialog.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatTableModule,
    MatProgressBarModule,
    MatChipsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatRadioModule,
    MatCheckboxModule,
    MatTabsModule,
    MatSnackBarModule,
    MatTooltipModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class MaterialUsageDialogComponent implements OnInit {
  usageForm: FormGroup;
  selectedTab = 0;
  isLoading = false;
  uploadedPhotos: File[] = [];

  // Material info
  materialName = '';
  materialType = '';
  totalQuantity = 0;
  unit = '';
  currentStatus = '';

  // Usage tracking
  quantityUsed = 0;
  quantityWasted = 0;
  quantityReturned = 0;
  quantityRemaining = 0;
  usagePercentage = 0;

  // For file previews
  URL = URL;

  constructor(
    public dialogRef: MatDialogRef<MaterialUsageDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: MaterialUsageDialogData,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar
  ) {
    this.usageForm = this.formBuilder.group({
      quantityUsed: [0, [Validators.required, Validators.min(0)]],
      quantityWasted: [0, [Validators.min(0)]],
      quantityReturned: [0, [Validators.min(0)]],
      wasteReason: [''],
      returnReason: [''],
      disposition: ['waste', Validators.required],
      hasWaste: [false],
      hasReturns: [false]
    });

    this.initializeMaterialInfo();
  }

  ngOnInit(): void {
    this.setupFormListeners();
  }

  private initializeMaterialInfo(): void {
    const material = this.data.material;
    
    if (material.purchasableMaterial) {
      this.materialName = material.purchasableMaterial.name;
      this.materialType = 'Purchasable';
      this.totalQuantity = material.purchasableMaterial.quantity;
      this.unit = material.purchasableMaterial.unit;
      this.currentStatus = material.purchasableMaterial.status;
      
      // Set initial values
      this.usageForm.patchValue({
        quantityUsed: this.totalQuantity
      });
    } else if (material.receivableMaterial) {
      this.materialName = material.receivableMaterial.name;
      this.materialType = 'Receivable';
      this.totalQuantity = material.receivableMaterial.estimatedQuantity;
      this.unit = material.receivableMaterial.unit;
      this.currentStatus = material.receivableMaterial.status;
      
      // Set initial values
      this.usageForm.patchValue({
        quantityUsed: this.totalQuantity
      });
    }
  }

  private setupFormListeners(): void {
    // Listen to quantity changes to calculate remaining
    this.usageForm.get('quantityUsed')?.valueChanges.subscribe(used => {
      this.quantityUsed = used || 0;
      this.calculateRemaining();
    });

    this.usageForm.get('quantityWasted')?.valueChanges.subscribe(wasted => {
      this.quantityWasted = wasted || 0;
      this.calculateRemaining();
    });

    this.usageForm.get('quantityReturned')?.valueChanges.subscribe(returned => {
      this.quantityReturned = returned || 0;
      this.calculateRemaining();
    });

    // Listen to waste/return checkboxes
    this.usageForm.get('hasWaste')?.valueChanges.subscribe(hasWaste => {
      if (!hasWaste) {
        this.usageForm.patchValue({ quantityWasted: 0, wasteReason: '' });
      }
    });

    this.usageForm.get('hasReturns')?.valueChanges.subscribe(hasReturns => {
      if (!hasReturns) {
        this.usageForm.patchValue({ quantityReturned: 0, returnReason: '' });
      }
    });
  }

  private calculateRemaining(): void {
    this.quantityRemaining = this.totalQuantity - this.quantityUsed - this.quantityWasted - this.quantityReturned;
    this.usagePercentage = this.totalQuantity > 0 ? (this.quantityUsed / this.totalQuantity) * 100 : 0;
  }

  onFileSelected(event: any): void {
    const files = event.target.files;
    if (files) {
      for (let i = 0; i < files.length; i++) {
        this.uploadedPhotos.push(files[i]);
      }
    }
  }

  removePhoto(index: number): void {
    this.uploadedPhotos.splice(index, 1);
  }

  validateForm(): boolean {
    const formValue = this.usageForm.value;
    
    // Check if total quantities don't exceed total
    const totalUsed = formValue.quantityUsed + formValue.quantityWasted + formValue.quantityReturned;
    if (totalUsed > this.totalQuantity) {
      this.snackBar.open('Total quantities cannot exceed the total material quantity', 'Close', { duration: 3000 });
      return false;
    }

    // Check if waste reason is provided when there's waste
    if (formValue.hasWaste && formValue.quantityWasted > 0 && !formValue.wasteReason) {
      this.snackBar.open('Please provide a reason for waste', 'Close', { duration: 3000 });
      return false;
    }

    // Check if return reason is provided when there are returns
    if (formValue.hasReturns && formValue.quantityReturned > 0 && !formValue.returnReason) {
      this.snackBar.open('Please provide a reason for returns', 'Close', { duration: 3000 });
      return false;
    }

    return true;
  }

  async saveUsage(): Promise<void> {
    if (!this.validateForm()) {
      return;
    }

    this.isLoading = true;
    const formValue = this.usageForm.value;

    try {
      const result: MaterialUsageResult = {
        status: 'used',
        quantityUsed: formValue.quantityUsed,
        quantityWasted: formValue.hasWaste ? formValue.quantityWasted : undefined,
        quantityReturned: formValue.hasReturns ? formValue.quantityReturned : undefined,
        wasteReason: formValue.hasWaste ? formValue.wasteReason : undefined,
        returnReason: formValue.hasReturns ? formValue.returnReason : undefined,
        disposition: formValue.disposition,
        photos: this.uploadedPhotos.length > 0 ? this.uploadedPhotos : undefined
      };

      this.dialogRef.close(result);
    } catch (error) {
      console.error('Error saving usage:', error);
      this.snackBar.open('Failed to save usage data', 'Close', { duration: 3000 });
    } finally {
      this.isLoading = false;
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }

  getDispositionOptions(): any[] {
    const options = [
      { value: 'waste', label: 'Dispose as Waste', icon: 'delete' },
      { value: 'warehouse', label: 'Return to Warehouse', icon: 'warehouse' }
    ];

    // Add reserve option only if there are remaining materials
    if (this.quantityRemaining > 0) {
      options.push({ 
        value: 'reserve-for-workorder', 
        label: 'Reserve for Same Work Order', 
        icon: 'bookmark' 
      });
    }

    return options;
  }

  getWasteReasons(): string[] {
    return [
      'Damaged during installation',
      'Quality issues',
      'Wrong specifications',
      'Expired/Outdated',
      'Safety concerns',
      'Environmental disposal required',
      'Other'
    ];
  }

  getReturnReasons(): string[] {
    return [
      'Not needed for this work order',
      'Wrong specifications received',
      'Quality issues',
      'Over-ordered',
      'Work order cancelled',
      'Other'
    ];
  }
} 