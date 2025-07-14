import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { map, switchMap, concatMap } from 'rxjs/operators';
import { v4 as uuidv4 } from 'uuid';
import { MaterialReallocation, MaterialReallocationStatus, MaterialReallocationAudit } from '../models/material-reallocation.model';
import { MockDatabaseService } from '../../../core/services/mock-database.service';
import { WorkOrderService } from '../../work-order/services/work-order.service';
import { environment } from '../../../../environments/environment';

// Interface for work order allocation data
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

// Interface for reallocation actions
export interface ReallocationAction {
  fromWorkOrderId?: string;
  toWorkOrderId?: string;
  quantity: number;
  reason: string;
  priority: 'low' | 'medium' | 'high' | 'critical';
}

@Injectable({ providedIn: 'root' })
export class MaterialReallocationService {
  constructor(
    private mockDb: MockDatabaseService
  ) {}

  /**
   * Get work order allocations for a material
   */
  getWorkOrderAllocations(materialId: string): Observable<WorkOrderAllocation[]> {
    return this.mockDb.getWorkOrders().pipe(
      map((workOrders: any[]) => {
        return workOrders
          .filter((wo: any) => {
            const materials = wo.materials || [];
            return materials.some((m: any) => 
              (m.purchasableMaterial?.id === materialId) || 
              (m.receivableMaterial?.id === materialId)
            );
          })
          .map((wo: any) => {
            const material = (wo.materials || []).find((m: any) => 
              (m.purchasableMaterial?.id === materialId) || 
              (m.receivableMaterial?.id === materialId)
            );
            
            const allocatedQty = material?.purchasableMaterial?.quantity || 
                               material?.receivableMaterial?.estimatedQuantity || 0;
            const usedQty = material?.purchasableMaterial?.usedQuantity || 
                          material?.receivableMaterial?.usedQuantity || 0;
            const remainingQty = allocatedQty - usedQty;
            
            return {
              workOrderId: wo.id,
              workOrderNumber: wo.workOrderNumber,
              workOrderTitle: wo.title,
              status: wo.status,
              completionPercentage: wo.completionPercentage || 0,
              allocatedQuantity: allocatedQty,
              usedQuantity: usedQty,
              remainingQuantity: remainingQty,
              canReduce: remainingQty > 0,
              canIncrease: true, // Could be limited by available inventory
              priority: wo.priority || 'medium'
            };
          });
      })
    );
  }

  /**
   * Apply multiple reallocation actions
   */
  applyReallocations(materialId: string, actions: ReallocationAction[]): Observable<any> {
    if (actions.length === 0) {
      return of({ success: true, message: 'No actions to apply' });
    }

    // Process actions sequentially to maintain data consistency
    return of(...actions).pipe(
      concatMap(action => {
        if (action.fromWorkOrderId && action.toWorkOrderId) {
          // Transfer between work orders
          return this.requestReallocation(
            materialId,
            action.fromWorkOrderId,
            action.toWorkOrderId,
            action.quantity,
            action.reason,
            'system' // Could be current user
          ).pipe(
            switchMap(reallocation => 
              this.approveReallocation(reallocation.id, true, 'system')
            )
          );
        } else if (action.fromWorkOrderId) {
          // Reduce allocation (return to inventory)
          return this.requestReallocation(
            materialId,
            action.fromWorkOrderId,
            'inventory', // Special work order ID for inventory
            action.quantity,
            action.reason,
            'system'
          ).pipe(
            switchMap(reallocation => 
              this.approveReallocation(reallocation.id, true, 'system')
            )
          );
        } else if (action.toWorkOrderId) {
          // Increase allocation (from inventory)
          return this.requestReallocation(
            materialId,
            'inventory', // Special work order ID for inventory
            action.toWorkOrderId,
            action.quantity,
            action.reason,
            'system'
          ).pipe(
            switchMap(reallocation => 
              this.approveReallocation(reallocation.id, true, 'system')
            )
          );
        }
        return of(null);
      }),
      map(() => ({ success: true, message: 'Reallocations applied successfully' }))
    );
  }

  /**
   * Request a new material reallocation (pending approval)
   */
  requestReallocation(
    materialId: string,
    fromWorkOrderId: string,
    toWorkOrderId: string,
    quantity: number,
    reason: string,
    requestedBy: string
  ): Observable<MaterialReallocation> {
    // Validation
    if (!materialId || !fromWorkOrderId || !toWorkOrderId || fromWorkOrderId === toWorkOrderId) {
      return throwError(() => new Error('Invalid work order selection.'));
    }
    if (quantity <= 0) {
      return throwError(() => new Error('Quantity must be greater than zero.'));
    }
    // Check available quantity in source
    return this.mockDb.getWorkOrderById(fromWorkOrderId).pipe(
      switchMap(wo => {
        if (!wo) return throwError(() => new Error('Source work order not found.'));
        const assignment = (wo.materials || []).find(m => (m.purchasableMaterial?.id || m.receivableMaterial?.id) === materialId);
        const available = assignment?.purchasableMaterial?.quantity || assignment?.receivableMaterial?.estimatedQuantity || 0;
        if (available < quantity) {
          return throwError(() => new Error('Insufficient quantity in source work order.'));
        }
        // Create reallocation event
        const now = new Date();
        const reallocation: MaterialReallocation = {
          id: uuidv4(),
          materialId,
          fromWorkOrderId,
          toWorkOrderId,
          quantity,
          reason,
          requestedBy,
          requestedAt: now,
          status: 'pending',
          auditTrail: [
            {
              id: uuidv4(),
              action: 'requested',
              performedBy: requestedBy,
              performedAt: now,
              notes: reason
            }
          ]
        };
        return this.mockDb.createMaterialReallocation(reallocation);
      })
    );
  }

  /**
   * Approve or reject a reallocation (transactional update)
   */
  approveReallocation(
    reallocationId: string,
    approved: boolean,
    approver: string
  ): Observable<MaterialReallocation> {
    return this.mockDb.getMaterialReallocationById(reallocationId).pipe(
      switchMap(reallocation => {
        if (!reallocation) return throwError(() => new Error('Reallocation not found.'));
        if (reallocation.status !== 'pending') return throwError(() => new Error('Reallocation already processed.'));
        const now = new Date();
        if (!approved) {
          reallocation.status = 'rejected';
          reallocation.rejectedBy = approver;
          reallocation.rejectedAt = now;
          reallocation.auditTrail.push({
            id: uuidv4(),
            action: 'rejected',
            performedBy: approver,
            performedAt: now
          });
          return this.mockDb.updateMaterialReallocation(reallocationId, reallocation).pipe(
            map(() => reallocation)
          );
        }
        // Transactional update: reduce from source, add to destination
        let fromOriginal: any = null;
        let toOriginal: any = null;
        return this.mockDb.getWorkOrderById(reallocation.fromWorkOrderId).pipe(
          switchMap(fromWO => {
            if (!fromWO) return throwError(() => new Error('Source work order not found.'));
            const fromAssignment = (fromWO.materials || []).find(m => (m.purchasableMaterial?.id || m.receivableMaterial?.id) === reallocation.materialId);
            if (!fromAssignment) return throwError(() => new Error('Material not found in source work order.'));
            const fromQty = fromAssignment.purchasableMaterial?.quantity || fromAssignment.receivableMaterial?.estimatedQuantity || 0;
            if (fromQty < reallocation.quantity) return throwError(() => new Error('Insufficient quantity in source.'));
            fromOriginal = JSON.parse(JSON.stringify(fromAssignment)); // Deep copy
            // Reduce from source
            if (fromAssignment.purchasableMaterial) fromAssignment.purchasableMaterial.quantity = fromQty - reallocation.quantity;
            if (fromAssignment.receivableMaterial) fromAssignment.receivableMaterial.estimatedQuantity = fromQty - reallocation.quantity;
            
            // Update the work order with the modified material assignment
            const updatedFromWO = {
              ...fromWO,
              materials: (fromWO.materials || []).map(m => 
                (m.purchasableMaterial?.id || m.receivableMaterial?.id) === reallocation.materialId ? fromAssignment : m
              )
            };
            
            return this.mockDb.updateWorkOrder(fromWO.id, updatedFromWO).pipe(
              switchMap(() =>
                this.mockDb.getWorkOrderById(reallocation.toWorkOrderId).pipe(
                  switchMap(toWO => {
                    if (!toWO) {
                      // Rollback source
                      reallocation.status = 'pending';
                      reallocation.errorMessage = 'Destination work order not found. Rolled back.';
                      reallocation.auditTrail.push({
                        id: uuidv4(),
                        action: 'rollback',
                        performedBy: approver,
                        performedAt: now,
                        notes: reallocation.errorMessage
                      });
                      const rollbackFromWO = {
                        ...fromWO,
                        materials: (fromWO.materials || []).map(m => 
                          (m.purchasableMaterial?.id || m.receivableMaterial?.id) === reallocation.materialId ? fromOriginal : m
                        )
                      };
                      return this.mockDb.updateWorkOrder(fromWO.id, rollbackFromWO).pipe(
                        concatMap(() => this.mockDb.updateMaterialReallocation(reallocationId, reallocation)),
                        map(() => reallocation)
                      );
                    }
                    const toAssignment = (toWO.materials || []).find(m => (m.purchasableMaterial?.id || m.receivableMaterial?.id) === reallocation.materialId);
                    if (!toAssignment) {
                      // Rollback source
                      reallocation.status = 'pending';
                      reallocation.errorMessage = 'Material not found in destination work order. Rolled back.';
                      reallocation.auditTrail.push({
                        id: uuidv4(),
                        action: 'rollback',
                        performedBy: approver,
                        performedAt: now,
                        notes: reallocation.errorMessage
                      });
                      const rollbackFromWO = {
                        ...fromWO,
                        materials: (fromWO.materials || []).map(m => 
                          (m.purchasableMaterial?.id || m.receivableMaterial?.id) === reallocation.materialId ? fromOriginal : m
                        )
                      };
                      return this.mockDb.updateWorkOrder(fromWO.id, rollbackFromWO).pipe(
                        concatMap(() => this.mockDb.updateMaterialReallocation(reallocationId, reallocation)),
                        map(() => reallocation)
                      );
                    }
                    const toQty = toAssignment.purchasableMaterial?.quantity || toAssignment.receivableMaterial?.estimatedQuantity || 0;
                    toOriginal = JSON.parse(JSON.stringify(toAssignment)); // Deep copy
                    // Add to destination
                    if (toAssignment.purchasableMaterial) toAssignment.purchasableMaterial.quantity = toQty + reallocation.quantity;
                    if (toAssignment.receivableMaterial) toAssignment.receivableMaterial.estimatedQuantity = toQty + reallocation.quantity;
                    
                    // Update the destination work order
                    const updatedToWO = {
                      ...toWO,
                      materials: (toWO.materials || []).map(m => 
                        (m.purchasableMaterial?.id || m.receivableMaterial?.id) === reallocation.materialId ? toAssignment : m
                      )
                    };
                    
                    return this.mockDb.updateWorkOrder(toWO.id, updatedToWO).pipe(
                      switchMap(() => {
                        reallocation.status = 'approved';
                        reallocation.approvedBy = approver;
                        reallocation.approvedAt = now;
                        reallocation.completedAt = now;
                        reallocation.errorMessage = undefined;
                        reallocation.auditTrail.push({
                          id: uuidv4(),
                          action: 'approved',
                          performedBy: approver,
                          performedAt: now
                        });
                        reallocation.auditTrail.push({
                          id: uuidv4(),
                          action: 'completed',
                          performedBy: approver,
                          performedAt: now
                        });
                        return this.mockDb.updateMaterialReallocation(reallocationId, reallocation).pipe(
                          map(() => reallocation)
                        );
                      }),
                      // On error, rollback source
                      (err) => {
                        reallocation.status = 'pending';
                        reallocation.errorMessage = 'Failed to update destination. Rolled back.';
                        reallocation.auditTrail.push({
                          id: uuidv4(),
                          action: 'rollback',
                          performedBy: approver,
                          performedAt: now,
                          notes: reallocation.errorMessage
                        });
                        const rollbackFromWO = {
                          ...fromWO,
                          materials: (fromWO.materials || []).map(m => 
                            (m.purchasableMaterial?.id || m.receivableMaterial?.id) === reallocation.materialId ? fromOriginal : m
                          )
                        };
                        return this.mockDb.updateWorkOrder(fromWO.id, rollbackFromWO).pipe(
                          concatMap(() => this.mockDb.updateMaterialReallocation(reallocationId, reallocation)),
                          map(() => reallocation)
                        );
                      }
                    );
                  })
                )
              )
            );
          })
        );
      })
    );
  }

  /**
   * Get reallocation history for a material or work order
   */
  getReallocationHistory(materialId?: string, workOrderId?: string): Observable<MaterialReallocation[]> {
    return this.mockDb.getMaterialReallocations().pipe(
      map(list => list.filter(r =>
        (!materialId || r.materialId === materialId) &&
        (!workOrderId || r.fromWorkOrderId === workOrderId || r.toWorkOrderId === workOrderId)
      ))
    );
  }
} 