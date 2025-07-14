import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatDividerModule } from '@angular/material/divider';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { BaseMaterial } from '../../../models/material.model';
import { WorkOrder } from '../../../../work-order/models/work-order.model';
import { MaterialReallocationService } from '../../../services/material-reallocation.service';

export interface MaterialReallocationDialogData {
  materialId: string;
  materialCode: string;
  materialDescription: string;
  totalAllocated: number;
  totalUsed: number;
  totalRemaining: number;
  unit: string;
}

export interface WorkOrderAllocation {
  workOrderId: string;
  workOrderNumber: string;
  workOrderTitle: string;
  status: string;
  completionPercentage: number;
  allocatedQuantity: number;
  usedQuantity: number;
  remainingQuantity: number;
  canReduce: boolean;
  canIncrease: boolean;
  priority: 'low' | 'medium' | 'high' | 'critical';
}

export interface ReallocationAction {
  fromWorkOrderId?: string;
  toWorkOrderId?: string;
  quantity: number;
  reason: string;
  priority: 'low' | 'medium' | 'high' | 'critical';
}

@Component({
  selector: 'app-material-reallocation-dialog',
  templateUrl: './material-reallocation-dialog.component.html',
  styleUrls: ['./material-reallocation-dialog.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatTableModule,
    MatDividerModule,
    MatChipsModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatTooltipModule
  ]
})
export class MaterialReallocationDialogComponent implements OnInit {
  reallocationForm: FormGroup;
  workOrderAllocations: WorkOrderAllocation[] = [];
  filteredWorkOrders$: Observable<WorkOrderAllocation[]> = of([]);
  
  // Table columns
  displayedColumns = ['workOrder', 'allocation', 'usage', 'actions'];
  
  // Summary data
  totalAllocated = 0;
  totalUsed = 0;
  totalRemaining = 0;
  utilizationRate = 0;
  
  // Reallocation actions
  reallocationActions: ReallocationAction[] = [];
  
  // Loading states
  isLoading = true;
  isSaving = false;

  constructor(
    private fb: FormBuilder,
    private reallocationService: MaterialReallocationService,
    public dialogRef: MatDialogRef<MaterialReallocationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: MaterialReallocationDialogData
  ) {
    this.reallocationForm = this.createForm();
  }

  ngOnInit(): void {
    this.loadWorkOrderAllocations();
  }

  private createForm(): FormGroup {
    return this.fb.group({
      searchTerm: [''],
      filterStatus: [''],
      filterPriority: ['']
    });
  }

  private loadWorkOrderAllocations(): void {
    this.isLoading = true;
    
    this.reallocationService.getWorkOrderAllocations(this.data.materialId).subscribe({
      next: (allocations: WorkOrderAllocation[]) => {
        this.workOrderAllocations = allocations;
        this.filteredWorkOrders$ = of(allocations);
        this.calculateSummaryData();
        this.isLoading = false;
      },
      error: (error: any) => {
        console.error('Error loading work order allocations:', error);
        this.isLoading = false;
      }
    });
  }

  private calculateSummaryData(): void {
    this.totalAllocated = this.workOrderAllocations.reduce((sum, wo) => sum + wo.allocatedQuantity, 0);
    this.totalUsed = this.workOrderAllocations.reduce((sum, wo) => sum + wo.usedQuantity, 0);
    this.totalRemaining = this.totalAllocated - this.totalUsed;
    this.utilizationRate = this.totalAllocated > 0 ? (this.totalUsed / this.totalAllocated) * 100 : 0;
  }

  onSearchChange(): void {
    const searchTerm = this.reallocationForm.get('searchTerm')?.value?.toLowerCase() || '';
    const statusFilter = this.reallocationForm.get('filterStatus')?.value || '';
    const priorityFilter = this.reallocationForm.get('filterPriority')?.value || '';

    this.filteredWorkOrders$ = of(this.workOrderAllocations.filter(wo => {
      const matchesSearch = !searchTerm || 
        wo.workOrderNumber.toLowerCase().includes(searchTerm) ||
        wo.workOrderTitle.toLowerCase().includes(searchTerm);
      
      const matchesStatus = !statusFilter || wo.status === statusFilter;
      const matchesPriority = !priorityFilter || wo.priority === priorityFilter;

      return matchesSearch && matchesStatus && matchesPriority;
    }));
  }

  reduceAllocation(workOrder: WorkOrderAllocation): void {
    const maxReduce = workOrder.allocatedQuantity - workOrder.usedQuantity;
    if (maxReduce <= 0) return;

    const quantity = Math.min(maxReduce, 10); // Default reduce by 10 or remaining quantity
    
    this.reallocationActions.push({
      fromWorkOrderId: workOrder.workOrderId,
      quantity: quantity,
      reason: 'Optimization',
      priority: 'medium'
    });

    // Update the work order allocation
    workOrder.allocatedQuantity -= quantity;
    workOrder.remainingQuantity = workOrder.allocatedQuantity - workOrder.usedQuantity;
    
    this.calculateSummaryData();
  }

  increaseAllocation(workOrder: WorkOrderAllocation): void {
    const quantity = 10; // Default increase by 10
    
    this.reallocationActions.push({
      toWorkOrderId: workOrder.workOrderId,
      quantity: quantity,
      reason: 'Priority adjustment',
      priority: workOrder.priority
    });

    // Update the work order allocation
    workOrder.allocatedQuantity += quantity;
    workOrder.remainingQuantity = workOrder.allocatedQuantity - workOrder.usedQuantity;
    
    this.calculateSummaryData();
  }

  transferAllocation(fromWorkOrder: WorkOrderAllocation, toWorkOrder: WorkOrderAllocation): void {
    const maxTransfer = fromWorkOrder.allocatedQuantity - fromWorkOrder.usedQuantity;
    if (maxTransfer <= 0) return;

    const quantity = Math.min(maxTransfer, 10); // Default transfer 10 or remaining quantity
    
    this.reallocationActions.push({
      fromWorkOrderId: fromWorkOrder.workOrderId,
      toWorkOrderId: toWorkOrder.workOrderId,
      quantity: quantity,
      reason: 'Transfer between work orders',
      priority: toWorkOrder.priority
    });

    // Update both work order allocations
    fromWorkOrder.allocatedQuantity -= quantity;
    fromWorkOrder.remainingQuantity = fromWorkOrder.allocatedQuantity - fromWorkOrder.usedQuantity;
    
    toWorkOrder.allocatedQuantity += quantity;
    toWorkOrder.remainingQuantity = toWorkOrder.allocatedQuantity - toWorkOrder.usedQuantity;
    
    this.calculateSummaryData();
  }

  removeReallocationAction(action: ReallocationAction): void {
    const index = this.reallocationActions.indexOf(action);
    if (index > -1) {
      this.reallocationActions.splice(index, 1);
      
      // Revert the changes
      this.loadWorkOrderAllocations();
    }
  }

  getUtilizationPercentage(workOrder: WorkOrderAllocation): number {
    return workOrder.allocatedQuantity > 0 ? (workOrder.usedQuantity / workOrder.allocatedQuantity) * 100 : 0;
  }

  getUtilizationColor(workOrder: WorkOrderAllocation): string {
    const percentage = this.getUtilizationPercentage(workOrder);
    if (percentage > 90) return 'warn';
    if (percentage > 70) return 'accent';
    return 'primary';
  }

  getPriorityColor(priority: string): string {
    switch (priority) {
      case 'critical': return 'warn';
      case 'high': return 'accent';
      case 'medium': return 'primary';
      default: return '';
    }
  }

  onConfirm(): void {
    if (this.reallocationActions.length === 0) {
      this.dialogRef.close();
      return;
    }

    this.isSaving = true;
    
    this.reallocationService.applyReallocations(this.data.materialId, this.reallocationActions).subscribe({
      next: (result: any) => {
        this.isSaving = false;
        this.dialogRef.close({
          success: true,
          reallocations: this.reallocationActions,
          summary: {
            totalAllocated: this.totalAllocated,
            totalUsed: this.totalUsed,
            totalRemaining: this.totalRemaining,
            utilizationRate: this.utilizationRate
          }
        });
      },
      error: (error: any) => {
        console.error('Error applying reallocations:', error);
        this.isSaving = false;
        // Don't close dialog on error, let user retry
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  clearFilters(): void {
    this.reallocationForm.patchValue({
      searchTerm: '',
      filterStatus: '',
      filterPriority: ''
    });
    this.onSearchChange();
  }
} 