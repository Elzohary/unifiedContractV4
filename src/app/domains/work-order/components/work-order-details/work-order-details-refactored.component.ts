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
import { WorkOrder, WorkOrderStatus, Task, workOrderDetail, Permit } from '../../models/work-order.model';
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
    this.loadWorkOrderDetails(id);
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
      // Merge updated statuses into the original permits array
      const updatedPermits = ((workOrder.permits || []) as Permit[]).map((permit: Permit) => {
        const updated = updatedStatuses.find(u => u.type === permit.type);
        return updated ? { ...permit, status: updated.status as Permit['status'] } : permit;
      });
      const updatedWorkOrder = { ...workOrder, permits: updatedPermits };
      this.workOrderDetailsViewModel.updateWorkOrder(updatedWorkOrder);
    }
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