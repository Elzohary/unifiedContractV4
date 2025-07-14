import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject, takeUntil, take } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

// Angular Material Imports
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { MatBadgeModule } from '@angular/material/badge';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { A11yModule } from '@angular/cdk/a11y';

// ViewModels and Services
import { MaterialInventoryViewModel, StockAlert } from '../../viewModels/material-inventory.viewmodel';
import { MaterialMovement } from '../../models/inventory.model';
import { MaterialManagementService } from '../../services/material-management.service';
import { MaterialService } from '../../services/material.service';
import { BreadcrumbComponent } from '../../../../shared/components/breadcrumb/breadcrumb.component';
import { MockDatabaseService } from '../../../../core/services/mock-database.service';
import { WorkOrder } from '../../../work-order/models/work-order.model';
import { BaseMaterial } from '../../models/material.model';

// Dialogs
import { StockAdjustmentDialogComponent } from '../dialogs/stock-adjustment-dialog/stock-adjustment-dialog.component';
import { MaterialRequisitionDialogComponent } from '../dialogs/material-requisition-dialog/material-requisition-dialog.component';

@Component({
  selector: 'app-material-inventory-dashboard',
  templateUrl: './material-inventory-dashboard.component.html',
  styleUrls: ['./material-inventory-dashboard.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatTabsModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatChipsModule,
    MatTooltipModule,
    MatMenuModule,
    MatDividerModule,
    MatBadgeModule,
    MatProgressBarModule,
    MatSnackBarModule,
    MatDialogModule,
    A11yModule,
    BreadcrumbComponent
  ],
  providers: [MaterialInventoryViewModel]
})
export class MaterialInventoryDashboardComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  // Observable streams from ViewModel
  dashboardData$ = this.viewModel.dashboardData$;
  loading$ = this.viewModel.loading$;
  error$ = this.viewModel.error$;
  warehouses$ = this.viewModel.warehouses$;

  // Table columns for recent movements
  movementColumns: string[] = ['movementNumber', 'material', 'type', 'quantity', 'location', 'date', 'actions'];

  // Selected tab
  selectedTab = 0;

  // Filter state
  workOrders: any[] = [];
  materials: any[] = [];
  selectedWorkOrder: string | null = null;
  selectedMaterial: string | null = null;

  groupedTimeline$ = this.viewModel.groupedTimeline$;

  // Table state for All Materials in Work Orders
  selectedType: string | null = null;
  selectedStatus: string | null = null;
  materialStatuses: string[] = [
    'Pending', 'Ordered', 'Delivered to Warehouse', 'Delivered to Site', 'Installed', 'Wastage',
    'Received', 'Not Received Yet', 'At Warehouse', 'Returned', 'Partially Used', 'In Use', 'Completed'
  ];

  // Table columns
  materialsTableColumns: string[] = ['materialName', 'workOrderNumber', 'type', 'status', 'usage'];

  // Grouping toggle (could be a UI toggle in the future)
  groupByWorkOrder = false;

  // Data for the table
  filteredMaterialsInWorkOrders: any[] = [];
  groupedMaterialsInWorkOrders: any[] = [];

  constructor(
    private viewModel: MaterialInventoryViewModel,
    private router: Router,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private materialManagementService: MaterialManagementService,
    private materialService: MaterialService,
    private activatedRoute: ActivatedRoute,
    private mockDb: MockDatabaseService
  ) {}

  ngOnInit(): void {
    // Subscribe to error messages
    this.error$.pipe(
      takeUntil(this.destroy$)
    ).subscribe(error => {
      if (error) {
        this.snackBar.open(error, 'Close', {
          duration: 5000,
          panelClass: ['error-snackbar']
        });
      }
    });

    // Handle query parameters from materials hub
    this.activatedRoute.queryParams.pipe(
      takeUntil(this.destroy$)
    ).subscribe(params => {
      if (params['action']) {
        // Delay to ensure component is fully initialized
        setTimeout(() => {
          switch (params['action']) {
            case 'stock-adjustment':
              this.createStockAdjustment();
              break;
            case 'requisition':
              this.createMaterialRequisition();
              break;
            default:
              break;
          }
          // Clear the query parameter after action
          this.router.navigate([], {
            relativeTo: this.activatedRoute,
            queryParams: {},
            replaceUrl: true
          });
        }, 500);
      }
    });

    // Load work orders and materials for filters
    this.mockDb.workOrders$.pipe(takeUntil(this.destroy$)).subscribe(workOrders => {
      this.workOrders = workOrders;
    });
    this.mockDb.materials$.pipe(takeUntil(this.destroy$)).subscribe(materials => {
      this.materials = materials;
    });

    // Compute materials in work orders after loading workOrders/materials
    this.computeMaterialsInWorkOrders();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // Navigation methods
  navigateToInventoryList(): void {
    this.router.navigate(['/materials/catalog']);
  }

  navigateToWarehouses(): void {
    this.router.navigate(['/materials/warehouses']);
  }

  navigateToPurchaseOrders(): void {
    this.router.navigate(['/materials/purchase-orders']);
  }

  navigateToStockMovements(): void {
    this.router.navigate(['/materials/stock-movements']);
  }

  // Action methods
  createStockAdjustment(): void {
    // Get available materials for the dialog
    this.materialService.materials$.pipe(
      take(1)
    ).subscribe(materials => {
      const dialogRef = this.dialog.open(StockAdjustmentDialogComponent, {
        width: '800px',
        maxWidth: '95vw',
        data: {
          availableMaterials: materials
        }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.materialManagementService.processStockAdjustment(result).subscribe({
            next: (success) => {
              if (success) {
                this.snackBar.open('Stock adjustment processed successfully', 'Close', {
                  duration: 3000,
                  panelClass: ['success-snackbar']
                });
                this.refreshDashboard();
              }
            },
            error: (error) => {
              this.snackBar.open(`Failed to process stock adjustment: ${error.message}`, 'Close', {
                duration: 5000,
                panelClass: ['error-snackbar']
              });
            }
          });
        }
      });
    });
  }

  createMaterialRequisition(): void {
    // Get available materials for the dialog
    this.materialService.materials$.pipe(
      take(1)
    ).subscribe(materials => {
      const dialogRef = this.dialog.open(MaterialRequisitionDialogComponent, {
        width: '1000px',
        maxWidth: '95vw',
        height: '90vh',
        data: {
          availableMaterials: materials,
          requestType: 'general'
        }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.materialManagementService.processRequisition(result).subscribe({
            next: (requisition) => {
              const approvalMessage = requisition.approvalRequired ? 
                ' (pending approval)' : ' and approved automatically';
              
              this.snackBar.open(
                `Material requisition ${requisition.requestNumber} created successfully${approvalMessage}`, 
                'Close', 
                {
                  duration: 4000,
                  panelClass: ['success-snackbar']
                }
              );
              this.refreshDashboard();
            },
            error: (error) => {
              this.snackBar.open(`Failed to create requisition: ${error.message}`, 'Close', {
                duration: 5000,
                panelClass: ['error-snackbar']
              });
            }
          });
        }
      });
    });
  }

  viewMovementDetails(movement: MaterialMovement): void {
    this.router.navigate(['/materials/stock-movements'], {
      queryParams: { movementId: movement.id }
    });
  }

  handleAlert(alert: StockAlert): void {
    switch (alert.type) {
      case 'low-stock':
        // Navigate to purchase orders
        this.router.navigate(['/materials/purchase-orders'], {
          queryParams: { materialId: alert.materialId }
        });
        break;
      case 'expiring':
        // Navigate to material catalog
        this.router.navigate(['/materials/catalog'], {
          queryParams: { materialId: alert.materialId }
        });
        break;
      case 'overstock':
        // Navigate to material catalog
        this.router.navigate(['/materials/catalog'], {
          queryParams: { materialId: alert.materialId }
        });
        break;
      case 'no-movement':
        // Navigate to stock movements
        this.router.navigate(['/materials/stock-movements'], {
          queryParams: { materialId: alert.materialId }
        });
        break;
      default:
        console.log('Handle alert:', alert);
    }
  }

  dismissAlert(alert: StockAlert): void {
    // TODO: Implement alert dismissal
    console.log('Dismiss alert:', alert);
    this.snackBar.open('Alert dismissed', 'Close', {
      duration: 2000
    });
  }

  // Compute and filter materials in work orders
  defaultComputeMaterialsInWorkOrders() {
    // Combine workOrders and materials
    const workOrders = this.workOrders || [];
    const materials = this.materials || [];
    let allRows: any[] = [];
    for (const wo of workOrders) {
      if (!wo.materials) continue;
      for (const assignment of wo.materials) {
        let material: BaseMaterial | undefined;
        if (assignment.purchasableMaterial) {
          material = materials.find(m => m.description === assignment.purchasableMaterial.description);
        } else if (assignment.receivableMaterial) {
          material = materials.find(m => m.description === assignment.receivableMaterial.description);
        }
        if (!material) continue;
        const type = assignment.materialType;
        const status = this.getMaterialStatus(assignment, type);
        const usage = this.getMaterialUsage(assignment, type);
        const isPartial = usage && usage.toLowerCase().includes('partial');
        allRows.push({
          materialName: material.description,
          workOrderNumber: wo.details?.workOrderNumber || wo.id,
          type,
          status,
          usage,
          isPartial
        });
      }
    }
    // Apply filters
    let filtered = allRows;
    if (this.selectedWorkOrder) filtered = filtered.filter(row => row.workOrderNumber === this.workOrders.find(wo => wo.id === this.selectedWorkOrder)?.details?.workOrderNumber);
    if (this.selectedMaterial) filtered = filtered.filter(row => row.materialName === this.materials.find(m => m.id === this.selectedMaterial)?.description);
    if (this.selectedType) {
      const type = this.selectedType;
      if (type) filtered = filtered.filter(row => (row.type || '').toLowerCase().trim() === type?.toLowerCase().trim());
    }
    if (this.selectedStatus) {
      const status = this.selectedStatus;
      if (status) filtered = filtered.filter(row => (row.status || '').toLowerCase().trim() === status?.toLowerCase().trim());
    }
    this.filteredMaterialsInWorkOrders = filtered;
    // Group by work order if enabled
    if (this.groupByWorkOrder) {
      const grouped: any = {};
      for (const row of filtered) {
        if (!grouped[row.workOrderNumber]) grouped[row.workOrderNumber] = { workOrderNumber: row.workOrderNumber, materials: [] };
        grouped[row.workOrderNumber].materials.push(row);
      }
      this.groupedMaterialsInWorkOrders = Object.values(grouped);
    }
  }

  // Call this after filters change
  computeMaterialsInWorkOrders() {
    this.defaultComputeMaterialsInWorkOrders();
  }

  // Smart status logic for purchasable/receivable
  getMaterialStatus(assignment: any, type: string): string {
    if (type === 'purchasable') {
      const pm = assignment.purchasableMaterial;
      if (!pm) return 'Pending';
      if (pm.status === 'pending') return 'Pending';
      if (pm.status === 'ordered') return 'Ordered';
      if (pm.status === 'delivered') {
        if (pm.delivery?.storageLocation === 'warehouse') return 'Delivered to Warehouse';
        if (pm.delivery?.storageLocation === 'site-direct') return 'Delivered to Site';
        return 'Delivered';
      }
      if (pm.status === 'in-use') return 'In Use';
      if (pm.status === 'used') return 'Installed';
      // Add more logic as needed
      return pm.status.charAt(0).toUpperCase() + pm.status.slice(1);
    } else if (type === 'receivable') {
      const rm = assignment.receivableMaterial;
      if (!rm) return 'Not Received Yet';
      if (rm.status === 'pending') return 'Not Received Yet';
      if (rm.status === 'received') return 'Received';
      if (rm.status === 'used') return 'Completed';
      // Add more logic as needed
      return rm.status.charAt(0).toUpperCase() + rm.status.slice(1);
    }
    return 'Unknown';
  }

  getMaterialUsage(assignment: any, type: string): string {
    if (type === 'purchasable') {
      const pm = assignment.purchasableMaterial;
      if (!pm) return '';
      if (pm.siteUsageRecords && pm.siteUsageRecords.length > 0) {
        const total = pm.quantity;
        const used = pm.siteUsageRecords.reduce((sum: number, rec: any) => sum + (rec.quantityUsed || 0), 0);
        if (used === 0) return 'Not Used';
        if (used < total) return `Partially Used: ${used}/${total}`;
        if (used === total) return `Fully Used: ${used}/${total}`;
      }
      if (pm.siteUsage?.actualQuantityUsed !== undefined) {
        const used = pm.siteUsage.actualQuantityUsed;
        const total = pm.quantity;
        if (used === 0) return 'Not Used';
        if (used < total) return `Partially Used: ${used}/${total}`;
        if (used === total) return `Fully Used: ${used}/${total}`;
      }
      return '';
    } else if (type === 'receivable') {
      const rm = assignment.receivableMaterial;
      if (!rm) return '';
      if (rm.usageRecords && rm.usageRecords.length > 0) {
        const total = rm.estimatedQuantity;
        const used = rm.usageRecords.reduce((sum: number, rec: any) => sum + (rec.quantityUsed || 0), 0);
        if (used === 0) return 'Not Used';
        if (used < total) return `Partially Used: ${used}/${total}`;
        if (used === total) return `Fully Used: ${used}/${total}`;
      }
      if (rm.actualQuantity !== undefined) {
        const used = rm.actualQuantity;
        const total = rm.estimatedQuantity;
        if (used === 0) return 'Not Used';
        if (used < total) return `Partially Used: ${used}/${total}`;
        if (used === total) return `Fully Used: ${used}/${total}`;
      }
      return '';
    }
    return '';
  }

  resetMaterialsTableFilters() {
    this.selectedWorkOrder = null;
    this.selectedMaterial = null;
    this.selectedType = null;
    this.selectedStatus = null;
    this.computeMaterialsInWorkOrders();
  }

  // Call computeMaterialsInWorkOrders on filter changes
  onWorkOrderFilterChange(workOrderId: string | null) {
    this.selectedWorkOrder = workOrderId;
    this.computeMaterialsInWorkOrders();
  }
  onMaterialFilterChange(materialId: string | null) {
    this.selectedMaterial = materialId;
    this.computeMaterialsInWorkOrders();
  }

  // Timeline filter handlers
  onTimelineWorkOrderFilterChange(workOrderId: string | null) {
    this.selectedWorkOrder = workOrderId;
    this.viewModel.setSelectedWorkOrder(workOrderId);
  }
  onTimelineMaterialFilterChange(materialId: string | null) {
    this.selectedMaterial = materialId;
    this.viewModel.setSelectedMaterial(materialId);
  }

  // Helper methods
  getMovementTypeIcon(type: MaterialMovement['movementType']): string {
    const icons: Record<MaterialMovement['movementType'], string> = {
      'receipt': 'add_circle',
      'issue': 'remove_circle',
      'transfer': 'swap_horiz',
      'return': 'undo',
      'write-off': 'cancel'
    };
    return icons[type] || 'help';
  }

  getMovementTypeColor(type: MaterialMovement['movementType']): string {
    const colors: Record<MaterialMovement['movementType'], string> = {
      'receipt': 'primary',
      'issue': 'accent',
      'transfer': 'primary',
      'return': 'warn',
      'write-off': 'warn'
    };
    return colors[type] || 'basic';
  }

  getAlertIcon(type: StockAlert['type']): string {
    const icons: Record<StockAlert['type'], string> = {
      'low-stock': 'inventory_2',
      'expiring': 'schedule',
      'overstock': 'inventory',
      'no-movement': 'trending_flat'
    };
    return icons[type] || 'warning';
  }

  getAlertSeverityColor(severity: StockAlert['severity']): string {
    const colors: Record<StockAlert['severity'], string> = {
      'low': 'primary',
      'medium': 'accent',
      'high': 'warn',
      'critical': 'warn'
    };
    return colors[severity] || 'basic';
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-SA', {
      style: 'currency',
      currency: 'SAR'
    }).format(value);
  }

  formatDate(date: Date | string): string {
    if (!date) return '-';
    const dateObj = new Date(date);
    return dateObj.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  // Material History Timeline methods
  getEventColor(color: string): string {
    switch (color) {
      case 'primary': return '#3f51b5';
      case 'accent': return '#ff4081';
      case 'warn': return '#ff9800';
      case 'success': return '#4caf50';
      default: return '#9e9e9e';
    }
  }

  getEventIcon(icon: string): string {
    return icon;
  }

  getEventDetails(details: { [key: string]: string }): { key: string; value: string }[] {
    return Object.entries(details).map(([key, value]) => ({ key, value }));
  }

  // Refresh data
  refreshDashboard(): void {
    this.viewModel.refresh();
  }
}
