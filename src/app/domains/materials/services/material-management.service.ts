import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, throwError, combineLatest } from 'rxjs';
import { map, switchMap, tap, catchError, delay } from 'rxjs/operators';
import { MaterialService } from './material.service';
import { MaterialIntegrationService } from './material-integration.service';
import { BaseMaterial, StockStatus, calculateStockStatus } from '../models/material.model';
import { MaterialMovement, StockAdjustment } from '../models/inventory.model';
import { MockDatabaseService } from '../../../core/services/mock-database.service';
import { StockAdjustmentResult } from '../components/dialogs/stock-adjustment-dialog/stock-adjustment-dialog.component';
import { MaterialRequisitionResult } from '../components/dialogs/material-requisition-dialog/material-requisition-dialog.component';

export interface MaterialRequisition {
  id: string;
  requestNumber: string;
  requestType: 'work-order' | 'maintenance' | 'general';
  workOrderId?: string;
  workOrderNumber?: string;
  requestedBy: string;
  requestedByName: string;
  requestDate: Date;
  requiredBy: Date;
  status: 'pending' | 'approved' | 'rejected' | 'partially-fulfilled' | 'fulfilled' | 'cancelled';
  items: RequisitionItem[];
  justification: string;
  totalEstimatedCost: number;
  urgency: 'low' | 'medium' | 'high' | 'critical';
  approvalRequired: boolean;
  approvedBy?: string;
  approvedDate?: Date;
  notes?: string;
}

export interface RequisitionItem {
  id: string;
  materialId: string;
  material: BaseMaterial;
  requestedQuantity: number;
  approvedQuantity?: number;
  fulfilledQuantity?: number;
  remainingQuantity?: number;
  urgency: 'low' | 'medium' | 'high' | 'critical';
  status: 'pending' | 'approved' | 'rejected' | 'fulfilled' | 'partially-fulfilled';
  notes?: string;
  estimatedCost?: number;
  actualCost?: number;
}

export interface StockAlert {
  id: string;
  type: 'low-stock' | 'out-of-stock' | 'overstocked' | 'expiring' | 'expired';
  severity: 'low' | 'medium' | 'high' | 'critical';
  materialId: string;
  materialCode: string;
  materialDescription: string;
  currentStock: number;
  thresholdValue: number;
  message: string;
  actionRequired: string;
  dateDetected: Date;
  isActive: boolean;
  acknowledgedBy?: string;
  acknowledgedDate?: Date;
}

export interface MaterialDashboardData {
  totalMaterials: number;
  totalValue: number;
  lowStockItems: number;
  outOfStockItems: number;
  pendingRequisitions: number;
  pendingAdjustments: number;
  recentMovements: MaterialMovement[];
  stockAlerts: StockAlert[];
  topUsedMaterials: MaterialUsageSummary[];
  monthlyTrends: MaterialTrend[];
}

export interface MaterialUsageSummary {
  materialId: string;
  materialCode: string;
  materialDescription: string;
  totalUsed: number;
  totalCost: number;
  usageFrequency: number;
  lastUsedDate: Date;
}

export interface MaterialTrend {
  month: string;
  totalValue: number;
  totalMovements: number;
  newMaterials: number;
  lowStockItems: number;
}

@Injectable({
  providedIn: 'root'
})
export class MaterialManagementService {
  // State Management
  private requisitionsSubject = new BehaviorSubject<MaterialRequisition[]>([]);
  private adjustmentsSubject = new BehaviorSubject<StockAdjustment[]>([]);
  private alertsSubject = new BehaviorSubject<StockAlert[]>([]);
  private dashboardDataSubject = new BehaviorSubject<MaterialDashboardData | null>(null);
  private loadingSubject = new BehaviorSubject<boolean>(false);
  private errorSubject = new BehaviorSubject<string | null>(null);

  // Public Observables
  public requisitions$ = this.requisitionsSubject.asObservable();
  public adjustments$ = this.adjustmentsSubject.asObservable();
  public alerts$ = this.alertsSubject.asObservable();
  public dashboardData$ = this.dashboardDataSubject.asObservable();
  public loading$ = this.loadingSubject.asObservable();
  public error$ = this.errorSubject.asObservable();

  // Filtered Observables
  public activeAlerts$ = this.alerts$.pipe(
    map(alerts => alerts.filter(alert => alert.isActive))
  );

  public pendingRequisitions$ = this.requisitions$.pipe(
    map(requisitions => requisitions.filter(req => req.status === 'pending'))
  );

  public criticalAlerts$ = this.alerts$.pipe(
    map(alerts => alerts.filter(alert => 
      alert.isActive && (alert.severity === 'high' || alert.severity === 'critical')
    ))
  );

  constructor(
    private materialService: MaterialService,
    private integrationService: MaterialIntegrationService,
    private mockDb: MockDatabaseService
  ) {
    this.initializeService();
  }

  private initializeService(): void {
    this.loadDashboardData();
    this.loadRequisitions();
    this.loadAdjustments();
    this.loadAlerts();
  }

  /**
   * Process stock adjustment
   */
  processStockAdjustment(adjustmentData: StockAdjustmentResult): Observable<boolean> {
    this.setLoading(true);
    
    const adjustment: StockAdjustment = {
      id: this.generateId(),
      materialId: adjustmentData.materialId,
      warehouseId: 'main-warehouse', // Default warehouse
      adjustmentType: adjustmentData.adjustmentType === 'set-absolute' ? 'increase' : adjustmentData.adjustmentType,
      quantity: adjustmentData.quantity,
      reason: adjustmentData.reason,
      notes: adjustmentData.notes,
      performedBy: adjustmentData.performedBy,
      performedDate: adjustmentData.adjustmentDate,
      status: 'pending'
    };

    return this.materialService.materials$.pipe(
      map(materials => materials.find(m => m.id === adjustmentData.materialId)),
      switchMap(material => {
        if (!material) {
          return throwError(() => new Error('Material not found'));
        }

        // Calculate new stock level
        const currentStock = material.totalStock || 0;
        let newStock = currentStock;
        let actualQuantity = adjustmentData.quantity;

        switch (adjustmentData.adjustmentType) {
          case 'increase':
            newStock = currentStock + adjustmentData.quantity;
            break;
          case 'decrease':
            newStock = Math.max(0, currentStock - adjustmentData.quantity);
            actualQuantity = currentStock - newStock; // Actual decrease amount
            break;
          case 'set-absolute':
            newStock = adjustmentData.quantity;
            actualQuantity = Math.abs(newStock - currentStock);
            adjustment.adjustmentType = newStock > currentStock ? 'increase' : 'decrease';
            break;
        }

        // Update adjustment quantity to actual amount
        adjustment.quantity = actualQuantity;

        // Create movement record
        const movement: MaterialMovement = {
          id: this.generateId(),
          movementNumber: this.generateMovementNumber(),
          materialId: material.id!,
          materialCode: material.code,
          materialDescription: material.description,
          movementType: adjustment.adjustmentType === 'increase' ? 'receipt' : 'issue',
          quantity: actualQuantity,
          unit: material.unit,
          fromLocation: adjustment.adjustmentType === 'increase' ? 
            { type: 'warehouse', id: 'external-adjustment', name: 'Stock Adjustment' } : 
            { type: 'warehouse', id: 'main-warehouse', name: 'Main Warehouse' },
          toLocation: adjustment.adjustmentType === 'increase' ? 
            { type: 'warehouse', id: 'main-warehouse', name: 'Main Warehouse' } : 
            { type: 'warehouse', id: 'external-adjustment', name: 'Stock Adjustment' },
          relatedEntity: {
            type: 'adjustment',
            id: adjustment.id,
            reference: `ADJ-${adjustment.id.slice(-6)}`
          },
          performedBy: adjustmentData.performedBy,
          performedByName: adjustmentData.performedBy, // TODO: Get actual name
          performedDate: adjustmentData.adjustmentDate,
          notes: `Stock adjustment: ${adjustmentData.reason}`
        };

        // Save adjustment and movement
        return this.saveAdjustmentAndMovement(adjustment, movement, material, newStock);
      }),
      tap(() => {
        this.refreshData();
        this.setLoading(false);
      }),
      catchError(error => {
        this.setError(`Failed to process stock adjustment: ${error.message}`);
        this.setLoading(false);
        return throwError(() => error);
      })
    );
  }

  /**
   * Process material requisition
   */
  processRequisition(requisitionData: MaterialRequisitionResult): Observable<MaterialRequisition> {
    this.setLoading(true);

    const requisition: MaterialRequisition = {
      id: this.generateId(),
      requestNumber: this.generateRequisitionNumber(),
      requestType: requisitionData.requestType,
      workOrderId: requisitionData.workOrderId,
      workOrderNumber: requisitionData.workOrderNumber,
      requestedBy: requisitionData.requestedBy,
      requestedByName: requisitionData.requestedBy, // TODO: Get actual name
      requestDate: requisitionData.requestDate,
      requiredBy: requisitionData.requiredBy,
      status: requisitionData.approvalRequired ? 'pending' : 'approved',
      items: requisitionData.items.map((item, index) => ({
        id: `${this.generateId()}-${index}`,
        materialId: item.materialId,
        material: item.material,
        requestedQuantity: item.requestedQuantity,
        urgency: item.urgency,
        status: requisitionData.approvalRequired ? 'pending' : 'approved',
        notes: item.notes,
        estimatedCost: item.estimatedCost
      })),
      justification: requisitionData.justification,
      totalEstimatedCost: requisitionData.totalEstimatedCost,
      urgency: requisitionData.urgency,
      approvalRequired: requisitionData.approvalRequired
    };

    return this.saveRequisition(requisition).pipe(
      tap(() => {
        this.refreshData();
        this.setLoading(false);
      }),
      catchError(error => {
        this.setError(`Failed to process requisition: ${error.message}`);
        this.setLoading(false);
        return throwError(() => error);
      })
    );
  }

  /**
   * Approve requisition
   */
  approveRequisition(requisitionId: string, approvedBy: string, notes?: string): Observable<boolean> {
    const requisitions = this.requisitionsSubject.value;
    const requisition = requisitions.find(r => r.id === requisitionId);
    
    if (!requisition) {
      return throwError(() => new Error('Requisition not found'));
    }

    requisition.status = 'approved';
    requisition.approvedBy = approvedBy;
    requisition.approvedDate = new Date();
    if (notes) requisition.notes = notes;

    // Update all items to approved
    requisition.items.forEach(item => {
      item.status = 'approved';
      item.approvedQuantity = item.requestedQuantity;
      item.remainingQuantity = item.requestedQuantity;
    });

    this.requisitionsSubject.next([...requisitions]);
    return of(true).pipe(delay(500));
  }

  /**
   * Generate stock alerts
   */
  generateStockAlerts(): Observable<StockAlert[]> {
    return this.materialService.materials$.pipe(
      map(materials => {
        const alerts: StockAlert[] = [];

        materials.forEach(material => {
          const stock = material.totalStock || 0;
          const stockStatus = calculateStockStatus(material);

          // Low stock alert
          if (stockStatus === StockStatus.LOW_STOCK) {
            alerts.push({
              id: `low-stock-${material.id}`,
              type: 'low-stock',
              severity: stock === 0 ? 'critical' : 'high',
              materialId: material.id!,
              materialCode: material.code,
              materialDescription: material.description,
              currentStock: stock,
              thresholdValue: material.minimumStock || material.reorderPoint || 0,
              message: `Low stock: ${stock} ${material.unit} remaining`,
              actionRequired: 'Reorder required',
              dateDetected: new Date(),
              isActive: true
            });
          }

          // Out of stock alert
          if (stockStatus === StockStatus.OUT_OF_STOCK) {
            alerts.push({
              id: `out-of-stock-${material.id}`,
              type: 'out-of-stock',
              severity: 'critical',
              materialId: material.id!,
              materialCode: material.code,
              materialDescription: material.description,
              currentStock: stock,
              thresholdValue: 0,
              message: 'Out of stock',
              actionRequired: 'Immediate reorder required',
              dateDetected: new Date(),
              isActive: true
            });
          }

          // Overstocked alert
          if (material.maximumStock && stock > material.maximumStock) {
            alerts.push({
              id: `overstocked-${material.id}`,
              type: 'overstocked',
              severity: 'medium',
              materialId: material.id!,
              materialCode: material.code,
              materialDescription: material.description,
              currentStock: stock,
              thresholdValue: material.maximumStock,
              message: `Overstocked: ${stock - material.maximumStock} ${material.unit} over maximum`,
              actionRequired: 'Consider redistribution or promotion',
              dateDetected: new Date(),
              isActive: true
            });
          }
        });

        return alerts;
      }),
      tap(alerts => this.alertsSubject.next(alerts))
    );
  }

  /**
   * Load dashboard data from centralized mock database
   */
  loadDashboardData(): Observable<MaterialDashboardData> {
    this.setLoading(true);

    return combineLatest([
      this.mockDb.materials$,
      this.mockDb.workOrders$
    ]).pipe(
      map(([materials, workOrders]: [BaseMaterial[], any[]]) => {
        // Calculate dashboard data from centralized mock database
        const totalMaterials = materials.length;
        const lowStockItems = materials.filter((m: BaseMaterial) => (m.totalStock || 0) < (m.minimumStock || 10)).length;
        const outOfStockItems = materials.filter((m: BaseMaterial) => (m.totalStock || 0) === 0).length;
        const pendingRequisitions = workOrders.filter((wo: any) => 
          wo.details.status === 'pending' || wo.details.status === 'in-progress'
        ).length;
        const pendingAdjustments = 3; // Mock value for now

        // Calculate total value from materials
        const totalValue = materials.reduce((sum: number, material: BaseMaterial) => {
          const stock = material.totalStock || 0;
          const unitPrice = material.averageCost || material.lastPurchaseCost || 0;
          return sum + (stock * unitPrice);
        }, 0);

        // Generate movements from centralized data
        const recentMovements = this.generateMovementsFromCentralizedData(materials, workOrders);
        const stockAlerts = this.generateAlertsFromCentralizedData(materials);
        const topUsedMaterials = this.generateTopUsedMaterialsFromCentralizedData(materials);
        const monthlyTrends = this.generateMockMonthlyTrends();

        const dashboardData: MaterialDashboardData = {
          totalMaterials,
          totalValue,
          lowStockItems,
          outOfStockItems,
          pendingRequisitions,
          pendingAdjustments,
          recentMovements,
          stockAlerts,
          topUsedMaterials,
          monthlyTrends
        };

        this.dashboardDataSubject.next(dashboardData);
        this.setLoading(false);
        return dashboardData;
      }),
      catchError(error => {
        this.setError('Failed to load dashboard data');
        this.setLoading(false);
        return throwError(() => error);
      })
    );
  }

  // Enhanced mock data generators
  private generateMockMovements(): MaterialMovement[] {
    const movements: MaterialMovement[] = [
      {
        id: 'mov-001',
        movementNumber: 'MOV-2024-001',
        materialId: 'mat-001',
        materialCode: 'CONC-001',
        materialDescription: 'Ready Mix Concrete Grade 30',
        movementType: 'issue',
        quantity: 50,
        unit: 'm³',
        fromLocation: { type: 'warehouse', id: 'wh-001', name: 'Main Warehouse' },
        toLocation: { type: 'work-order', id: 'wo-001', name: 'WO-2024-001: Road Maintenance' },
        relatedEntity: {
          type: 'work-order',
          id: 'wo-001',
          reference: 'WO-2024-001'
        },
        performedBy: 'USR001',
        performedByName: 'Ahmed Al-Hassan',
        performedDate: new Date(Date.now() - 2 * 60 * 60 * 1000), // 2 hours ago
        notes: 'Material issued for road maintenance project'
      },
      {
        id: 'mov-002',
        movementNumber: 'MOV-2024-002',
        materialId: 'mat-002',
        materialCode: 'STEEL-002',
        materialDescription: 'Reinforcement Steel Bar 16mm',
        movementType: 'receipt',
        quantity: 1000,
        unit: 'kg',
        fromLocation: { type: 'warehouse', id: 'sup-001', name: 'External Supplier - Al-Rajhi Steel Co.' },
        toLocation: { type: 'warehouse', id: 'wh-001', name: 'Main Warehouse' },
        relatedEntity: {
          type: 'purchase-order',
          id: 'po-001',
          reference: 'PO-2024-001'
        },
        performedBy: 'USR002',
        performedByName: 'Mohammed Al-Zahra',
        performedDate: new Date(Date.now() - 4 * 60 * 60 * 1000), // 4 hours ago
        notes: 'Steel delivery from supplier'
      }
    ];
    return movements;
  }

  private generateMockAlerts(): StockAlert[] {
    const alerts: StockAlert[] = [
      {
        id: 'alert-001',
        type: 'low-stock',
        severity: 'high',
        materialId: 'mat-003',
        materialCode: 'CEMENT-001',
        materialDescription: 'Portland Cement Type I',
        currentStock: 15,
        thresholdValue: 50,
        message: 'Stock level below minimum threshold',
        actionRequired: 'Reorder required - affects 3 active work orders',
        dateDetected: new Date(Date.now() - 24 * 60 * 60 * 1000), // 1 day ago
        isActive: true
      },
      {
        id: 'alert-002',
        type: 'out-of-stock',
        severity: 'critical',
        materialId: 'mat-004',
        materialCode: 'PIPES-003',
        materialDescription: 'PVC Pipes 200mm Diameter',
        currentStock: 0,
        thresholdValue: 0,
        message: 'Material completely out of stock',
        actionRequired: 'URGENT: Immediate procurement needed for WO-2024-003',
        dateDetected: new Date(Date.now() - 6 * 60 * 60 * 1000), // 6 hours ago
        isActive: true
      }
    ];
    return alerts;
  }

  private generateMockTopUsedMaterials(): MaterialUsageSummary[] {
    return [
      {
        materialId: 'mat-001',
        materialCode: 'CONC-001',
        materialDescription: 'Ready Mix Concrete Grade 30',
        totalUsed: 850,
        totalCost: 127500,
        usageFrequency: 15,
        lastUsedDate: new Date(Date.now() - 2 * 60 * 60 * 1000)
      },
      {
        materialId: 'mat-002',
        materialCode: 'STEEL-002',
        materialDescription: 'Reinforcement Steel Bar 16mm',
        totalUsed: 2450,
        totalCost: 98000,
        usageFrequency: 12,
        lastUsedDate: new Date(Date.now() - 4 * 60 * 60 * 1000)
      }
    ];
  }

  private generateMockMonthlyTrends(): MaterialTrend[] {
    const months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'];
    return months.map((month, index) => ({
      month,
      totalValue: 2200000 + (index * 50000) + Math.random() * 100000,
      totalMovements: 150 + Math.floor(Math.random() * 50),
      newMaterials: Math.floor(Math.random() * 10),
      lowStockItems: Math.floor(Math.random() * 30)
    }));
  }

  /**
   * Generate movements from centralized data
   */
  private generateMovementsFromCentralizedData(materials: BaseMaterial[], workOrders: any[]): MaterialMovement[] {
    const movements: MaterialMovement[] = [];
    
    // Create movements based on materials with stock
    materials.forEach((material, index) => {
      if (material.totalStock && material.totalStock > 0) {
        movements.push({
          id: `mov-${String(index + 1).padStart(3, '0')}`,
          movementNumber: `MOV-2024-${String(index + 1).padStart(4, '0')}`,
          materialId: material.id!,
          materialCode: material.code,
          materialDescription: material.description,
          movementType: 'issue',
          quantity: Math.min(material.totalStock || 0, 10), // Use available stock or default to 10
          unit: material.unit,
          fromLocation: { type: 'warehouse', id: 'wh-001', name: 'Main Warehouse' },
          toLocation: { type: 'work-order', id: 'wo-001', name: 'Work Order' },
          relatedEntity: {
            type: 'work-order',
            id: 'wo-001',
            reference: 'WO-001'
          },
          performedBy: 'USR001',
          performedByName: 'System User',
          performedDate: new Date(Date.now() - (index * 2 * 60 * 60 * 1000)), // Staggered times
          notes: `Material issued for work order`
        });
      }
    });

    return movements.slice(0, 5); // Return only recent movements
  }

  /**
   * Generate alerts from centralized data
   */
  private generateAlertsFromCentralizedData(materials: BaseMaterial[]): StockAlert[] {
    const alerts: StockAlert[] = [];
    
    materials.forEach((material, index) => {
      const currentStock = material.totalStock || 0;
      const minimumStock = material.minimumStock || 10;
      
      if (currentStock < minimumStock) {
        alerts.push({
          id: `alert-${String(index + 1).padStart(3, '0')}`,
          type: currentStock === 0 ? 'out-of-stock' : 'low-stock',
          severity: currentStock === 0 ? 'critical' : 'high',
          materialId: material.id!,
          materialCode: material.code,
          materialDescription: material.description,
          currentStock,
          thresholdValue: minimumStock,
          message: currentStock === 0 ? 'Material completely out of stock' : 'Stock level below minimum threshold',
          actionRequired: currentStock === 0 ? 'URGENT: Immediate procurement needed' : 'Reorder required',
          dateDetected: new Date(Date.now() - (index * 24 * 60 * 60 * 1000)), // Staggered dates
          isActive: true
        });
      }
    });

    return alerts.slice(0, 5); // Return only top alerts
  }

  /**
   * Generate top used materials from centralized data
   */
  private generateTopUsedMaterialsFromCentralizedData(materials: BaseMaterial[]): MaterialUsageSummary[] {
    return materials
      .filter(material => material.totalStock && material.totalStock > 0)
      .slice(0, 5)
      .map((material, index) => ({
        materialId: material.id!,
        materialCode: material.code,
        materialDescription: material.description,
        totalUsed: material.totalStock || 0,
        totalCost: (material.totalStock || 0) * (material.averageCost || material.lastPurchaseCost || 0),
        usageFrequency: Math.floor(Math.random() * 20) + 5,
        lastUsedDate: new Date(Date.now() - (index * 24 * 60 * 60 * 1000))
      }));
  }

  // Private helper methods
  private saveAdjustmentAndMovement(
    adjustment: StockAdjustment, 
    movement: MaterialMovement, 
    material: BaseMaterial, 
    newStock: number
  ): Observable<boolean> {
    // In a real implementation, this would call the API
    adjustment.status = 'approved';
    
    const adjustments = this.adjustmentsSubject.value;
    this.adjustmentsSubject.next([adjustment, ...adjustments]);
    
    // Update material stock
    material.totalStock = newStock;
    material.availableStock = newStock - (material.reservedStock || 0);
    
    return of(true).pipe(delay(500));
  }

  private saveRequisition(requisition: MaterialRequisition): Observable<MaterialRequisition> {
    const requisitions = this.requisitionsSubject.value;
    this.requisitionsSubject.next([requisition, ...requisitions]);
    return of(requisition).pipe(delay(500));
  }

  private loadRequisitions(): void {
    // Mock data loading
    const mockRequisitions: MaterialRequisition[] = [];
    this.requisitionsSubject.next(mockRequisitions);
  }

  private loadAdjustments(): void {
    // Mock data loading
    const mockAdjustments: StockAdjustment[] = [];
    this.adjustmentsSubject.next(mockAdjustments);
  }

  private loadAlerts(): void {
    this.generateStockAlerts().subscribe();
  }

  private refreshData(): void {
    this.loadDashboardData().subscribe();
    this.generateStockAlerts().subscribe();
  }

  private setLoading(loading: boolean): void {
    this.loadingSubject.next(loading);
  }

  private setError(error: string | null): void {
    this.errorSubject.next(error);
    if (error) {
      setTimeout(() => this.errorSubject.next(null), 5000);
    }
  }

  private generateId(): string {
    return 'id_' + Math.random().toString(36).substr(2, 9);
  }

  private generateAdjustmentNumber(): string {
    const date = new Date();
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const random = Math.floor(Math.random() * 9999).toString().padStart(4, '0');
    return `ADJ-${year}${month}-${random}`;
  }

  private generateRequisitionNumber(): string {
    const date = new Date();
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const random = Math.floor(Math.random() * 9999).toString().padStart(4, '0');
    return `REQ-${year}${month}-${random}`;
  }

  private generateMovementNumber(): string {
    const date = new Date();
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const random = Math.floor(Math.random() * 9999).toString().padStart(4, '0');
    return `MOV-${year}${month}-${random}`;
  }
} 