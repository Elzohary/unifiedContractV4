import { Component, Input, ChangeDetectorRef, Output, EventEmitter, OnChanges, SimpleChanges, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { WorkOrderService } from '../../../../services/work-order.service';
import { AssignWorkOrderItemDialogComponent } from '../../../work-order-item-dialog/assign-work-order-item-dialog.component';
import { WorkOrder, Permit } from '../../../../models/work-order.model';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ManualWorkOrderItemDialogComponent } from '../manual-work-order-item-dialog.component';
import { WorkOrderPermitsChecklistDialogComponent } from '../work-order-permits-checklist-dialog.component';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
// Removed import of ManualWorkOrderItemDialogComponent due to missing module or type declarations

interface PermitStatus {
  type: string;
  isApproved: boolean;
}

@Component({
  selector: 'app-wo-overview-tab',
  templateUrl: './wo-overview-tab.component.html',
  styleUrls: ['./wo-overview-tab.component.scss'],
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    MatCardModule,
    MatDividerModule,
    MatIconModule,
    MatListModule,
    MatProgressBarModule,
    MatTableModule,
    MatButtonModule,
    MatDialogModule,
    AssignWorkOrderItemDialogComponent,
    MatTooltipModule,
    FormsModule,
    MatFormFieldModule
  ]
})
export class WoOverviewTabComponent implements OnChanges {
  @Input() workOrder!: WorkOrder;
  @Output() permitsChanged = new EventEmitter<{type: string, status: string}[]>();
  @Output() itemsChanged = new EventEmitter<void>();

  constructor(
    public authService: AuthService,
    private dialog: MatDialog,
    private workOrderService: WorkOrderService,
    private cdr: ChangeDetectorRef
  ) {
    console.log('WoOverviewTabComponent loaded - UNIQUE');
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['workOrder']) {
      console.log('[DEBUG] WoOverviewTabComponent received new workOrder:', changes['workOrder'].currentValue);
      this.cdr.markForCheck();
    }
  }

  // Table columns
  displayedColumns: string[] = [
    'itemNumber',
    'description',
    'uom',
    'estimatedQty',
    'estimatedPrice',
    'actualQty',
    'actualPrice'
  ];

  get canAddItems(): boolean {
    return this.authService.hasPermission('work-orders.edit');
  }

  addItem() {
    const client = this.workOrder?.details?.client?.toLowerCase?.() || '';
    if (client === 'sec' || client === 'saudi electricity company') {
      const dialogRef = this.dialog.open(AssignWorkOrderItemDialogComponent, {
        width: '600px',
        data: {
          title: 'Assign Work Order Item'
        }
      });
      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.workOrderService.addItemToWorkOrder(this.workOrder.id, result).subscribe(() => {
            this.itemsChanged.emit();
          });
        }
      });
    } else {
      const dialogRef = this.dialog.open(ManualWorkOrderItemDialogComponent, {
        width: '600px',
        data: {
          title: 'Add Work Order Item'
        }
      });
      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.workOrderService.addItemToWorkOrder(this.workOrder.id, result).subscribe(() => {
            this.itemsChanged.emit();
          });
        }
      });
    }
  }

  assignItem() {
    const dialogRef = this.dialog.open(AssignWorkOrderItemDialogComponent, {
      width: '600px',
      data: {
        title: 'Assign Work Order Item',
        workOrderId: this.workOrder.id,
        assignedItemIds: this.workOrder.items?.map(item => item.id) || []
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.workOrderService.addItemToWorkOrder(this.workOrder.id, result).subscribe(() => {
          this.itemsChanged.emit();
        });
      }
    });
  }

  getTotalEstimatedPrice(): number {
    if (!this.workOrder.items || this.workOrder.items.length === 0) return 0;
    return this.workOrder.items.reduce((total, item) => {
      const estimatedPrice = item.estimatedPrice || 0;
      const estimatedQty = item.estimatedQuantity || 0;
      return total + (estimatedPrice);
    }, 0);
  }

  getPermitStatus(type: string): boolean {
    if (!this.workOrder.permits) return false;
    const permit = this.workOrder.permits.find(p => p.type === type);
    return permit?.status?.toLowerCase() === 'approved';
  }

  formatDate(date?: string | Date): string {
    if (!date) return 'Not specified';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  getProgressColor(): string {
    const percentage = this.workOrder.details.completionPercentage;
    if (percentage < 30) return 'warn';
    if (percentage < 70) return 'accent';
    return 'primary';
  }

  getTotalExpense(): number {
    if (!this.workOrder.expenseBreakdown) return 0;
    return (
      this.workOrder.expenseBreakdown.materials +
      this.workOrder.expenseBreakdown.labor +
      this.workOrder.expenseBreakdown.other
    );
  }

  openPermitChecklistDialog() {
    const dialogRef = this.dialog.open(WorkOrderPermitsChecklistDialogComponent, {
      width: '400px',
      data: {
        permits: this.workOrder.permits || []
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.workOrderService.updateWorkOrderPermits(this.workOrder.id, result).subscribe({
          next: () => {
            this.workOrderService.getWorkOrderById(this.workOrder.id).subscribe(updatedWorkOrder => {
              this.workOrder = updatedWorkOrder;
              this.cdr.markForCheck();
            });
          },
          error: (err) => console.error('Failed to update permits', err)
        });
      }
    });
  }

  // --- Price Details Cards Logic ---

  getEstimatedItemsDetails() {
    const items = this.workOrder.items || [];
    const totalEstimatedPrice = items.reduce((sum, item) => sum + (item.estimatedPrice || 0), 0);
    const estimationVAT = totalEstimatedPrice * 0.15;
    const totalEstimatedPriceWithVAT = totalEstimatedPrice + estimationVAT;
    return {
      totalEstimatedPrice,
      estimationVAT,
      totalEstimatedPriceWithVAT
    };
  }

  getActualItemsDetails() {
    const items = this.workOrder.items || [];
    const totalActualPrice = items.reduce((sum, item) => sum + (item.actualPrice || 0), 0);
    const actualVAT = totalActualPrice * 0.15;
    const totalActualPriceWithVAT = totalActualPrice + actualVAT;
    // Partial payment: 50% for now
    const partialPaymentTotalActual = totalActualPrice * 0.5;
    const partialPaymentVAT = actualVAT * 0.5;
    const partialPaymentTotalActualWithVAT = partialPaymentTotalActual + partialPaymentVAT;
    return {
      totalActualPrice,
      actualVAT,
      totalActualPriceWithVAT,
      partialPaymentTotalActual,
      partialPaymentVAT,
      partialPaymentTotalActualWithVAT
    };
  }

  getFinalItemsDetails() {
    const items = this.workOrder.items || [];
    // For now, use actualPrice as finalPrice (can be changed if a final field is added)
    const totalFinalPrice = items.reduce((sum, item) => sum + (item.actualPrice || 0), 0);
    const finalVAT = totalFinalPrice * 0.15;
    const totalFinalPriceWithVAT = totalFinalPrice + finalVAT;
    // Partial payment: 50% for now
    const partialPaymentTotalFinal = totalFinalPrice * 0.5;
    const partialPaymentFinalVAT = finalVAT * 0.5;
    const partialPaymentTotalFinalWithVAT = partialPaymentTotalFinal + partialPaymentFinalVAT;
    // Invoice status
    let fullyInvoiced = false, partiallyInvoiced = false, notInvoiced = true;
    const invoices = this.workOrder.invoices || [];
    if (invoices.length > 0) {
      const paid = invoices.filter(inv => inv.status === 'paid');
      const sent = invoices.filter(inv => inv.status === 'sent' || inv.status === 'draft');
      if (paid.length && paid.reduce((sum, inv) => sum + (inv.amount || 0), 0) >= totalFinalPriceWithVAT) {
        fullyInvoiced = true; notInvoiced = false;
      } else if (paid.length || sent.length) {
        partiallyInvoiced = true; notInvoiced = false;
      }
    }
    let invoiceStatus = 'Not Invoiced';
    if (fullyInvoiced) invoiceStatus = 'Fully Invoiced';
    else if (partiallyInvoiced) invoiceStatus = 'Partially Invoiced';
    return {
      totalFinalPrice,
      finalVAT,
      totalFinalPriceWithVAT,
      partialPaymentTotalFinal,
      partialPaymentFinalVAT,
      partialPaymentTotalFinalWithVAT,
      invoiceStatus
    };
  }

  // --- Table Totals and Editable Actual Quantity ---
  getItemsTotal(field: string): number {
    if (!this.workOrder.items) return 0;
    switch (field) {
      case 'estimatedQuantity':
        return this.workOrder.items.reduce((sum, item) => sum + (item.estimatedQuantity || 0), 0);
      case 'estimatedPrice':
        return this.workOrder.items.reduce((sum, item) => sum + (item.estimatedPrice || 0), 0);
      case 'actualQuantity':
        return this.workOrder.items.reduce((sum, item) => sum + (item.actualQuantity || 0), 0);
      case 'actualPrice':
        return this.workOrder.items.reduce((sum, item) => sum + (item.actualPrice || 0), 0);
      default:
        return 0;
    }
  }

  editActualQtyIndex: number | null = null;
  tempActualQty: number | null = null;

  startEditActualQty(index: number, item: any) {
    this.editActualQtyIndex = index;
    this.tempActualQty = item.actualQuantity;
  }

  cancelEditActualQty() {
    this.editActualQtyIndex = null;
    this.tempActualQty = null;
  }

  saveActualQuantity(item: any, index: number): void {
    if (this.tempActualQty != null) {
      item.actualQuantity = this.tempActualQty;
      // Automatically update actual price
      if (item.itemDetail && typeof item.itemDetail.unitPrice === 'number') {
        item.actualPrice = item.actualQuantity * item.itemDetail.unitPrice;
      }
      // TODO: Implement backend update logic here
      console.log('Save actual quantity for item', item.id, '->', item.actualQuantity, 'Actual Price:', item.actualPrice, 'Completion:', this.completionPercentage);
    }
    this.editActualQtyIndex = null;
    this.tempActualQty = null;
  }

  // Always calculate completion percentage from items
  get completionPercentage(): number {
    // Prefer the backend value if it's a valid number
    if (this.workOrder && this.workOrder.details && typeof this.workOrder.details.completionPercentage === 'number') {
      return Math.round(this.workOrder.details.completionPercentage);
    }
    
    // Fallback to frontend calculation if backend value is not available
    if (!this.workOrder || !this.workOrder.items || this.workOrder.items.length === 0) return 0;
    const totalActual = this.workOrder.items.reduce((sum, it) => sum + (it.actualQuantity || 0), 0);
    const totalEstimated = this.workOrder.items.reduce((sum, it) => sum + (it.estimatedQuantity || 0), 0);
    if (totalEstimated === 0) return 0;
    return Math.round((totalActual / totalEstimated) * 100);
  }

  partialPaymentAmount: number = 0;
  partialPaymentVAT: number = 0;
  partialPaymentTotalWithVAT: number = 0;
  editPartialPayment: boolean = false;
  tempPartialPaymentAmount: number = 0;

  onPartialAmountChange() {
    // Calculate VAT and total
    this.partialPaymentVAT = this.partialPaymentAmount * 0.15;
    this.partialPaymentTotalWithVAT = this.partialPaymentAmount + this.partialPaymentVAT;
    // Update invoice status if partial amount is entered
    if (this.partialPaymentAmount > 0 && this.workOrder && this.workOrder.details) {
      this.workOrder.details['invoiceStatus'] = 'Partially Invoiced';
    } else if (this.workOrder && this.workOrder.details) {
      this.workOrder.details['invoiceStatus'] = 'Not Invoiced';
    }
    this.cdr.markForCheck();
  }

  startEditPartialPayment() {
    this.tempPartialPaymentAmount = this.partialPaymentAmount;
    this.editPartialPayment = true;
  }

  cancelEditPartialPayment() {
    this.editPartialPayment = false;
  }

  savePartialPayment() {
    this.partialPaymentAmount = this.tempPartialPaymentAmount;
    this.editPartialPayment = false;
    this.onPartialAmountChange();
  }
} 