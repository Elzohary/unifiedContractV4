import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subject, takeUntil, Observable, combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';

// Angular Material
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatBadgeModule } from '@angular/material/badge';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

// Shared Components and Services
import { HelpService } from '../../../../shared/services/help.service';
import { HelpDirective } from '../../../../shared/directives/help.directive';
import { HelpCenterComponent } from '../../../../shared/components/help-center/help-center.component';
import { BreadcrumbComponent } from '../../../../shared/components/breadcrumb/breadcrumb.component';

// Materials Services
import { MaterialManagementService, MaterialDashboardData } from '../../services/material-management.service';

// Centralized Mock Database Service
import { MockDatabaseService } from '../../../../core/services/mock-database.service';

// Models
import { WorkOrder } from '../../../work-order/models/work-order.model';
import { BaseMaterial } from '../../models/material.model';

interface MaterialNeeded {
  id: string;
  name: string;
  quantityNeeded: number;
  quantityAvailable: number;
  unit: string;
}

interface WorkOrderWithMaterialNeeds extends WorkOrder {
  materialsNeeded: MaterialNeeded[];
}

interface OverviewData {
  totalMaterials: number;
  lowStockAlerts: number;
  pendingAllocations: number;
  totalValue: number;
  movementsToday: number;
  activePurchaseOrders: number;
}

@Component({
  selector: 'app-materials-hub',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatBadgeModule,
    MatTooltipModule,
    MatDialogModule,
    HelpDirective,
    BreadcrumbComponent
  ],
  templateUrl: './materials-hub.component.html',
  styleUrls: ['./materials-hub.component.scss']
})
export class MaterialsHubComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  // Help System
  isHelpModeActive$ = this.helpService.isHelpModeActive$;

  // Data observables
  workOrdersWithMaterialNeeds$!: Observable<WorkOrderWithMaterialNeeds[]>;
  workOrdersRequiringMaterials: WorkOrderWithMaterialNeeds[] = [];
  
  overviewData: OverviewData = {
    totalMaterials: 0,
    lowStockAlerts: 0,
    pendingAllocations: 0,
    totalValue: 0,
    movementsToday: 0,
    activePurchaseOrders: 0
  };

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private materialService: MaterialManagementService,
    private helpService: HelpService,
    private mockDb: MockDatabaseService
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadData(): void {
    // Combine work orders and materials data from centralized mock database
    this.workOrdersWithMaterialNeeds$ = combineLatest([
      this.mockDb.workOrders$,
      this.mockDb.materials$,
      this.materialService.dashboardData$
    ]).pipe(
      map(([workOrders, materials, dashboardData]: [WorkOrder[], BaseMaterial[], MaterialDashboardData | null]) => {
        // Filter work orders that require materials and are not completed
        const workOrdersNeedingMaterials = workOrders
          .filter((wo: WorkOrder) => wo.details.status !== 'completed' && 
                        wo.details.status !== 'cancelled')
          .map((workOrder: WorkOrder) => {
            // Use real material assignments from the work order
            const materialsNeeded = (workOrder.materials || []).map(assign => {
              if (assign.materialType === 'purchasable' && assign.purchasableMaterial) {
                return {
                  id: assign.purchasableMaterial.id,
                  name: assign.purchasableMaterial.name,
                  quantityNeeded: assign.purchasableMaterial.quantity,
                  quantityAvailable: this.getAvailableStockForMaterial(assign.purchasableMaterial.id, materials),
                  unit: assign.purchasableMaterial.unit
                };
              } else if (assign.materialType === 'receivable' && assign.receivableMaterial) {
                return {
                  id: assign.receivableMaterial.id,
                  name: assign.receivableMaterial.name,
                  quantityNeeded: assign.receivableMaterial.estimatedQuantity,
                  quantityAvailable: this.getAvailableStockForMaterial(assign.receivableMaterial.id, materials),
                  unit: assign.receivableMaterial.unit
                };
              }
              return null;
            }).filter(Boolean);
            return {
              ...workOrder,
              materialsNeeded
            } as WorkOrderWithMaterialNeeds;
          })
          .filter((wo: WorkOrderWithMaterialNeeds) => wo.materialsNeeded.length > 0)
          .sort((a: WorkOrderWithMaterialNeeds, b: WorkOrderWithMaterialNeeds) => {
            // Prioritize by urgency and material availability
            const priorityOrder = { 'high': 3, 'medium': 2, 'low': 1 };
            const aPriority = priorityOrder[a.details.priority as keyof typeof priorityOrder] || 1;
            const bPriority = priorityOrder[b.details.priority as keyof typeof priorityOrder] || 1;
            if (aPriority !== bPriority) {
              return bPriority - aPriority;
            }
            // Secondary sort by due date
            const aDate = new Date(a.details.dueDate || '').getTime();
            const bDate = new Date(b.details.dueDate || '').getTime();
            return aDate - bDate;
          });
        return workOrdersNeedingMaterials;
      })
    );

    // Subscribe to the combined data
    this.workOrdersWithMaterialNeeds$
      .pipe(takeUntil(this.destroy$))
      .subscribe(workOrders => {
        this.workOrdersRequiringMaterials = workOrders;
      });

    // Load overview data from centralized sources
    combineLatest([
      this.mockDb.materials$,
      this.mockDb.workOrders$,
      this.materialService.dashboardData$
    ])
      .pipe(takeUntil(this.destroy$))
      .subscribe(([materials, workOrders, dashboardData]: [BaseMaterial[], WorkOrder[], MaterialDashboardData | null]) => {
        // Calculate overview data from centralized mock database
        const totalMaterials = materials.length;
        const lowStockAlerts = materials.filter(m => (m.totalStock || 0) < (m.minimumStock || 10)).length;
        const pendingAllocations = workOrders.filter(wo => 
          wo.details.status === 'pending' || wo.details.status === 'in-progress'
        ).length;
        
        // Calculate total value from materials
        const totalValue = materials.reduce((sum: number, material: BaseMaterial) => {
          const stock = material.totalStock || 0;
          const unitPrice = material.averageCost || material.lastPurchaseCost || 0;
          return sum + (stock * unitPrice);
        }, 0);

        // Get movements today from dashboard data
        const movementsToday = dashboardData?.recentMovements?.length || 0;
        
        // Mock active purchase orders (this would come from procurement service in real app)
        const activePurchaseOrders = Math.floor(Math.random() * 12) + 3;

        this.overviewData = {
          totalMaterials,
          lowStockAlerts,
          pendingAllocations,
          totalValue,
          movementsToday,
          activePurchaseOrders
        };
      });

    // Load the dashboard data if not already loaded
    this.materialService.loadDashboardData().subscribe();
  }

  private getAvailableStockForMaterial(materialId: string, materials: BaseMaterial[]): number {
    const found = materials.find(m => m.id === materialId);
    return found ? (found.totalStock || 0) : 0;
  }

  // Navigation methods
  allocateMaterials(workOrderId: string): void {
    this.router.navigate(['/materials/work-order-hub'], {
      queryParams: { workOrderId }
    });
  }

  viewWorkOrder(workOrderId: string): void {
    this.router.navigate(['/work-orders', workOrderId]);
  }

  // Remove unused parameter
  private handleWorkOrderAction(action: string): void {
    // Implementation for handling work order actions
  }

  // Help system methods
  openHelpCenter(): void {
    this.dialog.open(HelpCenterComponent, {
      width: '800px',
      maxWidth: '95vw',
      data: { topic: 'materials-hub' }
    });
  }

  navigateTo(route: string): void {
    this.router.navigate([route]);
  }

  toggleHelpMode(): void {
    this.helpService.toggleHelpMode();
  }
} 