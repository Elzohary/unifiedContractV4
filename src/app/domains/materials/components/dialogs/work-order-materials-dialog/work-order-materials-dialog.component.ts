import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { WorkOrderService } from '../../../../work-order/services/work-order.service';
import { materialAssignment, PurchasableMaterial, ReceivableMaterial, SiteUsageRecord, UsageRecord } from '../../../../work-order/models/work-order.model';
import { MaterialUsageDialogComponent, MaterialUsageResult } from '../material-usage-dialog/material-usage-dialog.component';

export interface WorkOrderMaterialsDialogData {
  workOrderNumber: string;
  workOrderTitle: string;
  materials: materialAssignment[];
  workOrderId: string;
}

@Component({
  selector: 'app-work-order-materials-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    MatChipsModule,
    MatSelectModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule,
    MatTooltipModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './work-order-materials-dialog.component.html',
  styleUrls: ['./work-order-materials-dialog.component.scss']
})
export class WorkOrderMaterialsDialogComponent implements OnInit {
  displayedColumns: string[] = ['name', 'code', 'type', 'quantity', 'unit', 'status', 'actions'];
  statusUpdateForm: FormGroup;
  editingMaterialId: string | null = null;
  isLoading = false;

  // Status options for different material types
  purchasableStatusOptions = [
    { value: 'pending', label: 'Pending' },
    { value: 'ordered', label: 'Ordered' },
    { value: 'delivered', label: 'Delivered' },
    { value: 'in-use', label: 'In Use' },
    { value: 'used', label: 'Used' }
  ];

  receivableStatusOptions = [
    { value: 'pending', label: 'Pending' },
    { value: 'ordered', label: 'Ordered' },
    { value: 'received', label: 'Received' },
    { value: 'used', label: 'Used' }
  ];

  constructor(
    public dialogRef: MatDialogRef<WorkOrderMaterialsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: WorkOrderMaterialsDialogData,
    private formBuilder: FormBuilder,
    private workOrderService: WorkOrderService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {
    this.statusUpdateForm = this.formBuilder.group({
      status: ['', Validators.required],
      notes: ['']
    });
  }

  ngOnInit(): void {
    // Initialize form if needed
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'delivered':
      case 'received':
        return 'primary';
      case 'pending':
        return 'warn';
      case 'ordered':
        return 'accent';
      case 'in-use':
        return 'primary';
      case 'used':
        return 'accent';
      default:
        return '';
    }
  }

  getStatusOptions(material: materialAssignment): any[] {
    if (material.purchasableMaterial) {
      return this.purchasableStatusOptions;
    } else if (material.receivableMaterial) {
      return this.receivableStatusOptions;
    }
    return [];
  }

  getCurrentStatus(material: materialAssignment): string {
    if (material.purchasableMaterial) {
      return material.purchasableMaterial.status;
    } else if (material.receivableMaterial) {
      return material.receivableMaterial.status;
    }
    return 'N/A';
  }

  startEdit(material: materialAssignment): void {
    this.editingMaterialId = material.id;
    const currentStatus = this.getCurrentStatus(material);
    this.statusUpdateForm.patchValue({
      status: currentStatus,
      notes: ''
    });
  }

  cancelEdit(): void {
    this.editingMaterialId = null;
    this.statusUpdateForm.reset();
  }

    async updateMaterialStatus(material: materialAssignment): Promise<void> {
    if (this.statusUpdateForm.invalid) {
      return;
    }

    this.isLoading = true;
    const formValue = this.statusUpdateForm.value;

    // Special handling for "used" status - open enhanced usage dialog
    if (formValue.status === 'used') {
      this.handleUsedStatus(material);
      return;
    }

    try {
      // Prepare the update data
      const updateData: Partial<materialAssignment> = {};
      
      if (material.purchasableMaterial) {
        updateData.purchasableMaterial = {
          ...material.purchasableMaterial,
          status: formValue.status
        };
        
        // Add delivery date for delivered status
        if (formValue.status === 'delivered') {
          updateData.purchasableMaterial.delivery = {
            ...material.purchasableMaterial.delivery,
            receivedDate: new Date(),
            receivedBy: 'Current User',
            receivedByName: 'Current User',
            storageLocation: 'warehouse'
          };
        }
      } else if (material.receivableMaterial) {
        updateData.receivableMaterial = {
          ...material.receivableMaterial,
          status: formValue.status
        };
        
        // Add receiving date for received status
        if (formValue.status === 'received') {
          updateData.receivableMaterial.receiving = {
            ...material.receivableMaterial.receiving,
            receivedDate: new Date(),
            materialMan: 'Current User',
            materialManName: 'Current User',
            storageLocation: 'warehouse'
          };
        }
      }

      // Call service to update the material assignment - SINGLE SOURCE OF TRUTH
      this.workOrderService.updateMaterialAssignment(
        this.data.workOrderId, 
        material.id, 
        updateData
      ).subscribe({
        next: () => {
          this.snackBar.open('Material status updated successfully', 'Close', {
            duration: 3000,
            horizontalPosition: 'center',
            verticalPosition: 'bottom'
          });

          this.editingMaterialId = null;
          this.statusUpdateForm.reset();
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error updating material status:', error);
          this.snackBar.open('Failed to update material status', 'Close', {
            duration: 3000,
            horizontalPosition: 'center',
            verticalPosition: 'bottom'
          });
          this.isLoading = false;
        }
      });
    } catch (error) {
      console.error('Error updating material status:', error);
      this.snackBar.open('Failed to update material status', 'Close', {
        duration: 3000,
        horizontalPosition: 'center',
        verticalPosition: 'bottom'
      });
      this.isLoading = false;
    }
  }

  private handleUsedStatus(material: materialAssignment): void {
    // Open the enhanced material usage dialog
    const dialogRef = this.dialog.open(MaterialUsageDialogComponent, {
      width: '900px',
      maxWidth: '95vw',
      height: '90vh',
      data: {
        material: material,
        workOrderId: this.data.workOrderId,
        action: 'update-usage'
      }
    });

    dialogRef.afterClosed().subscribe((result: MaterialUsageResult | undefined) => {
      if (result) {
        this.processUsageResult(material, result);
      } else {
        // User cancelled - reset the form
        this.editingMaterialId = null;
        this.statusUpdateForm.reset();
        this.isLoading = false;
      }
    });
  }

  private processUsageResult(material: materialAssignment, result: MaterialUsageResult): void {
    // Prepare the complete update data - SINGLE SOURCE OF TRUTH
    const updateData: Partial<materialAssignment> = {};
    
    if (material.purchasableMaterial) {
      // Create usage record
      const usageRecord: SiteUsageRecord = {
        id: crypto.randomUUID(),
        recordType: 'usage-update',
        recordDate: new Date(),
        recordedBy: 'Current User',
        recordedByName: 'Current User',
        quantityUsed: result.quantityUsed,
        cumulativeQuantityUsed: result.quantityUsed,
        usagePercentage: (result.quantityUsed / material.purchasableMaterial.quantity) * 100,
        remainingQuantity: material.purchasableMaterial.quantity - result.quantityUsed,
        usageNotes: result.usageNotes
      };

      const newUsageRecords = [usageRecord];

      // Add waste record if applicable
      if (result.quantityWasted && result.quantityWasted > 0) {
        const wasteRecord: SiteUsageRecord = {
          id: crypto.randomUUID(),
          recordType: 'waste',
          recordDate: new Date(),
          recordedBy: 'Current User',
          recordedByName: 'Current User',
          quantityWasted: result.quantityWasted,
          wasteReason: result.wasteReason
        };
        newUsageRecords.push(wasteRecord);
      }

      updateData.purchasableMaterial = {
        ...material.purchasableMaterial,
        status: 'used',
        siteUsageRecords: [
          ...(material.purchasableMaterial.siteUsageRecords || []),
          ...newUsageRecords
        ]
      };
    } else if (material.receivableMaterial) {
      // Create usage record for receivable material
      const usageRecord: UsageRecord = {
        id: crypto.randomUUID(),
        recordType: 'usage-update',
        recordDate: new Date(),
        recordedBy: 'Current User',
        recordedByName: 'Current User',
        quantityUsed: result.quantityUsed,
        cumulativeQuantityUsed: result.quantityUsed,
        usagePercentage: (result.quantityUsed / material.receivableMaterial.estimatedQuantity) * 100,
        remainingQuantity: material.receivableMaterial.estimatedQuantity - result.quantityUsed,
        usageNotes: result.usageNotes
      };

      updateData.receivableMaterial = {
        ...material.receivableMaterial,
        status: 'used',
        usageRecords: [
          ...(material.receivableMaterial.usageRecords || []),
          usageRecord
        ]
      };
    }

    // Call service to update the material assignment - SINGLE SOURCE OF TRUTH
    this.workOrderService.updateMaterialAssignment(
      this.data.workOrderId, 
      material.id, 
      updateData
    ).subscribe({
      next: () => {
        this.snackBar.open('Material usage recorded successfully', 'Close', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom'
        });

        this.editingMaterialId = null;
        this.statusUpdateForm.reset();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error updating material usage:', error);
        this.snackBar.open('Failed to record material usage', 'Close', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom'
        });
        this.isLoading = false;
      }
    });
  }

  isEditing(material: materialAssignment): boolean {
    return this.editingMaterialId === material.id;
  }

  canUpdateStatus(material: materialAssignment): boolean {
    const currentStatus = this.getCurrentStatus(material);
    // Allow updates for all statuses except 'used' (final status)
    return currentStatus !== 'used';
  }
} 