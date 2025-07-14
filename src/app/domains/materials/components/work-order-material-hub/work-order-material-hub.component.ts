import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, Subject, BehaviorSubject, combineLatest, of } from 'rxjs';
import { takeUntil, map, startWith } from 'rxjs/operators';

// Angular Material
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatBadgeModule } from '@angular/material/badge';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';

// Services and Models
import { MaterialManagementService } from '../../services/material-management.service';
import { MaterialService } from '../../services/material.service';
import { MaterialRequisitionDialogComponent } from '../dialogs/material-requisition-dialog/material-requisition-dialog.component';
import { BreadcrumbComponent } from '../../../../shared/components/breadcrumb/breadcrumb.component';
import { WorkOrderService } from '../../../work-order/services/work-order.service';
import { WorkOrder, materialAssignment } from '../../../work-order/models/work-order.model';
import { OptimizationDialogComponent } from '../dialogs/optimization-dialog/optimization-dialog.component';
import { MaterialUsageDialogComponent } from '../dialogs/material-usage-dialog/material-usage-dialog.component';
import { WorkOrderMaterialsDialogComponent } from '../dialogs/work-order-materials-dialog/work-order-materials-dialog.component';
import { MaterialAllocationDetailsDialogComponent } from '../dialogs/material-allocation-details-dialog/material-allocation-details-dialog.component';
import { MaterialReallocationDialogComponent } from '../dialogs/material-reallocation-dialog/material-reallocation-dialog.component';
import { NgCardComponent } from '../../../../shared/components/ng-card/ng-card.component';
import { NgTableComponent } from '../../../../shared/components/ng-table/ng-table.component';
import { UnderConstructionComponent } from '../../../../shared/components/under-construction/under-construction.component';

export interface WorkOrderMaterialSummary {
  workOrderId: string;
  workOrderNumber: string;
  workOrderTitle: string;
  status: string;
  totalMaterials: number;
  assignedMaterials: number;
  deliveredMaterials: number;
  usedMaterials: number;
  totalEstimatedCost: number;
  totalActualCost: number;
  completionPercentage: number;
  urgency: 'low' | 'medium' | 'high' | 'critical';
  lastUpdated: Date;
  hasRecentMaterialUpdates?: boolean;
  pendingMaterialActions?: number;
}

export interface MaterialAllocationSummary {
  materialId: string;
  materialCode: string;
  materialDescription: string;
  totalAllocated: number;
  totalUsed: number;
  totalRemaining: number;
  allocatedToWorkOrders: number;
  pendingDeliveries: number;
  unit: string;
  estimatedValue: number;
  actualValue: number;
}

@Component({
  selector: 'app-work-order-material-hub',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatTabsModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatBadgeModule,
    MatTooltipModule,
    MatMenuModule,
    MatDividerModule,
    MatDialogModule,
    MatSnackBarModule,
    BreadcrumbComponent,
    NgCardComponent,
    NgTableComponent,
    UnderConstructionComponent
  ],
  template: `
    <div class="work-order-material-hub">
      <!-- Breadcrumbs -->
      <app-breadcrumb></app-breadcrumb>
      
      <!-- Header Section -->
      <mat-card class="header-card">
        <mat-card-header>
          <mat-card-title>
            <mat-icon>engineering</mat-icon>
            Work Order Material Management Hub
          </mat-card-title>
          <mat-card-subtitle>
            Central hub for managing materials across all work orders
          </mat-card-subtitle>
          <div class="header-actions">
            <button mat-raised-button color="primary" 
                    (click)="createMaterialRequisition()"
                    matTooltip="Create a new material requisition">
              <mat-icon>assignment</mat-icon>
              New Requisition
            </button>
            <button mat-stroked-button 
                    (click)="refreshData()"
                    matTooltip="Refresh all data">
              <mat-icon>refresh</mat-icon>
              Refresh
            </button>
          </div>
        </mat-card-header>
      </mat-card>

      <!-- Summary Cards -->
      <div class="summary-cards">
        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-content">
              <mat-icon class="summary-icon primary">assignment</mat-icon>
              <div class="summary-details">
                <div class="summary-value">{{totalWorkOrders}}</div>
                <div class="summary-label">Active Work Orders</div>
              </div>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="summary-card">
          <mat-card-content>
            <div class="summary-content">
              <mat-icon class="summary-icon success">attach_money</mat-icon>
              <div class="summary-details">
                <div class="summary-value">{{totalValue | currency:'SAR'}}</div>
                <div class="summary-label">Total Value</div>
              </div>
            </div>
          </mat-card-content>
        </mat-card>
      </div>

      <!-- Main Content Tabs -->
      <mat-card class="content-card">
        <mat-tab-group [(selectedIndex)]="selectedTab" class="main-tabs">
          <!-- Work Orders Tab -->
          <mat-tab label="Work Orders">
            <div class="tab-content">
              <!-- Filters -->
              <div class="filters-section">
                <mat-form-field appearance="outline">
                  <mat-label>Search</mat-label>
                  <input matInput 
                         [(ngModel)]="workOrderSearchTerm"
                         (ngModelChange)="onWorkOrderSearchChange()"
                         placeholder="Search by work order number or title...">
                  <mat-icon matSuffix>search</mat-icon>
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Status</mat-label>
                  <mat-select [(ngModel)]="selectedStatus" (ngModelChange)="onStatusFilterChange()">
                    <mat-option value="">All Statuses</mat-option>
                    <mat-option value="active">Active</mat-option>
                    <mat-option value="pending">Pending</mat-option>
                    <mat-option value="completed">Completed</mat-option>
                  </mat-select>
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Urgency</mat-label>
                  <mat-select [(ngModel)]="selectedUrgency" (ngModelChange)="onUrgencyFilterChange()">
                    <mat-option value="">All Urgencies</mat-option>
                    <mat-option value="critical">Critical</mat-option>
                    <mat-option value="high">High</mat-option>
                    <mat-option value="medium">Medium</mat-option>
                    <mat-option value="low">Low</mat-option>
                  </mat-select>
                </mat-form-field>
              </div>

              <!-- Work Orders Table -->
              <div class="table-container">
                <table mat-table [dataSource]="(filteredWorkOrders$ | async) || []" class="work-orders-table">
                  <ng-container matColumnDef="workOrder">
                    <th mat-header-cell *matHeaderCellDef>Work Order</th>
                    <td mat-cell *matCellDef="let wo">
                      <div class="work-order-cell">
                        <span class="wo-number">{{wo.workOrderNumber}}</span>
                        <span class="wo-title">{{wo.workOrderTitle}}</span>
                      </div>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="status">
                    <th mat-header-cell *matHeaderCellDef>Status</th>
                    <td mat-cell *matCellDef="let wo">
                      <mat-chip [class]="'status-' + wo.status">{{wo.status | titlecase}}</mat-chip>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="materials">
                    <th mat-header-cell *matHeaderCellDef>Materials</th>
                    <td mat-cell *matCellDef="let wo">
                      <div class="materials-cell">
                        <div class="materials-header">
                          <span class="materials-count">{{wo.assignedMaterials}}/{{wo.totalMaterials}}</span>
                          <mat-icon *ngIf="wo.hasRecentMaterialUpdates" 
                                   class="update-indicator" 
                                   matTooltip="Recent material status updates"
                                   color="accent">
                            update
                          </mat-icon>
                        </div>
                        <mat-progress-bar 
                          mode="determinate" 
                          [value]="(wo.assignedMaterials / wo.totalMaterials) * 100"
                          class="materials-progress">
                        </mat-progress-bar>
                        <div *ngIf="wo.pendingMaterialActions && wo.pendingMaterialActions > 0" 
                             class="pending-actions">
                          <mat-chip color="warn" class="pending-chip">
                            {{wo.pendingMaterialActions}} pending
                          </mat-chip>
                        </div>
                      </div>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="completion">
                    <th mat-header-cell *matHeaderCellDef>Completion</th>
                    <td mat-cell *matCellDef="let wo">
                      <div class="completion-cell">
                        <span class="completion-percentage">{{wo.completionPercentage}}%</span>
                        <mat-progress-bar 
                          mode="determinate" 
                          [value]="wo.completionPercentage"
                          [color]="wo.completionPercentage < 50 ? 'warn' : 'primary'">
                        </mat-progress-bar>
                      </div>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="cost">
                    <th mat-header-cell *matHeaderCellDef>Cost</th>
                    <td mat-cell *matCellDef="let wo">
                      <div class="cost-cell">
                        <span class="estimated-cost">Est: {{wo.totalEstimatedCost | currency:'SAR'}}</span>
                        <span class="actual-cost">Act: {{wo.totalActualCost | currency:'SAR'}}</span>
                      </div>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="urgency">
                    <th mat-header-cell *matHeaderCellDef>Urgency</th>
                    <td mat-cell *matCellDef="let wo">
                      <mat-chip [class]="'urgency-' + wo.urgency">{{wo.urgency | titlecase}}</mat-chip>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="actions">
                    <th mat-header-cell *matHeaderCellDef></th>
                    <td mat-cell *matCellDef="let wo">
                      <button mat-icon-button 
                              [matMenuTriggerFor]="woMenu"
                              matTooltip="Work order actions">
                        <mat-icon>more_vert</mat-icon>
                      </button>
                      <mat-menu #woMenu="matMenu">
                        <button mat-menu-item 
                                (click)="viewWorkOrderMaterials(wo)"
                                matTooltip="View materials for this work order">
                          <mat-icon>visibility</mat-icon>
                          <span>View Materials</span>
                        </button>
                        <button mat-menu-item 
                                (click)="requestMaterialsForWorkOrder(wo)"
                                matTooltip="Request additional materials">
                          <mat-icon>add_shopping_cart</mat-icon>
                          <span>Request Materials</span>
                        </button>
                        <button mat-menu-item 
                                (click)="optimizeMaterials(wo)"
                                matTooltip="Optimize material allocation">
                          <mat-icon>auto_fix_high</mat-icon>
                          <span>Optimize Materials</span>
                        </button>
                      </mat-menu>
                    </td>
                  </ng-container>

                  <tr mat-header-row *matHeaderRowDef="workOrderColumns"></tr>
                  <tr mat-row *matRowDef="let row; columns: workOrderColumns;"></tr>
                </table>
              </div>
            </div>
          </mat-tab>

          <!-- Material Allocations Tab -->
          <mat-tab label="Material Allocations">
            <div class="tab-content">
              <!-- Material Search -->
              <div class="filters-section">
                <mat-form-field appearance="outline">
                  <mat-label>Search Materials</mat-label>
                  <input matInput 
                         [(ngModel)]="materialSearchTerm"
                         (ngModelChange)="onMaterialSearchChange()"
                         placeholder="Search by material code or description...">
                  <mat-icon matSuffix>search</mat-icon>
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Allocation Status</mat-label>
                  <mat-select [(ngModel)]="selectedAllocationStatus" (ngModelChange)="onAllocationStatusFilterChange()">
                    <mat-option value="">All</mat-option>
                    <mat-option value="over-allocated">Over Allocated</mat-option>
                    <mat-option value="under-allocated">Under Allocated</mat-option>
                    <mat-option value="optimal">Optimal</mat-option>
                  </mat-select>
                </mat-form-field>
              </div>

              <!-- Material Allocations Table -->
              <div class="table-container">
                <table mat-table [dataSource]="(filteredMaterialAllocations$ | async) || []" class="allocations-table">
                  <ng-container matColumnDef="material">
                    <th mat-header-cell *matHeaderCellDef>Material</th>
                    <td mat-cell *matCellDef="let allocation">
                      <div class="material-cell">
                        <span class="material-code">{{allocation.materialCode}}</span>
                        <span class="material-description">{{allocation.materialDescription}}</span>
                      </div>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="allocation">
                    <th mat-header-cell *matHeaderCellDef>Allocation</th>
                    <td mat-cell *matCellDef="let allocation">
                      <div class="allocation-cell">
                        <span class="allocation-text">
                          {{allocation.totalUsed}}/{{allocation.totalAllocated}} {{allocation.unit}}
                        </span>
                        <mat-progress-bar 
                          mode="determinate" 
                          [value]="(allocation.totalUsed / allocation.totalAllocated) * 100">
                        </mat-progress-bar>
                      </div>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="workOrders">
                    <th mat-header-cell *matHeaderCellDef>Work Orders</th>
                    <td mat-cell *matCellDef="let allocation">
                      <mat-chip class="work-orders-chip">
                        {{allocation.allocatedToWorkOrders}} WOs
                      </mat-chip>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="value">
                    <th mat-header-cell *matHeaderCellDef>Value</th>
                    <td mat-cell *matCellDef="let allocation">
                      <div class="value-cell">
                        <span class="estimated-value">Est: {{allocation.estimatedValue | currency:'SAR'}}</span>
                        <span class="actual-value">Act: {{allocation.actualValue | currency:'SAR'}}</span>
                      </div>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="status">
                    <th mat-header-cell *matHeaderCellDef>Status</th>
                    <td mat-cell *matCellDef="let allocation">
                      <mat-chip [class]="getAllocationStatusClass(allocation)">
                        {{getAllocationStatus(allocation)}}
                      </mat-chip>
                    </td>
                  </ng-container>

                  <ng-container matColumnDef="actions">
                    <th mat-header-cell *matHeaderCellDef></th>
                    <td mat-cell *matCellDef="let allocation">
                      <button mat-icon-button 
                              [matMenuTriggerFor]="materialMenu"
                              matTooltip="Material allocation actions">
                        <mat-icon>more_vert</mat-icon>
                      </button>
                      <mat-menu #materialMenu="matMenu">
                        <button mat-menu-item 
                                (click)="viewMaterialDetails(allocation)"
                                matTooltip="View material details">
                          <mat-icon>info</mat-icon>
                          <span>View Details</span>
                        </button>
                        <button mat-menu-item 
                                (click)="reallocateMaterial(allocation)"
                                matTooltip="Reallocate material to different work orders">
                          <mat-icon>swap_horiz</mat-icon>
                          <span>Reallocate</span>
                        </button>
                        <button mat-menu-item 
                                (click)="trackMaterialUsage(allocation)"
                                matTooltip="View usage history and tracking">
                          <mat-icon>timeline</mat-icon>
                          <span>Usage History</span>
                        </button>
                      </mat-menu>
                    </td>
                  </ng-container>

                  <tr mat-header-row *matHeaderRowDef="allocationColumns"></tr>
                  <tr mat-row *matRowDef="let row; columns: allocationColumns;"></tr>
                </table>
              </div>
            </div>
          </mat-tab>
        </mat-tab-group>
      </mat-card>
    </div>
  `,
  styles: [`
    .work-order-material-hub {
      padding: 16px;
      display: flex;
      flex-direction: column;
      gap: 16px;
    }

    .header-card .mat-card-header {
      display: flex;
      align-items: center;
      justify-content: space-between;
    }

    .header-actions {
      display: flex;
      gap: 8px;
      margin-left: auto;
    }

    .summary-cards {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
      gap: 16px;
    }

    .summary-card {
      min-height: 120px;
    }

    .summary-content {
      display: flex;
      align-items: center;
      gap: 16px;
    }

    .summary-icon {
      font-size: 48px;
      height: 48px;
      width: 48px;
    }

    .summary-icon.primary { color: #1976d2; }
    .summary-icon.accent { color: #ff4081; }
    .summary-icon.warn { color: #ff9800; }
    .summary-icon.success { color: #4caf50; }

    .summary-details {
      display: flex;
      flex-direction: column;
    }

    .summary-value {
      font-size: 24px;
      font-weight: 500;
      color: rgba(0, 0, 0, 0.87);
    }

    .summary-label {
      font-size: 14px;
      color: rgba(0, 0, 0, 0.6);
    }

    .content-card {
      flex: 1;
    }

    .main-tabs {
      min-height: 500px;
    }

    .tab-content {
      padding: 16px;
    }

    .filters-section {
      display: flex;
      gap: 16px;
      margin-bottom: 16px;
      flex-wrap: wrap;
    }

    .filters-section mat-form-field {
      min-width: 200px;
    }

    .table-container {
      overflow-x: auto;
    }

    .work-orders-table,
    .allocations-table {
      width: 100%;
    }

    .work-order-cell,
    .material-cell {
      display: flex;
      flex-direction: column;
      gap: 2px;
    }

    .wo-number,
    .material-code {
      font-weight: 500;
      color: #1976d2;
    }

    .wo-title,
    .material-description {
      font-size: 12px;
      color: rgba(0, 0, 0, 0.6);
    }

    .materials-cell,
    .completion-cell,
    .allocation-cell {
      display: flex;
      flex-direction: column;
      gap: 4px;
      min-width: 120px;
    }

    .materials-count,
    .completion-percentage,
    .allocation-text {
      font-size: 12px;
      font-weight: 500;
    }

    .materials-progress,
    .completion-cell mat-progress-bar,
    .allocation-cell mat-progress-bar {
      height: 6px;
    }

    .cost-cell,
    .value-cell {
      display: flex;
      flex-direction: column;
      gap: 2px;
    }

    .estimated-cost,
    .estimated-value {
      font-size: 12px;
      color: rgba(0, 0, 0, 0.6);
    }

    .actual-cost,
    .actual-value {
      font-size: 12px;
      font-weight: 500;
    }

    .status-active { background-color: #e8f5e8; color: #2e7d32; }
    .status-pending { background-color: #fff3e0; color: #ef6c00; }
    .status-completed { background-color: #e3f2fd; color: #1976d2; }

    .urgency-low { background-color: #e8f5e8; color: #2e7d32; }
    .urgency-medium { background-color: #fff3e0; color: #ef6c00; }
    .urgency-high { background-color: #ffebee; color: #c62828; }
    .urgency-critical { background-color: #fce4ec; color: #ad1457; }

    .work-orders-chip {
      background-color: #f5f5f5;
      color: rgba(0, 0, 0, 0.87);
    }

    .materials-header {
      display: flex;
      align-items: center;
      gap: 8px;
      margin-bottom: 4px;
    }

    .update-indicator {
      font-size: 16px;
      color: #ff9800;
    }

    .pending-actions {
      margin-top: 4px;
    }

    .pending-chip {
      font-size: 10px;
      height: 20px;
    }

    .materials-cell {
      min-width: 120px;
    }

    .materials-count {
      font-weight: 500;
      font-size: 14px;
    }

    .materials-progress {
      margin-top: 4px;
    }

    @media (max-width: 768px) {
      .filters-section {
        flex-direction: column;
      }

      .filters-section mat-form-field {
        min-width: unset;
        width: 100%;
      }

      .summary-cards {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class WorkOrderMaterialHubComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  
  // Data Sources
  private workOrdersSubject = new BehaviorSubject<WorkOrderMaterialSummary[]>([]);
  private materialAllocationsSubject = new BehaviorSubject<MaterialAllocationSummary[]>([]);
  
  // Filter States
  workOrderSearchTerm = '';
  selectedStatus = '';
  selectedUrgency = '';
  materialSearchTerm = '';
  selectedAllocationStatus = '';
  selectedTab = 0;

  // Summary Data
  totalWorkOrders = 0;
  totalMaterialsAllocated = 0;
  pendingDeliveries = 0;
  totalValue = 0;

  // Table Columns
  workOrderColumns = ['workOrder', 'status', 'materials', 'completion', 'cost', 'urgency', 'actions'];
  allocationColumns = ['material', 'allocation', 'workOrders', 'value', 'status', 'actions'];

  // Filtered Data
  filteredWorkOrders$: Observable<WorkOrderMaterialSummary[]> = of([]);
  filteredMaterialAllocations$: Observable<MaterialAllocationSummary[]> = of([]);

  constructor(
    private materialManagementService: MaterialManagementService,
    private materialService: MaterialService,
    private workOrderService: WorkOrderService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.initializeFilteredData();
  }

  ngOnInit(): void {
    this.loadData();
    this.setupDataSubscriptions();
    // Check for workOrderId in query params and open dialog if present
    let dialogOpened = false;
    this.route.queryParams.pipe(takeUntil(this.destroy$)).subscribe(params => {
      const workOrderId = params['workOrderId'];
      if (workOrderId && !dialogOpened) {
        // Wait for work orders to be loaded, then open dialog
        this.filteredWorkOrders$.pipe(takeUntil(this.destroy$)).subscribe(workOrders => {
          const found = workOrders.find(wo => wo.workOrderId === workOrderId);
          if (found && !dialogOpened) {
            dialogOpened = true;
            setTimeout(() => this.openWorkOrderMaterialsDialog(found), 300);
          }
        });
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private initializeFilteredData(): void {
    // Initialize filtered work orders with reactive filtering
    this.filteredWorkOrders$ = this.workOrdersSubject.asObservable().pipe(
      map(workOrders => this.filterWorkOrders(workOrders, this.workOrderSearchTerm, this.selectedStatus, this.selectedUrgency))
    );

    // Initialize filtered material allocations
    this.filteredMaterialAllocations$ = this.materialAllocationsSubject.asObservable().pipe(
      map(allocations => this.filterMaterialAllocations(allocations, this.materialSearchTerm, this.selectedAllocationStatus))
    );
  }

  // Enhanced filtering methods
  onWorkOrderSearchChange(): void {
    const workOrders = this.workOrdersSubject.value;
    const filtered = this.filterWorkOrders(workOrders, this.workOrderSearchTerm, this.selectedStatus, this.selectedUrgency);
    this.filteredWorkOrders$ = of(filtered);
  }

  onStatusFilterChange(): void {
    this.onWorkOrderSearchChange();
  }

  onUrgencyFilterChange(): void {
    this.onWorkOrderSearchChange();
  }

  onMaterialSearchChange(): void {
    const allocations = this.materialAllocationsSubject.value;
    const filtered = this.filterMaterialAllocations(allocations, this.materialSearchTerm, this.selectedAllocationStatus);
    this.filteredMaterialAllocations$ = of(filtered);
  }

  onAllocationStatusFilterChange(): void {
    this.onMaterialSearchChange();
  }

  clearWorkOrderFilters(): void {
    this.workOrderSearchTerm = '';
    this.selectedStatus = '';
    this.selectedUrgency = '';
    this.onWorkOrderSearchChange();
  }

  clearMaterialFilters(): void {
    this.materialSearchTerm = '';
    this.selectedAllocationStatus = '';
    this.onMaterialSearchChange();
  }

  private setupDataSubscriptions(): void {
    // Subscribe to work order changes for real-time updates
    this.workOrderService.workOrders$.pipe(
      takeUntil(this.destroy$)
    ).subscribe(workOrders => {
      const summaries = this.transformToMaterialSummary(workOrders);
      this.workOrdersSubject.next(summaries);
      this.totalWorkOrders = summaries.length;
    });

    // Subscribe to new work order creation
    this.workOrderService.newWorkOrder$.pipe(
      takeUntil(this.destroy$)
    ).subscribe(newWorkOrder => {
      const currentSummaries = this.workOrdersSubject.value;
      const newSummary = this.transformToMaterialSummary([newWorkOrder])[0];
      this.workOrdersSubject.next([newSummary, ...currentSummaries]);
      this.totalWorkOrders = currentSummaries.length + 1;
    });

    // Subscribe to management service data
    this.materialManagementService.dashboardData$
      .pipe(takeUntil(this.destroy$))
      .subscribe(data => {
        if (data) {
          this.updateSummaryData(data);
        }
      });
  }

  private loadData(): void {
    // Load materials first, then create allocations based on those materials
    this.materialService.loadMaterials().subscribe(() => {
      this.loadRealWorkOrders();
      this.loadMockMaterialAllocations();
    });
  }

  private loadRealWorkOrders(): void {
    this.workOrderService.getAllWorkOrders().pipe(
      takeUntil(this.destroy$),
      map(workOrders => this.transformToMaterialSummary(workOrders))
    ).subscribe({
      next: (summaries) => {
        this.workOrdersSubject.next(summaries);
        this.totalWorkOrders = summaries.length;
      },
      error: (error) => {
        console.error('Error loading work orders:', error);
        this.snackBar.open('Failed to load work orders', 'Close', { duration: 3000 });
        // Fallback to empty array
        this.workOrdersSubject.next([]);
        this.totalWorkOrders = 0;
      }
    });
  }

  private transformToMaterialSummary(workOrders: WorkOrder[]): WorkOrderMaterialSummary[] {
    return workOrders.map(wo => {
      const materials = wo.materials || [];
      const purchasableMaterials = materials.filter(m => m.purchasableMaterial);
      const receivableMaterials = materials.filter(m => m.receivableMaterial);
      
      const deliveredMaterials = purchasableMaterials.filter(m => 
        m.purchasableMaterial?.status === 'delivered'
      ).length;
      
      const usedMaterials = purchasableMaterials.filter(m => 
        m.purchasableMaterial?.status === 'used'
      ).length;

      const totalEstimatedCost = this.calculateEstimatedCost(materials);
      const totalActualCost = this.calculateActualCost(materials);

      // Calculate pending material actions
      const pendingActions = materials.filter(m => {
        if (m.purchasableMaterial) {
          return ['pending', 'ordered'].includes(m.purchasableMaterial.status);
        } else if (m.receivableMaterial) {
          return ['pending', 'ordered'].includes(m.receivableMaterial.status);
        }
        return false;
      }).length;

      // Check for recent material updates (within last 24 hours)
      const oneDayAgo = new Date();
      oneDayAgo.setDate(oneDayAgo.getDate() - 1);
      const hasRecentUpdates = materials.some(m => {
        const lastUpdate = m.purchasableMaterial?.delivery?.receivedDate || 
                          m.receivableMaterial?.receiving?.receivedDate ||
                          m.assignDate;
        return lastUpdate && new Date(lastUpdate) > oneDayAgo;
      });

      return {
        workOrderId: wo.id,
        workOrderNumber: wo.details.workOrderNumber,
        workOrderTitle: wo.details.title || `${wo.details.category} - ${wo.details.client}`,
        status: wo.details.status,
        totalMaterials: materials.length,
        assignedMaterials: materials.length,
        deliveredMaterials,
        usedMaterials,
        totalEstimatedCost,
        totalActualCost,
        completionPercentage: wo.details.completionPercentage || 0,
        urgency: wo.details.priority,
        lastUpdated: new Date(wo.details.lastUpdated || wo.details.createdDate),
        hasRecentMaterialUpdates: hasRecentUpdates,
        pendingMaterialActions: pendingActions
      };
    });
  }

  private calculateEstimatedCost(materials: materialAssignment[]): number {
    return materials.reduce((total, material) => {
      if (material.purchasableMaterial) {
        return total + (material.purchasableMaterial.totalCost || 0);
      }
      return total;
    }, 0);
  }

  private calculateActualCost(materials: materialAssignment[]): number {
    return materials.reduce((total, material) => {
      if (material.purchasableMaterial && material.purchasableMaterial.status === 'used') {
        return total + (material.purchasableMaterial.totalCost || 0);
      }
      return total;
    }, 0);
  }

  private loadMockMaterialAllocations(): void {
    // Get all materials from the service and create allocation data for each
    this.materialService.materials$.pipe(
      takeUntil(this.destroy$)
    ).subscribe(materials => {
      console.log('[DEBUG] Creating allocation data for', materials.length, 'materials');
      
      const mockAllocations: MaterialAllocationSummary[] = materials.map((material, index) => {
        // Generate realistic allocation data based on material properties
        const totalAllocated = (material.totalStock || 0) * 0.8; // 80% of total stock allocated
        const totalUsed = totalAllocated * (0.6 + Math.random() * 0.3); // 60-90% used
        const totalRemaining = totalAllocated - totalUsed;
        const allocatedToWorkOrders = Math.floor(Math.random() * 5) + 1; // 1-5 work orders
        const pendingDeliveries = Math.floor(Math.random() * 3); // 0-2 pending deliveries
        
        return {
          materialId: material.id!,
          materialCode: material.code,
          materialDescription: material.description,
          totalAllocated: Math.round(totalAllocated),
          totalUsed: Math.round(totalUsed),
          totalRemaining: Math.round(totalRemaining),
          allocatedToWorkOrders,
          pendingDeliveries,
          unit: material.unit,
          estimatedValue: Math.round(totalAllocated * (material.averageCost || 100)),
          actualValue: Math.round(totalUsed * (material.averageCost || 100))
        };
      });

      console.log('[DEBUG] Created allocation data for', mockAllocations.length, 'materials');
      this.materialAllocationsSubject.next(mockAllocations);
      this.totalMaterialsAllocated = mockAllocations.reduce((sum, alloc) => sum + alloc.totalAllocated, 0);
      this.pendingDeliveries = mockAllocations.reduce((sum, alloc) => sum + alloc.pendingDeliveries, 0);
      this.totalValue = mockAllocations.reduce((sum, alloc) => sum + alloc.actualValue, 0);
    });
  }

  private updateSummaryData(data: any): void {
    // Update summary cards with real data when available
    this.totalMaterialsAllocated = data.totalMaterialsAllocated || 0;
    this.pendingDeliveries = data.pendingDeliveries || 0;
    this.totalValue = data.totalValue || 0;
  }

  private filterWorkOrders(
    workOrders: WorkOrderMaterialSummary[], 
    searchTerm: string, 
    status: string, 
    urgency: string
  ): WorkOrderMaterialSummary[] {
    return workOrders.filter(wo => {
      const matchesSearch = !searchTerm || 
        wo.workOrderNumber.toLowerCase().includes(searchTerm.toLowerCase()) ||
        wo.workOrderTitle.toLowerCase().includes(searchTerm.toLowerCase());
      
      const matchesStatus = !status || wo.status === status;
      const matchesUrgency = !urgency || wo.urgency === urgency;

      return matchesSearch && matchesStatus && matchesUrgency;
    });
  }

  private filterMaterialAllocations(
    allocations: MaterialAllocationSummary[], 
    searchTerm: string, 
    status: string
  ): MaterialAllocationSummary[] {
    return allocations.filter(alloc => {
      const matchesSearch = !searchTerm || 
        alloc.materialCode.toLowerCase().includes(searchTerm.toLowerCase()) ||
        alloc.materialDescription.toLowerCase().includes(searchTerm.toLowerCase());
      
      const allocationStatus = this.getAllocationStatus(alloc);
      const matchesStatus = !status || allocationStatus.toLowerCase().includes(status.toLowerCase());

      return matchesSearch && matchesStatus;
    });
  }

  getAllocationStatus(allocation: MaterialAllocationSummary): string {
    const usagePercentage = (allocation.totalUsed / allocation.totalAllocated) * 100;
    
    if (usagePercentage > 95) return 'Over Allocated';
    if (usagePercentage < 60) return 'Under Allocated';
    return 'Optimal';
  }

  getAllocationStatusClass(allocation: MaterialAllocationSummary): string {
    const status = this.getAllocationStatus(allocation);
    return `allocation-${status.toLowerCase().replace(' ', '-')}`;
  }

  // Action Methods
  createMaterialRequisition(): void {
    this.materialService.materials$.pipe(
      takeUntil(this.destroy$)
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
              this.snackBar.open(
                `Requisition ${requisition.requestNumber} created successfully`, 
                'Close', 
                { duration: 3000, panelClass: ['success-snackbar'] }
              );
              this.refreshData();
            },
            error: (error) => {
              this.snackBar.open(
                `Failed to create requisition: ${error.message}`, 
                'Close', 
                { duration: 5000, panelClass: ['error-snackbar'] }
              );
            }
          });
        }
      });
    });
  }

  refreshData(): void {
    // Load materials first, then create allocations based on those materials
    this.materialService.loadMaterials().subscribe(() => {
      this.loadRealWorkOrders();
      this.loadMockMaterialAllocations();
      this.materialManagementService.loadDashboardData().subscribe();
    });
  }

  // Work Order Actions
  viewWorkOrderMaterials(workOrder: WorkOrderMaterialSummary): void {
    this.openWorkOrderMaterialsDialog(workOrder);
  }

  requestMaterialsForWorkOrder(workOrder: WorkOrderMaterialSummary): void {
    console.log('[DEBUG] Requesting materials for work order:', workOrder.workOrderNumber);
    
    this.materialService.materials$.pipe(
      takeUntil(this.destroy$)
    ).subscribe(materials => {
      console.log('[DEBUG] Materials loaded from service:', materials.length);
      console.log('[DEBUG] Materials:', materials);
      
      const dialogRef = this.dialog.open(MaterialRequisitionDialogComponent, {
        width: '1000px',
        maxWidth: '95vw',
        height: '90vh',
        data: {
          availableMaterials: materials,
          requestType: 'work-order',
          workOrderId: workOrder.workOrderId,
          workOrderNumber: workOrder.workOrderNumber
        }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.materialManagementService.processRequisition(result).subscribe({
            next: (requisition) => {
              this.snackBar.open(
                `Materials requested for ${workOrder.workOrderNumber}`, 
                'Close', 
                { duration: 3000, panelClass: ['success-snackbar'] }
              );
              this.refreshData();
            },
            error: (error) => {
              this.snackBar.open(
                `Failed to request materials: ${error.message}`, 
                'Close', 
                { duration: 5000, panelClass: ['error-snackbar'] }
              );
            }
          });
        }
      });
    });
  }

  optimizeMaterials(workOrder: WorkOrderMaterialSummary): void {
    // Get the full work order data for optimization analysis
    this.workOrderService.getWorkOrderById(workOrder.workOrderId).pipe(
      takeUntil(this.destroy$)
    ).subscribe({
      next: (fullWorkOrder) => {
        const optimizationSuggestions = this.analyzeMaterialOptimization(fullWorkOrder);
        
        // Show optimization dialog with suggestions
        this.showOptimizationDialog(workOrder, optimizationSuggestions);
      },
      error: (error) => {
        console.error('Error loading work order for optimization:', error);
        this.snackBar.open('Failed to load work order data for optimization', 'Close', { duration: 3000 });
      }
    });
  }

  private analyzeMaterialOptimization(workOrder: WorkOrder): any[] {
    const suggestions: any[] = [];
    const materials = workOrder.materials || [];

    // Analyze each material for optimization opportunities
    materials.forEach(material => {
      if (material.purchasableMaterial) {
        const pm = material.purchasableMaterial;
        
        // Check for over-ordering
        if (pm.quantity > 0 && pm.status === 'pending') {
          const estimatedUsage = this.estimateMaterialUsage(workOrder, material);
          if (pm.quantity > estimatedUsage * 1.2) { // 20% buffer
            suggestions.push({
              type: 'over-ordering',
              material: pm.name,
              currentQuantity: pm.quantity,
              suggestedQuantity: Math.ceil(estimatedUsage * 1.1),
              potentialSavings: (pm.quantity - Math.ceil(estimatedUsage * 1.1)) * (pm.unitCost || 0),
              reason: 'Quantity exceeds estimated usage by more than 20%'
            });
          }
        }

        // Check for delivery timing
        if (pm.status === 'ordered' && pm.deliveryDate) {
          const deliveryDate = new Date(pm.deliveryDate);
          const workOrderStart = new Date(workOrder.details.startDate || workOrder.details.createdDate);
          const daysDifference = (deliveryDate.getTime() - workOrderStart.getTime()) / (1000 * 60 * 60 * 24);
          
          if (daysDifference > 7) {
            suggestions.push({
              type: 'delivery-timing',
              material: pm.name,
              deliveryDate: pm.deliveryDate,
              workOrderStart: workOrder.details.startDate,
              daysDifference: Math.ceil(daysDifference),
              reason: 'Material delivery is scheduled too early'
            });
          }
        }
      }
    });

    // Check for missing critical materials
    const criticalMaterials = this.identifyCriticalMaterials(workOrder);
    const assignedMaterialNames = materials.map(m => 
      m.purchasableMaterial?.name || m.receivableMaterial?.name
    ).filter(Boolean);
    
    criticalMaterials.forEach(critical => {
      if (!assignedMaterialNames.includes(critical.name)) {
        suggestions.push({
          type: 'missing-critical',
          material: critical.name,
          reason: 'Critical material for this work order type is not assigned',
          priority: 'high'
        });
      }
    });

    return suggestions;
  }

  private estimateMaterialUsage(workOrder: WorkOrder, material: materialAssignment): number {
    // Simple estimation based on work order completion percentage and material type
    const baseQuantity = material.purchasableMaterial?.quantity || 0;
    const completionPercentage = workOrder.details.completionPercentage || 0;
    
    // Adjust based on completion percentage
    if (completionPercentage > 0) {
      return Math.ceil(baseQuantity * (completionPercentage / 100));
    }
    
    // Default estimation based on work order type
    const workOrderType = workOrder.details.category?.toLowerCase() || '';
    if (workOrderType.includes('maintenance')) {
      return Math.ceil(baseQuantity * 0.8);
    } else if (workOrderType.includes('installation')) {
      return Math.ceil(baseQuantity * 0.9);
    } else if (workOrderType.includes('repair')) {
      return Math.ceil(baseQuantity * 0.7);
    }
    
    return Math.ceil(baseQuantity * 0.85); // Default 85% usage
  }

  private identifyCriticalMaterials(workOrder: WorkOrder): any[] {
    // Identify critical materials based on work order type
    const workOrderType = workOrder.details.category?.toLowerCase() || '';
    const criticalMaterials: any[] = [];

    if (workOrderType.includes('electrical')) {
      criticalMaterials.push({ name: 'Electrical Cables', priority: 'high' });
      criticalMaterials.push({ name: 'Circuit Breakers', priority: 'high' });
    } else if (workOrderType.includes('plumbing')) {
      criticalMaterials.push({ name: 'Pipes', priority: 'high' });
      criticalMaterials.push({ name: 'Fittings', priority: 'medium' });
    } else if (workOrderType.includes('construction')) {
      criticalMaterials.push({ name: 'Concrete', priority: 'high' });
      criticalMaterials.push({ name: 'Steel Reinforcement', priority: 'high' });
    }

    return criticalMaterials;
  }

  private showOptimizationDialog(workOrder: WorkOrderMaterialSummary, suggestions: any[]): void {
    const dialogRef = this.dialog.open(OptimizationDialogComponent, {
      width: '800px',
      data: {
        workOrder,
        suggestions
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.applyOptimizations) {
        this.applyOptimizations(workOrder.workOrderId, result.selectedOptimizations);
      }
    });
  }

  private applyOptimizations(workOrderId: string, optimizations: any[]): void {
    // Apply the selected optimizations
    optimizations.forEach(optimization => {
      if (optimization.type === 'over-ordering') {
        // Update material quantity - need to find the material assignment first
        this.workOrderService.getWorkOrderById(workOrderId).pipe(
          takeUntil(this.destroy$)
        ).subscribe(workOrder => {
          const material = workOrder.materials?.find(m => 
            m.purchasableMaterial?.name === optimization.material
          );
          
          if (material && material.purchasableMaterial) {
            const updatedMaterial = {
              ...material,
              purchasableMaterial: {
                ...material.purchasableMaterial,
                quantity: optimization.suggestedQuantity
              }
            };
            
            this.workOrderService.updateMaterialAssignment(
              workOrderId, 
              material.id, 
              updatedMaterial
            ).subscribe();
          }
        });
      }
    });

    this.snackBar.open('Optimizations applied successfully', 'Close', { duration: 3000 });
    this.refreshData();
  }

  // Material Actions
  viewMaterialDetails(allocation: MaterialAllocationSummary): void {
    this.dialog.open(MaterialAllocationDetailsDialogComponent, {
      width: '500px',
      data: allocation
    });
  }

  reallocateMaterial(allocation: MaterialAllocationSummary): void {
    const dialogRef = this.dialog.open(MaterialReallocationDialogComponent, {
      width: '1200px',
      maxWidth: '95vw',
      height: '90vh',
      data: {
        materialId: allocation.materialId,
        materialCode: allocation.materialCode,
        materialDescription: allocation.materialDescription,
        totalAllocated: allocation.totalAllocated,
        totalUsed: allocation.totalUsed,
        totalRemaining: allocation.totalRemaining,
        unit: allocation.unit
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.success) {
        this.snackBar.open(
          `Successfully reallocated ${result.reallocations.length} material(s)`, 
          'Close', 
          { duration: 3000, panelClass: ['success-snackbar'] }
        );
        this.refreshData();
      }
    });
  }

  trackMaterialUsage(allocation: MaterialAllocationSummary): void {
    // Get all work orders that use this material
    this.workOrderService.getAllWorkOrders().pipe(
      takeUntil(this.destroy$)
    ).subscribe(workOrders => {
      const workOrdersUsingMaterial = workOrders.filter(wo => 
        wo.materials?.some(m => 
          m.purchasableMaterial?.name === allocation.materialDescription ||
          m.receivableMaterial?.name === allocation.materialDescription
        )
      );

      const usageData = this.analyzeMaterialUsage(allocation, workOrdersUsingMaterial);
      this.showMaterialUsageDialog(allocation, usageData);
    });
  }

  private analyzeMaterialUsage(allocation: MaterialAllocationSummary, workOrders: WorkOrder[]): any {
    const usageAnalysis = {
      totalAllocated: allocation.totalAllocated,
      totalUsed: allocation.totalUsed,
      totalRemaining: allocation.totalRemaining,
      utilizationRate: (allocation.totalUsed / allocation.totalAllocated) * 100,
      workOrders: workOrders.map(wo => {
        const material = wo.materials?.find(m => 
          m.purchasableMaterial?.name === allocation.materialDescription ||
          m.receivableMaterial?.name === allocation.materialDescription
        );
        
        return {
          workOrderId: wo.id,
          workOrderNumber: wo.details.workOrderNumber,
          workOrderTitle: wo.details.title || wo.details.category,
          status: wo.details.status,
          completionPercentage: wo.details.completionPercentage || 0,
          allocatedQuantity: material?.purchasableMaterial?.quantity || material?.receivableMaterial?.estimatedQuantity || 0,
          usedQuantity: this.calculateUsedQuantity(material, wo),
          usagePercentage: 0, // Will be calculated
          lastUsed: this.getLastUsageDate(material),
          cost: material?.purchasableMaterial?.totalCost || 0
        };
      })
    };

    // Calculate usage percentages
    usageAnalysis.workOrders.forEach(wo => {
      wo.usagePercentage = wo.allocatedQuantity > 0 ? (wo.usedQuantity / wo.allocatedQuantity) * 100 : 0;
    });

    return usageAnalysis;
  }

  private calculateUsedQuantity(material: materialAssignment | undefined, workOrder: WorkOrder): number {
    if (!material) return 0;

    if (material.purchasableMaterial) {
      // For purchasable materials, estimate usage based on completion percentage
      const completionPercentage = workOrder.details.completionPercentage || 0;
      return Math.ceil((material.purchasableMaterial.quantity * completionPercentage) / 100);
    } else if (material.receivableMaterial) {
      // For receivable materials, use actual quantity if available
      return material.receivableMaterial.actualQuantity || material.receivableMaterial.estimatedQuantity || 0;
    }

    return 0;
  }

  private getLastUsageDate(material: materialAssignment | undefined): Date | null {
    if (!material) return null;

    if (material.purchasableMaterial?.status === 'used') {
      return new Date(); // Mock - in real app, this would come from usage records
    } else if (material.receivableMaterial?.usageRecords && material.receivableMaterial.usageRecords.length > 0) {
      const lastRecord = material.receivableMaterial.usageRecords[0];
      return new Date(lastRecord.recordDate);
    }

    return null;
  }

  private showMaterialUsageDialog(allocation: MaterialAllocationSummary, usageData: any): void {
    const dialogRef = this.dialog.open(MaterialUsageDialogComponent, {
      width: '900px',
      data: {
        allocation,
        usageData
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Handle any actions from the usage dialog
        this.snackBar.open('Material usage data exported successfully', 'Close', { duration: 3000 });
      }
    });
  }

  // View materials assigned to a work order
  openWorkOrderMaterialsDialog(workOrder: WorkOrderMaterialSummary): void {
    this.workOrderService.getWorkOrderById(workOrder.workOrderId).pipe(
      takeUntil(this.destroy$)
    ).subscribe(fullWorkOrder => {
      const dialogRef = this.dialog.open(WorkOrderMaterialsDialogComponent, {
        width: '800px',
        data: {
          workOrderId: fullWorkOrder.id,
          workOrderNumber: fullWorkOrder.details.workOrderNumber,
          workOrderTitle: fullWorkOrder.details.title || fullWorkOrder.details.category,
          materials: fullWorkOrder.materials || []
        }
      });

      // Refresh data when dialog is closed to show updated statuses
      dialogRef.afterClosed().subscribe(() => {
        this.refreshData();
      });
    });
  }
} 