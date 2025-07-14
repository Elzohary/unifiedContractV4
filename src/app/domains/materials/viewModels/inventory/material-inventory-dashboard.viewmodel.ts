import { Injectable } from '@angular/core';
import { BehaviorSubject, of, combineLatest, Observable } from 'rxjs';
import { tap, catchError, map } from 'rxjs/operators';
import { MaterialMovement } from '../../models/inventory.model';
import { MockDatabaseService } from '../../../../core/services/mock-database.service';
import { BaseMaterial } from '../../models/material.model';

export interface InventoryDashboardData {
  totalMaterials: number;
  totalValue: number;
  lowStockItems: number;
  expiringItems: number;
  pendingOrders: number;
  stockAlerts: any[];
  warehouseUtilization: any[];
  recentMovements: MaterialMovement[];
  materialHistoryTimeline: MaterialHistoryEvent[];
}

export interface MaterialHistoryEvent {
  id: string;
  date: Date;
  title: string;
  description: string;
  icon: string;
  color: string;
  type: string;
  user?: string;
  materialId: string;
  materialName: string;
  workOrderId?: string;
  workOrderNumber?: string;
  details: { [key: string]: string };
  documents?: string[];
  photos?: string[];
  quantity?: number;
  unit?: string;
  cost?: number;
}

export interface GroupedTimeline {
  workOrderId: string;
  workOrderNumber: string;
  events: MaterialHistoryEvent[];
}

export interface WarehouseUtilization {
  warehouseId: string;
  warehouseName: string;
  capacity: number;
  used: number;
  utilizationPercentage: number;
}

export interface StockAlert {
  id: string;
  type: 'low-stock' | 'expiring' | 'overstock' | 'no-movement';
  severity: 'low' | 'medium' | 'high' | 'critical';
  materialId: string;
  materialCode: string;
  materialDescription: string;
  message: string;
  actionRequired: string;
  dateDetected: Date;
}

@Injectable({
  providedIn: 'root'
})
export class MaterialInventoryDashboardViewModel {
  // State
  private dashboardDataSubject = new BehaviorSubject<InventoryDashboardData | null>(null);
  private loadingSubject = new BehaviorSubject<boolean>(false);
  private errorSubject = new BehaviorSubject<string | null>(null);

  // Filter state
  private selectedWorkOrderSubject = new BehaviorSubject<string | null>(null);
  private selectedMaterialSubject = new BehaviorSubject<string | null>(null);

  // Observables
  public dashboardData$ = this.dashboardDataSubject.asObservable();
  public loading$ = this.loadingSubject.asObservable();
  public error$ = this.errorSubject.asObservable();
  public selectedWorkOrder$ = this.selectedWorkOrderSubject.asObservable();
  public selectedMaterial$ = this.selectedMaterialSubject.asObservable();

  // Grouped and filtered timeline
  public groupedTimeline$ = combineLatest([
    this.dashboardData$,
    this.selectedWorkOrder$,
    this.selectedMaterial$
  ]).pipe(
    map(([dashboardData, selectedWorkOrder, selectedMaterial]) => {
      if (!dashboardData) return [];
      let events = dashboardData.materialHistoryTimeline;
      // Filter by work order
      if (selectedWorkOrder) {
        events = events.filter(e => e.workOrderId === selectedWorkOrder);
      }
      // Filter by material
      if (selectedMaterial) {
        events = events.filter(e => e.materialId === selectedMaterial);
      }
      // Group by work order
      const grouped: { [workOrderId: string]: GroupedTimeline } = {};
      for (const event of events) {
        if (!event.workOrderId) continue;
        if (!grouped[event.workOrderId]) {
          grouped[event.workOrderId] = {
            workOrderId: event.workOrderId,
            workOrderNumber: event.workOrderNumber || event.workOrderId,
            events: []
          };
        }
        grouped[event.workOrderId].events.push(event);
      }
      // Sort events in each group by date (descending)
      Object.values(grouped).forEach(group => {
        group.events.sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime());
      });
      // Sort groups by work order number
      return Object.values(grouped).sort((a, b) => a.workOrderNumber.localeCompare(b.workOrderNumber));
    })
  );

  constructor(private mockDb: MockDatabaseService) {
    this.initialize();
  }

  private initialize(): void {
    this.loadDashboardData().subscribe();
  }

  /**
   * Load dashboard data from centralized mock database
   */
  loadDashboardData(): Observable<InventoryDashboardData> {
    this.loadingSubject.next(true);

    return combineLatest([
      this.mockDb.materials$,
      this.mockDb.workOrders$,
      this.generateMaterialHistoryTimeline()
    ]).pipe(
      map(([materials, workOrders, materialHistoryTimeline]) => {
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

        // Generate stock alerts
        const stockAlerts = materials
          .filter((m: BaseMaterial) => (m.totalStock || 0) < (m.minimumStock || 10))
          .map((m: BaseMaterial) => ({
            materialCode: m.code || 'N/A',
            materialDescription: m.description,
            message: `Low stock alert: ${m.totalStock || 0} ${m.unit} remaining`,
            actionRequired: 'Replenish stock',
            severity: (m.totalStock || 0) === 0 ? 'critical' : 'high',
            type: 'low-stock'
          }));

        // Generate warehouse utilization data
        const warehouseUtilization = [
          {
            warehouseName: 'Main Warehouse',
            used: 1250,
            capacity: 2000,
            utilizationPercentage: 62.5
          },
          {
            warehouseName: 'Secondary Storage',
            used: 800,
            capacity: 1200,
            utilizationPercentage: 66.7
          }
        ];

        // Generate recent movements (simplified for now)
        const recentMovements: MaterialMovement[] = [
          {
            id: 'mov-001',
            movementNumber: 'MOV-001',
            materialId: 'mat-001',
            materialCode: 'MAT-001',
            materialDescription: 'Steel Beams',
            movementType: 'receipt',
            quantity: 50,
            unit: 'pieces',
            toLocation: { 
              type: 'warehouse',
              id: 'wh-001',
              name: 'Main Warehouse' 
            },
            relatedEntity: {
              type: 'work-order',
              id: 'wo-001',
              reference: 'WO-2024-001'
            },
            performedBy: 'user-001',
            performedByName: 'John Doe',
            performedDate: new Date()
          }
        ];

        const dashboardData: InventoryDashboardData = {
          totalMaterials,
          totalValue,
          lowStockItems,
          expiringItems: 0, // Mock value
          pendingOrders: pendingRequisitions,
          stockAlerts,
          warehouseUtilization,
          recentMovements,
          materialHistoryTimeline
        };

        this.dashboardDataSubject.next(dashboardData);
        this.loadingSubject.next(false);
        return dashboardData;
      }),
      catchError((error) => {
        console.error('Error loading dashboard data:', error);
        this.loadingSubject.next(false);
        return of({
          totalMaterials: 0,
          totalValue: 0,
          lowStockItems: 0,
          expiringItems: 0,
          pendingOrders: 0,
          stockAlerts: [],
          warehouseUtilization: [],
          recentMovements: [],
          materialHistoryTimeline: []
        });
      })
    );
  }

  /**
   * Refresh dashboard data
   */
  public refresh(): void {
    this.loadDashboardData();
  }

  // Setters for filters
  setSelectedWorkOrder(workOrderId: string | null) {
    this.selectedWorkOrderSubject.next(workOrderId);
  }
  setSelectedMaterial(materialId: string | null) {
    this.selectedMaterialSubject.next(materialId);
  }

  /**
   * Generate comprehensive material history timeline from all work orders
   */
  generateMaterialHistoryTimeline(): Observable<MaterialHistoryEvent[]> {
    return combineLatest([
      this.mockDb.materials$,
      this.mockDb.workOrders$
    ]).pipe(
      map(([materials, workOrders]) => {
        const timelineEvents: MaterialHistoryEvent[] = [];
        
        // Process each work order's material assignments
        workOrders.forEach(workOrder => {
          if (workOrder.materials && workOrder.materials.length > 0) {
            workOrder.materials.forEach(materialAssignment => {
              // Find material by name/description since materialAssignment doesn't have materialId
              const material = materials.find(m => 
                m.description === materialAssignment.purchasableMaterial?.description || 
                m.description === materialAssignment.receivableMaterial?.description
              );
              if (!material) return;
              
              // Assignment Event
              timelineEvents.push({
                id: `${materialAssignment.id}-assignment`,
                date: new Date(materialAssignment.assignDate),
                title: 'Material Assigned to Work Order',
                description: `${material.description} was assigned to work order ${materialAssignment.workOrderNumber} for ${materialAssignment.materialType === 'purchasable' ? 'purchase and installation' : 'client provision'}`,
                icon: 'assignment',
                color: 'primary',
                type: 'assignment',
                user: materialAssignment.assignedBy,
                materialId: material.id || '',
                materialName: material.description,
                workOrderId: workOrder.id,
                workOrderNumber: materialAssignment.workOrderNumber,
                details: {
                  'Work Order': materialAssignment.workOrderNumber,
                  'Assigned By': materialAssignment.assignedBy,
                  'Material Type': materialAssignment.materialType === 'purchasable' ? 'Purchasable (To be bought)' : 'Receivable (Client provided)',
                  'Purpose': 'Installation/Construction'
                }
              });
              
              // Process purchasable material events
              if (materialAssignment.purchasableMaterial) {
                const pm = materialAssignment.purchasableMaterial;
                
                // Order Event
                if (pm.orderDate) {
                  timelineEvents.push({
                    id: `${materialAssignment.id}-order`,
                    date: new Date(pm.orderDate),
                    title: 'Purchase Order Created',
                    description: `Order placed with ${pm.supplier || 'supplier'} for ${pm.quantity} ${pm.unit} of ${material.description}`,
                    icon: 'shopping_cart',
                    color: 'accent',
                    type: 'order',
                    materialId: material.id || '',
                    materialName: material.description,
                    workOrderId: workOrder.id,
                    workOrderNumber: materialAssignment.workOrderNumber,
                    quantity: pm.quantity,
                    unit: pm.unit,
                    cost: pm.totalCost,
                    details: {
                      'Supplier': pm.supplier || 'Not specified',
                      'Quantity Ordered': `${pm.quantity} ${pm.unit}`,
                      'Unit Cost': `SAR ${pm.unitCost}`,
                      'Total Cost': `SAR ${pm.totalCost}`,
                      'Expected Delivery': this.formatDate(pm.deliveryDate),
                      'Order Status': 'Confirmed'
                    }
                  });
                }
                
                // Delivery Event
                if (pm.delivery) {
                  const deliveryLocation = pm.delivery.storageLocation === 'site-direct' ? 'directly to site' : 'to warehouse';
                  timelineEvents.push({
                    id: `${materialAssignment.id}-delivery`,
                    date: new Date(pm.delivery.receivedDate),
                    title: `Material Delivered ${deliveryLocation}`,
                    description: `${pm.quantity} ${pm.unit} of ${material.description} received ${deliveryLocation} and checked by ${pm.delivery.receivedByName}`,
                    icon: 'local_shipping',
                    color: 'primary',
                    type: 'delivery',
                    user: pm.delivery.receivedByName,
                    materialId: material.id || '',
                    materialName: material.description,
                    workOrderId: workOrder.id,
                    workOrderNumber: materialAssignment.workOrderNumber,
                    quantity: pm.quantity,
                    unit: pm.unit,
                    details: {
                      'Delivery Location': pm.delivery.storageLocation === 'site-direct' ? 'Direct to Site' : 'Warehouse Storage',
                      'Received By': pm.delivery.receivedByName,
                      'Quantity Received': `${pm.quantity} ${pm.unit}`,
                      'Condition': 'Good condition',
                      'Invoice Status': pm.invoice ? 'Invoice uploaded' : 'Invoice pending',
                      'Delivery Notes': pm.delivery.deliveryNote || 'No special notes'
                    },
                    documents: pm.invoice ? ['invoice.pdf'] : [],
                    photos: pm.delivery.deliveryPhotos?.map(p => p.fileUrl) || []
                  });
                  
                  // Warehouse Storage Event (only if delivered to warehouse)
                  if (pm.delivery.warehouseDetails && pm.delivery.storageLocation !== 'site-direct') {
                    timelineEvents.push({
                      id: `${materialAssignment.id}-warehouse`,
                      date: new Date(pm.delivery.receivedDate),
                      title: 'Stored in Warehouse',
                      description: `Material stored at ${pm.delivery.warehouseDetails.warehouseName} warehouse in bin ${pm.delivery.warehouseDetails.binLocation || 'unspecified location'}`,
                      icon: 'warehouse',
                      color: 'primary',
                      type: 'warehouse',
                      materialId: material.id || '',
                      materialName: material.description,
                      workOrderId: workOrder.id,
                      workOrderNumber: materialAssignment.workOrderNumber,
                      quantity: pm.quantity,
                      unit: pm.unit,
                      details: {
                        'Warehouse Name': pm.delivery.warehouseDetails.warehouseName,
                        'Storage Location': pm.delivery.warehouseDetails.binLocation || 'General storage area',
                        'Quantity in Storage': `${pm.quantity} ${pm.unit}`,
                        'Storage Conditions': 'Standard conditions',
                        'Access': 'Available for site transfer'
                      }
                    });
                  }
                }
                
                // Site Issue Event (only if not delivered directly to site)
                if (pm.siteUsage?.issuedToSite && pm.delivery?.storageLocation !== 'site-direct') {
                  timelineEvents.push({
                    id: `${materialAssignment.id}-site-issue`,
                    date: new Date(pm.siteUsage.issuedDate || new Date()),
                    title: 'Transferred from Warehouse to Site',
                    description: `${pm.quantity} ${pm.unit} of material transferred from warehouse to construction site`,
                    icon: 'build_circle',
                    color: 'warn',
                    type: 'site-issue',
                    user: pm.siteUsage.receivedBySiteName,
                    materialId: material.id || '',
                    materialName: material.description,
                    workOrderId: workOrder.id,
                    workOrderNumber: materialAssignment.workOrderNumber,
                    quantity: pm.quantity,
                    unit: pm.unit,
                    details: {
                      'Warehouse Keeper': pm.siteUsage.releasedByName ? `${pm.siteUsage.releasedByName} (${pm.siteUsage.releasedBy})` : 'Not recorded',
                      'Site Receiver': pm.siteUsage.receivedBySiteName || 'Not recorded',
                      'Quantity Transferred': `${pm.quantity} ${pm.unit}`,
                      'Transfer Purpose': 'Installation/Construction',
                      'Transfer Method': 'Company vehicle',
                      'Site Location': 'Main construction area'
                    },
                    photos: []
                  });
                }
                
                // Add individual usage records if available
                if (pm.siteUsageRecords && pm.siteUsageRecords.length > 0) {
                  pm.siteUsageRecords.forEach((record, index) => {
                    if (record.recordType === 'site-issue') {
                      timelineEvents.push({
                        id: `${materialAssignment.id}-site-issue-${index}`,
                        date: new Date(record.recordDate),
                        title: 'Transferred to Site',
                        description: `Material released by ${record.releasedByName} from warehouse and received by ${record.receivedBySiteName} at site`,
                        icon: 'build_circle',
                        color: 'warn',
                        type: 'site-issue',
                        user: record.receivedBySiteName,
                        materialId: material.id || '',
                        materialName: material.description,
                        workOrderId: workOrder.id,
                        workOrderNumber: materialAssignment.workOrderNumber,
                        quantity: pm.quantity,
                        unit: pm.unit,
                        details: {
                          'Released By (Warehouse)': `${record.releasedByName ?? 'Unknown'} (${record.releasedBy ?? 'Unknown'})`,
                          'Received By (Site)': `${record.receivedBySiteName ?? 'Unknown'} (${record.receivedBySite ?? 'Unknown'})`,
                          'Issue Date': this.formatDate(record.issuedDate),
                          'System Record By': record.recordedByName ?? 'Unknown',
                          'Transfer Status': 'Completed'
                        },
                        photos: record.photos?.map(p => p.fileUrl) || []
                      });
                    } else if (record.recordType === 'usage-update') {
                      timelineEvents.push({
                        id: `${materialAssignment.id}-usage-${index}`,
                        date: new Date(record.recordDate),
                        title: `Material Usage Update`,
                        description: `Used ${record.quantityUsed} ${pm.unit} (${record.remainingQuantity} ${pm.unit} remaining)`,
                        icon: 'construction',
                        color: 'primary',
                        type: 'usage',
                        user: record.recordedByName,
                        materialId: material.id || '',
                        materialName: material.description,
                        workOrderId: workOrder.id,
                        workOrderNumber: materialAssignment.workOrderNumber,
                        quantity: record.quantityUsed,
                        unit: pm.unit,
                        details: {
                          'Quantity Used (This Update)': `${record.quantityUsed} ${pm.unit}`,
                          'Total Used So Far': `${record.cumulativeQuantityUsed} ${pm.unit}`,
                          'Remaining Quantity': `${record.remainingQuantity} ${pm.unit}`,
                          'Usage Percentage': `${record.usagePercentage}%`,
                          'Usage Notes': record.usageNotes || 'No notes provided',
                          'Recorded By': record.recordedByName ?? 'Unknown',
                          'Record Date': this.formatDate(record.recordDate)
                        },
                        photos: record.photos?.map(p => p.fileUrl) || []
                      });
                    } else if (record.recordType === 'return') {
                      timelineEvents.push({
                        id: `${materialAssignment.id}-return-${index}`,
                        date: new Date(record.recordDate),
                        title: 'Material Returned',
                        description: `${record.quantityReturned} ${pm.unit} returned to warehouse`,
                        icon: 'undo',
                        color: 'accent',
                        type: 'usage',
                        user: record.recordedByName ?? 'Unknown',
                        materialId: material.id || '',
                        materialName: material.description,
                        workOrderId: workOrder.id,
                        workOrderNumber: materialAssignment.workOrderNumber,
                        quantity: record.quantityReturned,
                        unit: pm.unit,
                        details: {
                          'Quantity Returned': `${record.quantityReturned} ${pm.unit}`,
                          'Recorded By': record.recordedByName ?? 'Unknown',
                          'Notes': record.usageNotes || 'No notes'
                        },
                        photos: record.photos?.map(p => p.fileUrl) || []
                      });
                    } else if (record.recordType === 'waste') {
                      timelineEvents.push({
                        id: `${materialAssignment.id}-waste-${index}`,
                        date: new Date(record.recordDate),
                        title: 'Material Waste Recorded',
                        description: `${record.quantityWasted} ${pm.unit} recorded as waste`,
                        icon: 'delete_sweep',
                        color: 'warn',
                        type: 'usage',
                        user: record.recordedByName ?? 'Unknown',
                        materialId: material.id || '',
                        materialName: material.description,
                        workOrderId: workOrder.id,
                        workOrderNumber: materialAssignment.workOrderNumber,
                        quantity: record.quantityWasted,
                        unit: pm.unit,
                        details: {
                          'Quantity Wasted': `${record.quantityWasted} ${pm.unit}`,
                          'Waste Reason': record.wasteReason || 'Not specified',
                          'Recorded By': record.recordedByName ?? 'Unknown'
                        },
                        photos: record.photos?.map(p => p.fileUrl) || []
                      });
                    }
                  });
                }
                
                // Legacy single usage event (for backward compatibility)
                if (pm.siteUsage?.actualQuantityUsed !== undefined) {
                  const wasteAmount = pm.quantity - (pm.siteUsage.actualQuantityUsed || 0);
                  timelineEvents.push({
                    id: `${materialAssignment.id}-completion`,
                    date: new Date(pm.siteUsage.usageCompletedDate || new Date()),
                    title: 'Material Installation Completed',
                    description: `Installation completed with ${pm.siteUsage.actualQuantityUsed} ${pm.unit} used (${pm.siteUsage.usagePercentage}% of total)`,
                    icon: 'check_circle',
                    color: 'success',
                    type: 'usage',
                    materialId: material.id || '',
                    materialName: material.description,
                    workOrderId: workOrder.id,
                    workOrderNumber: materialAssignment.workOrderNumber,
                    quantity: pm.siteUsage.actualQuantityUsed,
                    unit: pm.unit,
                    details: {
                      'Quantity Used': `${pm.siteUsage.actualQuantityUsed} ${pm.unit}`,
                      'Usage Percentage': `${pm.siteUsage.usagePercentage}%`,
                      'Waste/Leftover': wasteAmount > 0 ? `${wasteAmount} ${pm.unit}` : 'None',
                      'Installation Quality': 'Verified and approved',
                      'Usage Notes': pm.siteUsage.usageNotes || 'Installation completed as planned',
                      'Completion Date': this.formatDate(pm.siteUsage.usageCompletedDate)
                    },
                    photos: pm.siteUsage.usagePhotos?.map(p => p.fileUrl) || []
                  });
                }
              }
              
              // Process receivable material events
              if (materialAssignment.receivableMaterial) {
                const rm = materialAssignment.receivableMaterial;
                
                // Received Event
                if (rm.receivedDate) {
                  timelineEvents.push({
                    id: `${materialAssignment.id}-received`,
                    date: new Date(rm.receivedDate),
                    title: 'Client Material Received',
                    description: `${rm.receivedQuantity || rm.estimatedQuantity} ${rm.unit} of ${material.description} received from client and verified`,
                    icon: 'inventory',
                    color: 'primary',
                    type: 'delivery',
                    user: rm.receivedBy?.name ?? 'Unknown',
                    materialId: material.id || '',
                    materialName: material.description,
                    workOrderId: workOrder.id,
                    workOrderNumber: materialAssignment.workOrderNumber,
                    quantity: rm.receivedQuantity || rm.estimatedQuantity,
                    unit: rm.unit,
                    details: {
                      'Received From': 'Client (SEC/Other)',
                      'Received By': rm.receivedBy?.name || 'Not specified',
                      'Employee Badge': rm.receivedBy?.badgeNumber || 'N/A',
                      'Expected Quantity': `${rm.estimatedQuantity} ${rm.unit}`,
                      'Actual Received': `${rm.receivedQuantity || 0} ${rm.unit}`,
                      'Difference': rm.receivedQuantity !== rm.estimatedQuantity ? 
                        `${Math.abs((rm.receivedQuantity || 0) - rm.estimatedQuantity)} ${rm.unit} ${(rm.receivedQuantity || 0) > rm.estimatedQuantity ? 'extra' : 'short'}` : 
                        'Exact match',
                      'Material Condition': 'Good condition'
                    }
                  });
                }
                
                // Add individual usage records if available
                if (rm.usageRecords && rm.usageRecords.length > 0) {
                  rm.usageRecords.forEach((record, index) => {
                    if (record.recordType === 'usage-update') {
                      timelineEvents.push({
                        id: `${materialAssignment.id}-client-usage-${index}`,
                        date: new Date(record.recordDate),
                        title: `Material Usage Update`,
                        description: `Used ${record.quantityUsed} ${rm.unit} (${record.remainingQuantity} ${rm.unit} remaining)`,
                        icon: 'construction',
                        color: 'primary',
                        type: 'usage',
                        user: record.recordedByName ?? 'Unknown',
                        materialId: material.id || '',
                        materialName: material.description,
                        workOrderId: workOrder.id,
                        workOrderNumber: materialAssignment.workOrderNumber,
                        quantity: record.quantityUsed,
                        unit: rm.unit,
                        details: {
                          'Quantity Used (This Update)': `${record.quantityUsed} ${rm.unit}`,
                          'Total Used So Far': `${record.cumulativeQuantityUsed} ${rm.unit}`,
                          'Remaining Quantity': `${record.remainingQuantity} ${rm.unit}`,
                          'Usage Percentage': `${record.usagePercentage}%`,
                          'Usage Notes': record.usageNotes || 'No notes provided',
                          'Recorded By': record.recordedByName ?? 'Unknown',
                          'Record Date': this.formatDate(record.recordDate)
                        },
                        photos: record.photos?.map(p => p.fileUrl) || []
                      });
                    } else if (record.recordType === 'return-to-client') {
                      timelineEvents.push({
                        id: `${materialAssignment.id}-client-return-${index}`,
                        date: new Date(record.recordDate),
                        title: 'Material Returned to Client',
                        description: `${record.quantityReturned} ${rm.unit} returned to client as extra material`,
                        icon: 'assignment_return',
                        color: 'accent',
                        type: 'usage',
                        user: record.recordedByName ?? 'Unknown',
                        materialId: material.id || '',
                        materialName: material.description,
                        workOrderId: workOrder.id,
                        workOrderNumber: materialAssignment.workOrderNumber,
                        quantity: record.quantityReturned,
                        unit: rm.unit,
                        details: {
                          'Quantity Returned': `${record.quantityReturned} ${rm.unit}`,
                          'Return Reason': record.returnReason || 'Extra material not needed',
                          'Recorded By': record.recordedByName ?? 'Unknown',
                          'Return Date': this.formatDate(record.recordDate)
                        },
                        photos: record.photos?.map(p => p.fileUrl) || []
                      });
                    } else if (record.recordType === 'reserve-for-later') {
                      timelineEvents.push({
                        id: `${materialAssignment.id}-reserve-${index}`,
                        date: new Date(record.recordDate),
                        title: 'Material Reserved for Later Use',
                        description: `${record.remainingQuantity} ${rm.unit} reserved for ${record.reservedForWorkOrder ? 'this work order' : 'future use'}`,
                        icon: 'schedule',
                        color: 'primary',
                        type: 'usage',
                        user: record.recordedByName ?? 'Unknown',
                        materialId: material.id || '',
                        materialName: material.description,
                        workOrderId: workOrder.id,
                        workOrderNumber: materialAssignment.workOrderNumber,
                        quantity: record.remainingQuantity,
                        unit: rm.unit,
                        details: {
                          'Quantity Reserved': `${record.remainingQuantity} ${rm.unit}`,
                          'Reservation': record.reservedForWorkOrder ? 'Reserved for this work order only' : 'Available for any work order',
                          'Notes': record.reservationNotes || 'No notes',
                          'Recorded By': record.recordedByName ?? 'Unknown'
                        },
                        photos: record.photos?.map(p => p.fileUrl) || []
                      });
                    }
                  });
                }
                
                // Legacy installation event (for backward compatibility)
                if (rm.actualQuantity !== undefined) {
                  timelineEvents.push({
                    id: `${materialAssignment.id}-client-completion`,
                    date: new Date(),
                    title: 'Client Material Installed',
                    description: `${rm.actualQuantity || rm.estimatedQuantity} ${rm.unit} of client-provided material successfully installed`,
                    icon: 'construction',
                    color: 'success',
                    type: 'usage',
                    materialId: material.id || '',
                    materialName: material.description,
                    workOrderId: workOrder.id,
                    workOrderNumber: materialAssignment.workOrderNumber,
                    quantity: rm.actualQuantity || rm.estimatedQuantity,
                    unit: rm.unit,
                    details: {
                      'Quantity Installed': `${rm.actualQuantity || rm.estimatedQuantity} ${rm.unit}`,
                      'Installation Status': 'Completed and verified',
                      'Quality Check': 'Passed',
                      'Client Approval': 'Pending'
                    }
                  });
                }
              }
            });
          }
        });
        
        // Sort events by date (most recent first)
        timelineEvents.sort((a, b) => {
          const dateA = new Date(a.date).getTime();
          const dateB = new Date(b.date).getTime();
          return dateB - dateA;
        });
        
        return timelineEvents;
      })
    );
  }

  private formatDate(date: string | Date | undefined): string {
    if (!date) return '-';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }
} 