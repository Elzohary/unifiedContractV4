import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Observable } from 'rxjs';

// Angular Material
import { MatTabsModule } from '@angular/material/tabs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTableModule } from '@angular/material/table';

// ViewModels
import { WorkOrderDetailsViewModel } from '../../viewModels/work-order-details.viewmodel';
import { WorkOrderRemarksViewModel } from '../../viewModels/work-order-remarks.viewmodel';
import { WorkOrderMaterialsViewModel } from '../../viewModels/work-order-materials.viewmodel';

// Models
import { WorkOrder, WorkOrderStatus, Task, Permit, SiteReport } from '../../models/work-order.model';
import { ActivityLog } from '../../../../shared/services/activity-log.service';

// Sub-components
import { WoHeaderComponent } from './components/wo-header/wo-header.component';
import { WoOverviewTabComponent } from './components/wo-overview-tab/wo-overview-tab.component';
import { WoTasksTabComponent } from './components/wo-tasks-tab/wo-tasks-tab.component';
import { WoIssuesTabComponent } from './components/wo-issues-tab/wo-issues-tab.component';
import { WoMaterialsTabComponent } from './components/wo-materials-tab/wo-materials-tab.component';
import { WoDocumentsTabComponent } from './components/wo-documents-tab/wo-documents-tab.component';
import { WoSiteReportTabComponent } from './components/wo-site-report-tab/wo-site-report-tab.component';

@Component({
  selector: 'app-work-order-details-refactored',
  templateUrl: './work-order-details-refactored.component.html',
  styleUrls: ['./work-order-details-refactored.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    // Material
    MatTabsModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatButtonModule,
    MatSnackBarModule,
    MatCardModule,
    MatProgressBarModule,
    MatTableModule,
    // Sub-components
    WoHeaderComponent,
    WoOverviewTabComponent,
    WoTasksTabComponent,
    WoIssuesTabComponent,
    WoMaterialsTabComponent,
    WoDocumentsTabComponent,
    WoSiteReportTabComponent
  ]
})
export class WorkOrderDetailsRefactoredComponent implements OnInit, OnDestroy {
  // Observables from ViewModels
  workOrder$: Observable<WorkOrder | null>;
  loading$: Observable<boolean>;
  error$: Observable<string | null>;
  activeTab$: Observable<string>;
  activityLogs$: Observable<ActivityLog[]>;
  tasks$: Observable<Task[]>;

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

  // Component state
  selectedTabIndex = 0;
  currentWorkOrder: WorkOrder | null = null;

  // Tab mapping
  private tabIndexMap: Record<string, number> = {
    'overview': 0,
    'tasks': 1,
    'issues': 2,
    'materials': 3,
    'documents': 4,
    'expenses': 5
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private workOrderDetailsViewModel: WorkOrderDetailsViewModel,
    private workOrderRemarksViewModel: WorkOrderRemarksViewModel,
    private workOrderMaterialsViewModel: WorkOrderMaterialsViewModel,
    private snackBar: MatSnackBar
  ) {
    // Initialize observables from ViewModels
    this.workOrder$ = this.workOrderDetailsViewModel.workOrder$;
    this.loading$ = this.workOrderDetailsViewModel.loading$;
    this.error$ = this.workOrderDetailsViewModel.error$;
    this.activeTab$ = this.workOrderDetailsViewModel.activeTab$;
    this.activityLogs$ = this.workOrderDetailsViewModel.activityLogs$;
    this.tasks$ = this.workOrderDetailsViewModel.tasks$;
  }

  ngOnInit(): void {
    // Load work order based on route parameter
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.loadWorkOrderDetails(id);
      } else {
        this.handleError('Work order ID is missing');
      }
    });

    // Handle tab navigation from route fragment
    this.route.fragment.subscribe(fragment => {
      if (fragment && this.tabIndexMap[fragment] !== undefined) {
        this.selectedTabIndex = this.tabIndexMap[fragment];
        this.workOrderDetailsViewModel.setActiveTab(fragment);
      }
    });

    // Subscribe to active tab changes from ViewModel
    this.activeTab$.subscribe(tab => {
      if (this.tabIndexMap[tab] !== undefined) {
        this.selectedTabIndex = this.tabIndexMap[tab];
      }
    });

    // Subscribe to errors
    this.error$.subscribe(error => {
      if (error) {
        this.handleError(error);
      }
    });

    this.workOrder$.subscribe(wo => {
      this.currentWorkOrder = wo;
    });
  }

  ngOnDestroy(): void {
    this.workOrderDetailsViewModel.destroy();
    this.workOrderRemarksViewModel.destroy();
    this.workOrderMaterialsViewModel.destroy();
  }

  /**
   * Load work order details and related data
   */
  private loadWorkOrderDetails(id: string): void {
    this.workOrderDetailsViewModel.loadWorkOrderDetails(id);
    this.workOrderRemarksViewModel.loadRemarksForWorkOrder(id);
  }

  /**
   * Reload the work order details by id (used for itemsChanged event)
   */
  reloadWorkOrder(id: string): void {
    this.workOrderDetailsViewModel.loadWorkOrderDetails(id);
    this.workOrder$.subscribe(wo => {
      console.log('[DEBUG] Reloaded work order:', wo);
      if (wo) {
        console.log('[DEBUG] Reloaded siteReports:', wo.siteReports);
      }
    }).unsubscribe();
  }

  /**
   * Navigate back to work orders list
   */
  goBack(): void {
    this.router.navigate(['/work-orders']);
  }

  /**
   * Navigate to edit work order
   */
  editWorkOrder(workOrder: WorkOrder): void {
    this.router.navigate(['/work-orders/edit', workOrder.id]);
  }

  /**
   * Update work order status
   */
  updateStatus(workOrderId: string, newStatus: WorkOrderStatus): void {
    this.workOrderDetailsViewModel.updateWorkOrderStatus(workOrderId, newStatus)
      .subscribe(success => {
        if (success) {
          this.snackBar.open('Work order status updated successfully', 'Close', { 
            duration: 3000 
          });
        } else {
          this.snackBar.open('Failed to update work order status', 'Close', { 
            duration: 3000 
          });
        }
      });
  }

  /**
   * Handle tab change
   */
  onTabChange(index: number): void {
    const tabName = Object.keys(this.tabIndexMap).find(key => this.tabIndexMap[key] === index);
    if (tabName) {
      this.workOrderDetailsViewModel.setActiveTab(tabName);
      this.router.navigate([], {
        relativeTo: this.route,
        fragment: tabName
      });
    }
  }

  /**
   * Handle print action
   */
  onPrint(): void {
    // TODO: Implement print functionality
    this.snackBar.open('Print functionality coming soon', 'Close', { duration: 3000 });
  }

  /**
   * Handle export action
   */
  onExport(): void {
    // TODO: Implement export functionality
    this.snackBar.open('Export functionality coming soon', 'Close', { duration: 3000 });
  }

  /**
   * Handle duplicate action
   */
  onDuplicate(): void {
    // TODO: Implement duplicate functionality
    this.snackBar.open('Duplicate functionality coming soon', 'Close', { duration: 3000 });
  }

  /**
   * Handle task events from tasks tab
   */
  onTaskAdded(task: Partial<Task>, workOrderId: string): void {
    this.workOrderDetailsViewModel.addTask(workOrderId, task)
      .subscribe(newTask => {
        if (newTask) {
          this.snackBar.open('Task added successfully', 'Close', { duration: 3000 });
        }
      });
  }

  onTaskUpdated(task: Task): void {
    this.workOrderDetailsViewModel.updateTask(task)
      .subscribe(success => {
        if (success) {
          this.snackBar.open('Task updated successfully', 'Close', { duration: 3000 });
        }
      });
  }

  onTaskDeleted(taskId: string): void {
    this.workOrderDetailsViewModel.deleteTask(taskId)
      .subscribe(success => {
        if (success) {
          this.snackBar.open('Task deleted successfully', 'Close', { duration: 3000 });
        }
      });
  }

  onPermitsChanged(updatedStatuses: {type: string, status: string}[]) {
    let currentWorkOrder: WorkOrder | null = null;
    this.workOrder$.subscribe(wo => currentWorkOrder = wo).unsubscribe();
    if (currentWorkOrder && typeof currentWorkOrder === 'object') {
      const workOrder = currentWorkOrder as WorkOrder;
      const existingPermits = Array.isArray(workOrder.permits) ? [...workOrder.permits] : [];
      // Ensure all permit types from the dialog are present
      const allPermitTypes = ['Initial', 'Municipality', 'RoadDepartment', 'Traffic'];
      const updatedPermits = allPermitTypes.map(type => {
        const updated = updatedStatuses.find(u => u.type === type);
        const existing = existingPermits.find(p => p.type === type);
        if (updated) {
          return existing ? { ...existing, status: updated.status as Permit['status'] } : {
            id: Date.now().toString() + type,
            type,
            status: (updated.status === 'approved' ? 'approved' : updated.status === 'pending' ? 'pending' : 'pending') as Permit['status'],
            title: '',
            description: '',
            number: '',
            authority: '',
            issueDate: new Date(),
            expiryDate: new Date(),
            issuedBy: '',
            documentRef: '',
            attachments: []
          };
        } else {
          // If not updated, keep existing or add as pending
          return existing ? existing : {
            id: Date.now().toString() + type,
            type,
            status: 'pending' as Permit['status'],
            title: '',
            description: '',
            number: '',
            authority: '',
            issueDate: new Date(),
            expiryDate: new Date(),
            issuedBy: '',
            documentRef: '',
            attachments: []
          };
        }
      });
      const updatedWorkOrder = { ...workOrder, permits: updatedPermits };
      this.workOrderDetailsViewModel.updateWorkOrder(updatedWorkOrder);
    }
  }

  onSiteReportAdded(formValue: any) {
    console.log('[DEBUG] onSiteReportAdded called with formValue:', formValue);
    if (this.currentWorkOrder && typeof this.currentWorkOrder === 'object') {
      // Construct a valid SiteReport object
      const newReport: SiteReport = {
        id: typeof crypto !== 'undefined' && crypto.randomUUID ? crypto.randomUUID() : Date.now().toString(),
        workOrderId: this.currentWorkOrder.id,
        foremanId: 'currentUserId', // TODO: Replace with actual user ID from auth
        foremanName: formValue.foremanName,
        workDone: formValue.workDone === 'other' ? formValue.workDoneOther : formValue.workDone, // Always use item id
        actualQuantity: typeof formValue.actualQuantity === 'number' ? formValue.actualQuantity : undefined,
        date: formValue.date,
        materialsUsed: (formValue.materialsUsed || []).map((m: any) => {
          // Lookup material name from workOrder.materials
          let materialName = 'Unknown';
          const mat = (this.currentWorkOrder!.materials || []).find((mat: any) => mat.id === m.materialId);
          if (mat) {
            materialName = mat.purchasableMaterial?.name || mat.receivableMaterial?.name || 'Unknown';
          }
          return {
            materialId: m.materialId,
            materialName,
            quantity: m.quantity
          };
        }),
        photos: (formValue.photos || []).map((p: any) => ({
          id: typeof crypto !== 'undefined' && crypto.randomUUID ? crypto.randomUUID() : Date.now().toString(),
          url: p.url,
          caption: p.caption
        })),
        notes: formValue.notes,
        createdAt: new Date()
      };

      // Update actualQuantity for the relevant item (add to existing value)
      if (this.currentWorkOrder.items && formValue.workDone && formValue.actualQuantity != null && formValue.workDone !== 'other') {
        const items = this.currentWorkOrder.items.map(item => {
          if (item.id === formValue.workDone) {
            const newActualQty = (item.actualQuantity || 0) + formValue.actualQuantity;
            return { ...item, actualQuantity: newActualQty };
          }
          return { ...item };
        });
        this.currentWorkOrder.items = items;
      }

      // Update materials used quantities and status
      let updatedMaterials = Array.isArray(this.currentWorkOrder.materials) ? [...this.currentWorkOrder.materials] : [];
      for (const used of newReport.materialsUsed) {
        const matIndex = updatedMaterials.findIndex(mat => mat.id === used.materialId);
        if (matIndex !== -1) {
          const mat = updatedMaterials[matIndex];
          if (mat.materialType === 'receivable' && mat.receivableMaterial) {
            // Increment actualQuantity and update status if needed
            const prevQty = mat.receivableMaterial.actualQuantity || 0;
            const newQty = prevQty + used.quantity;
            const estimatedQty = mat.receivableMaterial.estimatedQuantity || 1;
            updatedMaterials[matIndex] = {
              ...mat,
              receivableMaterial: {
                ...mat.receivableMaterial,
                actualQuantity: newQty,
                status: newQty >= estimatedQty ? 'used' : 'received',
                usageRecords: [
                  ...(mat.receivableMaterial.usageRecords || []),
                  {
                    id: `usage_${Date.now()}`,
                    recordType: 'usage-update',
                    recordDate: new Date(),
                    recordedBy: 'Current User',
                    recordedByName: formValue.foremanName,
                    quantityUsed: used.quantity,
                    cumulativeQuantityUsed: newQty,
                    usagePercentage: Math.round((newQty / estimatedQty) * 100),
                    remainingQuantity: Math.max(0, estimatedQty - newQty),
                    usageNotes: formValue.notes || ''
                  }
                ]
              }
            };
          } else if (mat.materialType === 'purchasable' && mat.purchasableMaterial) {
            // Add a siteUsageRecord and update legacy field
            const prevQty = mat.purchasableMaterial.siteUsage?.actualQuantityUsed || 0;
            const estimatedQty = mat.purchasableMaterial.quantity || 1;
            const newQty = prevQty + used.quantity;
            updatedMaterials[matIndex] = {
              ...mat,
              purchasableMaterial: {
                ...mat.purchasableMaterial,
                siteUsageRecords: [
                  ...(mat.purchasableMaterial.siteUsageRecords || []),
                  {
                    id: `usage_${Date.now()}`,
                    recordType: 'usage-update',
                    recordDate: new Date(),
                    recordedBy: 'Current User',
                    recordedByName: formValue.foremanName,
                    quantityUsed: used.quantity,
                    cumulativeQuantityUsed: newQty,
                    usagePercentage: Math.round((newQty / estimatedQty) * 100),
                    remainingQuantity: Math.max(0, estimatedQty - newQty),
                    usageNotes: formValue.notes || ''
                  }
                ],
                siteUsage: {
                  issuedToSite: mat.purchasableMaterial.siteUsage?.issuedToSite ?? false,
                  ...(mat.purchasableMaterial.siteUsage || {}),
                  actualQuantityUsed: newQty,
                  usagePercentage: Math.round((newQty / estimatedQty) * 100),
                  usageCompletedDate: new Date(),
                  usageNotes: formValue.notes || ''
                },
                status: newQty >= estimatedQty ? 'used' : 'in-use'
              }
            };
          }
        }
      }

      const siteReports: SiteReport[] = Array.isArray(this.currentWorkOrder.siteReports)
        ? [...this.currentWorkOrder.siteReports, newReport]
        : [newReport];

      const updatedWorkOrder = {
        ...this.currentWorkOrder,
        siteReports,
        items: this.currentWorkOrder.items, // Keep the new items array
        materials: updatedMaterials
      };
      console.log('[DEBUG] Submitting updatedWorkOrder with siteReports:', updatedWorkOrder.siteReports);
      this.workOrderDetailsViewModel.updateWorkOrder(updatedWorkOrder).subscribe({
        next: (wo) => {
          if (wo && wo.id) {
            this.workOrderDetailsViewModel.loadWorkOrderDetails(wo.id);
          }
        },
        error: () => {
          this.handleError('Failed to update work order after adding site report.');
        }
      });
    }
  }

  onSiteReportDeleted(deletedReport: SiteReport) {
    if (!this.currentWorkOrder) return;
    // Remove the report from siteReports
    const siteReports = Array.isArray(this.currentWorkOrder.siteReports)
      ? this.currentWorkOrder.siteReports.filter(r => r.id !== deletedReport.id)
      : [];
    this.currentWorkOrder.siteReports = siteReports;
    // Recalculate actualQuantity for all items based on all siteReports
    if (Array.isArray(this.currentWorkOrder.items)) {
      const itemActualQtyMap: { [itemId: string]: number } = {};
      siteReports.forEach(report => {
        if (report.workDone && typeof report.actualQuantity === 'number' && report.workDone !== 'other') {
          itemActualQtyMap[report.workDone] = (itemActualQtyMap[report.workDone] || 0) + report.actualQuantity;
        }
      });
      const items = this.currentWorkOrder.items.map(item => {
        return { ...item, actualQuantity: itemActualQtyMap[item.id] || 0 };
      });
      this.currentWorkOrder.items = items;
    }
    const updatedWorkOrder = {
      ...this.currentWorkOrder,
      siteReports,
      items: this.currentWorkOrder.items
    };
    this.workOrderDetailsViewModel.updateWorkOrder(updatedWorkOrder).subscribe({
      next: (wo) => {
        if (wo && wo.id) {
          this.workOrderDetailsViewModel.loadWorkOrderDetails(wo.id);
        }
      },
      error: () => {
        this.handleError('Failed to update work order after deleting site report.');
      }
    });
  }

  /**
   * Handle error messages
   */
  private handleError(message: string): void {
    this.snackBar.open(message, 'Close', { 
      duration: 3000,
      panelClass: ['error-snackbar']
    });
  }
} 