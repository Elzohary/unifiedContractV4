import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { delay, map, catchError, switchMap } from 'rxjs/operators';
import { WorkOrder, WorkOrderPriority, WorkOrderIssue, materialAssignment } from '../models/work-order.model';
import { Task } from '../models/work-order.model';
import { WorkOrderStatus } from '../models/work-order-status.enum';
import { ApiService } from '../../../core/services/api.service';
import { StateService } from '../../../core/services/state.service';
import { ActivityLogService } from '../../../shared/services/activity-log.service';
import { MockDatabaseService } from '../../../core/services/mock-database.service';
import { environment } from '../../../../environments/environment';

export interface WorkOrderStatusResponse {
  id: string;
  name: string;
  code: string;
}

interface StatusTransitionHistory {
  id: string;
  workOrderId: string;
  fromStatus: WorkOrderStatus;
  toStatus: WorkOrderStatus;
  changedBy: string;
  changedDate: string;
  reason?: string;
}

interface RemarkData {
  content: string;
  createdBy?: string;
  type?: 'general' | 'technical' | 'safety' | 'quality';
  peopleInvolved?: string[];
}

@Injectable({
  providedIn: 'root'
})
export class WorkOrderService {
  private endpoint = 'work-orders';
  private statusEndpoint = 'work-order-statuses';
  private statusesCache$ = new BehaviorSubject<WorkOrderStatusResponse[]>([]);

  // Status transition rules - simplified for basic workflow
  private readonly allowedStatusTransitions: Partial<Record<WorkOrderStatus, WorkOrderStatus[]>> = {
    [WorkOrderStatus.Pending]: [WorkOrderStatus.InProgress, WorkOrderStatus.Cancelled],
    [WorkOrderStatus.InProgress]: [WorkOrderStatus.OnHold, WorkOrderStatus.Completed, WorkOrderStatus.Cancelled],
    [WorkOrderStatus.OnHold]: [WorkOrderStatus.InProgress, WorkOrderStatus.Cancelled],
    [WorkOrderStatus.Completed]: [WorkOrderStatus.InProgress],
    [WorkOrderStatus.Cancelled]: [WorkOrderStatus.Pending],
    [WorkOrderStatus.ClosedWithMustakhlasNeed1stApprovalNeedReturnScSrap]: [],
  };

  // Subjects for reactive updates
  private workOrdersSubject = new BehaviorSubject<WorkOrder[]>([]);
  workOrders$ = this.workOrdersSubject.asObservable();

  private newWorkOrderSubject = new Subject<WorkOrder>();
  newWorkOrder$ = this.newWorkOrderSubject.asObservable();

  private simulateNetwork<T>(data: T): Observable<T> {
    return of(data).pipe(delay(environment.mockDataDelay));
  }

  constructor(
    private apiService: ApiService,
    private stateService: StateService,
    private activityLogService: ActivityLogService,
    private mockDatabaseService: MockDatabaseService
  ) {
    this.loadStatuses();
    this.loadWorkOrders();
  }

  private loadStatuses(): void {
    // Initialize with default statuses
    const defaultStatuses: WorkOrderStatusResponse[] = [
      { id: '1', name: 'Draft', code: 'draft' },
      { id: '2', name: 'In Progress', code: 'in-progress' },
      { id: '3', name: 'On Hold', code: 'on-hold' },
      { id: '4', name: 'Completed', code: 'completed' },
      { id: '5', name: 'Closed', code: 'closed' },
      { id: '6', name: 'Cancelled', code: 'cancelled' }
    ];
    this.statusesCache$.next(defaultStatuses);
  }

  private loadWorkOrders(): void {
    // Subscribe to the centralized mock database
    this.mockDatabaseService.workOrders$.subscribe(workOrders => {
      this.workOrdersSubject.next(workOrders);
    });
  }

  getStatuses(): Observable<WorkOrderStatusResponse[]> {
    return this.statusesCache$.asObservable();
  }

  isValidStatus(status: string): boolean {
    return Object.values(WorkOrderStatus).includes(status as WorkOrderStatus);
  }

  getStatusDisplayName(status: WorkOrderStatus): string {
    const statusMap: Partial<Record<WorkOrderStatus, string>> = {
      [WorkOrderStatus.Pending]: 'Pending',
      [WorkOrderStatus.InProgress]: 'In Progress',
      [WorkOrderStatus.OnHold]: 'On Hold',
      [WorkOrderStatus.Completed]: 'Completed',
      [WorkOrderStatus.Cancelled]: 'Cancelled',
      [WorkOrderStatus.ClosedWithMustakhlasNeed1stApprovalNeedReturnScSrap]: 'Closed with Mustakhlas'
    };
    return statusMap[status] || status;
  }

  canTransitionTo(currentStatus: WorkOrderStatus, newStatus: WorkOrderStatus): boolean {
    return this.allowedStatusTransitions[currentStatus]?.includes(newStatus) || false;
  }

  updateWorkOrderStatus(id: string, newStatus: WorkOrderStatus, reason?: string): Observable<WorkOrder> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrderById(id).pipe(
        switchMap(workOrder => {
          if (!workOrder) {
            throw new Error(`Work order with id ${id} not found`);
          }
          
          const currentStatus = workOrder.details.status;
          if (!this.canTransitionTo(currentStatus, newStatus)) {
            throw new Error(`Invalid status transition from ${currentStatus} to ${newStatus}`);
          }

          const updatedWorkOrder = {
            ...workOrder,
            details: {
              ...workOrder.details,
              status: newStatus
            }
          };

          return this.mockDatabaseService.updateWorkOrder(id, updatedWorkOrder);
        }),
        catchError(error => {
          console.error('Error updating work order status:', error);
          throw error;
        })
      );
    } else {
      // Real API call - extract data from ApiResponse
      return this.apiService.put<WorkOrder>(`${this.endpoint}/${id}/status`, { status: newStatus, reason }).pipe(
        map(response => response.data)
      );
    }
  }

  getStatusHistory(workOrderId: string): Observable<StatusTransitionHistory[]> {
    // Mock implementation - in real app, this would come from the backend
    return of([]).pipe(delay(environment.mockDataDelay));
  }

  getAllWorkOrders(): Observable<WorkOrder[]> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrders();
    } else {
      return this.apiService.get<any[]>(this.endpoint).pipe(
        map(response => response.data.map(wo => ({
          ...wo,
          details: {
            workOrderNumber: wo.workOrderNumber,
            internalOrderNumber: wo.internalOrderNumber,
            title: wo.title,
            description: wo.description,
            client: wo.client || '',
            location: wo.location,
            status: wo.statusCode ? wo.statusCode.toLowerCase() : '',
            priority: wo.priorityCode ? wo.priorityCode.toLowerCase() : '',
            category: wo.category,
            completionPercentage: wo.completionPercentage,
            receivedDate: wo.receivedDate,
            startDate: wo.startDate,
            dueDate: wo.dueDate,
            targetEndDate: wo.targetEndDate,
            createdDate: wo.createdAt,
            createdBy: wo.createdBy,
            lastUpdated: wo.lastModifiedAt,
            estimatedPrice: wo.estimatedCost
          }
        })))
      );
    }
  }

  getWorkOrderById(id: string): Observable<WorkOrder> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrderById(id).pipe(
        map(workOrder => {
          if (!workOrder) {
            throw new Error(`Work order with id ${id} not found`);
          }
          return workOrder;
        })
      );
    } else {
      return this.apiService.get<any>(`${this.endpoint}/${id}`).pipe(
        map(response => {
          const wo = response.data;
          return {
            ...wo,
            details: {
              workOrderNumber: wo.workOrderNumber,
              internalOrderNumber: wo.internalOrderNumber,
              title: wo.title,
              description: wo.description,
              client: wo.client || '',
              location: wo.location,
              status: wo.status || '',
              priority: wo.priority || '',
              category: wo.category,
              type: wo.type, // <-- Add this line
              class: wo.class, // <-- Add this line
              completionPercentage: wo.completionPercentage,
              receivedDate: wo.receivedDate,
              startDate: wo.startDate,
              dueDate: wo.dueDate,
              targetEndDate: wo.targetEndDate,
              createdDate: wo.createdAt,
              createdBy: wo.createdBy,
              lastUpdated: wo.lastModifiedAt,
              estimatedPrice: wo.estimatedCost,
              permits: wo.permits
            }
          };
        })
      );
    }
  }

  createWorkOrder(workOrderData: Partial<WorkOrder>): Observable<WorkOrder> {
    if (environment.useMockData) {
      // Create a new work order with proper structure
      const newWorkOrder: Partial<WorkOrder> = {
        ...workOrderData,
        details: {
          workOrderNumber: workOrderData.details?.workOrderNumber || `WO-${new Date().getFullYear()}-${Date.now()}`,
          internalOrderNumber: workOrderData.details?.internalOrderNumber || `INT-${new Date().getFullYear()}-${Date.now()}`,
          title: workOrderData.details?.title || '',
          description: workOrderData.details?.description || '',
          client: workOrderData.details?.client || '',
          location: workOrderData.details?.location || '',
          status: WorkOrderStatus.Pending,
          priority: 'medium' as WorkOrderPriority,
          category: workOrderData.details?.category || '',
          completionPercentage: 0,
          receivedDate: workOrderData.details?.receivedDate || new Date().toISOString(),
          startDate: workOrderData.details?.startDate || new Date().toISOString(),
          dueDate: workOrderData.details?.dueDate || new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString(),
          targetEndDate: workOrderData.details?.targetEndDate || new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString(),
          createdDate: new Date().toISOString(),
          createdBy: 'Current User' // This should come from auth service
        },
        items: workOrderData.items || [],
        remarks: [],
        issues: [],
        materials: [],
        permits: [],
        tasks: []
      };

      return this.mockDatabaseService.createWorkOrder(newWorkOrder).pipe(
        map(workOrder => {
          this.newWorkOrderSubject.next(workOrder);
          return workOrder;
        })
      );
    } else {
      return this.apiService.post<WorkOrder>(this.endpoint, workOrderData).pipe(
        map(response => response.data)
      );
    }
  }

  updateWorkOrder(id: string, workOrderData: Partial<WorkOrder>): Observable<WorkOrder> {
    console.log('[DEBUG] Service.updateWorkOrder called with:', id, workOrderData);
    if (environment.useMockData) {
      return this.mockDatabaseService.updateWorkOrder(id, workOrderData).pipe(
        map(result => {
          console.log('[DEBUG] Service.updateWorkOrder mock DB returned:', result);
          return result;
        })
      );
    } else {
      return this.apiService.put<WorkOrder>(`${this.endpoint}/${id}`, workOrderData).pipe(
        map(response => response.data)
      );
    }
  }

  deleteWorkOrder(id: string): Observable<boolean> {
    if (environment.useMockData) {
      return this.mockDatabaseService.deleteWorkOrder(id);
    } else {
      return this.apiService.delete<boolean>(`${this.endpoint}/${id}`).pipe(
        map(response => response.data)
      );
    }
  }

  updateWorkOrderPriority(id: string, priority: WorkOrderPriority): Observable<WorkOrder> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrderById(id).pipe(
        switchMap(workOrder => {
          if (!workOrder) {
            throw new Error(`Work order with id ${id} not found`);
          }
          
          const updatedWorkOrder = {
            ...workOrder,
            details: {
              ...workOrder.details,
              priority
            }
          };

          return this.mockDatabaseService.updateWorkOrder(id, updatedWorkOrder);
        })
      );
    } else {
      return this.apiService.put<WorkOrder>(`${this.endpoint}/${id}/priority`, { priority }).pipe(
        map(response => response.data)
      );
    }
  }

  addRemarkToWorkOrder(workOrderId: string, remarkData: RemarkData): Observable<WorkOrder> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrderById(workOrderId).pipe(
        switchMap(workOrder => {
          if (!workOrder) {
            throw new Error(`Work order with id ${workOrderId} not found`);
          }

          const newRemark = {
            id: crypto.randomUUID(),
            content: remarkData.content,
            createdDate: new Date().toISOString(),
            createdBy: remarkData.createdBy || 'Current User',
            type: remarkData.type || 'general',
            workOrderId,
            peopleInvolved: remarkData.peopleInvolved || []
          };

          const updatedWorkOrder = {
            ...workOrder,
            remarks: [...(workOrder.remarks || []), newRemark]
          };

          return this.mockDatabaseService.updateWorkOrder(workOrderId, updatedWorkOrder);
        })
      );
    } else {
      return this.apiService.post<WorkOrder>(`${this.endpoint}/${workOrderId}/remarks`, remarkData).pipe(
        map(response => response.data)
      );
    }
  }

  updateRemark(workOrderId: string, remarkId: string, remarkData: Partial<RemarkData>): Observable<WorkOrder> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrderById(workOrderId).pipe(
        switchMap(workOrder => {
          if (!workOrder) {
            throw new Error(`Work order with id ${workOrderId} not found`);
          }

          const remarks = workOrder.remarks || [];
          const remarkIndex = remarks.findIndex(r => r.id === remarkId);
          
          if (remarkIndex === -1) {
            throw new Error(`Remark with id ${remarkId} not found`);
          }

          const updatedRemarks = [...remarks];
          updatedRemarks[remarkIndex] = {
            ...updatedRemarks[remarkIndex],
            ...remarkData
          };

          const updatedWorkOrder = {
            ...workOrder,
            remarks: updatedRemarks
          };

          return this.mockDatabaseService.updateWorkOrder(workOrderId, updatedWorkOrder);
        })
      );
    } else {
      return this.apiService.put<WorkOrder>(`${this.endpoint}/${workOrderId}/remarks/${remarkId}`, remarkData).pipe(
        map(response => response.data)
      );
    }
  }

  deleteRemark(workOrderId: string, remarkId: string): Observable<WorkOrder> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrderById(workOrderId).pipe(
        switchMap(workOrder => {
          if (!workOrder) {
            throw new Error(`Work order with id ${workOrderId} not found`);
          }

          const remarks = workOrder.remarks || [];
          const filteredRemarks = remarks.filter(r => r.id !== remarkId);

          const updatedWorkOrder = {
            ...workOrder,
            remarks: filteredRemarks
          };

          return this.mockDatabaseService.updateWorkOrder(workOrderId, updatedWorkOrder);
        })
      );
    } else {
      return this.apiService.delete<WorkOrder>(`${this.endpoint}/${workOrderId}/remarks/${remarkId}`).pipe(
        map(response => response.data)
      );
    }
  }

  updateWorkOrderTask(workOrderId: string, taskIndex: number, updatedTask: Task): Observable<WorkOrder> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrderById(workOrderId).pipe(
        switchMap(workOrder => {
          if (!workOrder) {
            throw new Error(`Work order with id ${workOrderId} not found`);
          }

          const tasks = workOrder.tasks || [];
          if (taskIndex < 0 || taskIndex >= tasks.length) {
            throw new Error(`Task index ${taskIndex} is out of bounds`);
          }

          const updatedTasks = [...tasks];
          updatedTasks[taskIndex] = updatedTask;

          const updatedWorkOrder = {
            ...workOrder,
            tasks: updatedTasks
          };

          return this.mockDatabaseService.updateWorkOrder(workOrderId, updatedWorkOrder);
        })
      );
    } else {
      return this.apiService.put<WorkOrder>(`${this.endpoint}/${workOrderId}/tasks/${taskIndex}`, updatedTask).pipe(
        map(response => response.data)
      );
    }
  }

  addTaskToWorkOrder(workOrderId: string, taskData: Partial<Task>): Observable<WorkOrder> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrderById(workOrderId).pipe(
        switchMap(workOrder => {
          if (!workOrder) {
            throw new Error(`Work order with id ${workOrderId} not found`);
          }

          const newTask: Task = {
            id: crypto.randomUUID(),
            title: taskData.title || '',
            description: taskData.description || '',
            dueDate: taskData.dueDate || new Date().toISOString(),
            startDate: taskData.startDate || new Date().toISOString(),
            priority: taskData.priority || 'medium',
            status: taskData.status || 'pending',
            completed: taskData.completed || false,
            workOrderId: workOrderId,
            manpower: taskData.manpower || [],
            equipment: taskData.equipment || [],
            createdBy: taskData.createdBy || 'Current User',
            createdAt: new Date()
          };

          const tasks = workOrder.tasks || [];
          const updatedWorkOrder = {
            ...workOrder,
            tasks: [...tasks, newTask]
          };

          return this.mockDatabaseService.updateWorkOrder(workOrderId, updatedWorkOrder);
        })
      );
    } else {
      return this.apiService.post<WorkOrder>(`${this.endpoint}/${workOrderId}/tasks`, taskData).pipe(
        map(response => response.data)
      );
    }
  }

  deleteTask(workOrderId: string, taskIndex: number): Observable<WorkOrder> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrderById(workOrderId).pipe(
        switchMap(workOrder => {
          if (!workOrder) {
            throw new Error(`Work order with id ${workOrderId} not found`);
          }

          const tasks = workOrder.tasks || [];
          if (taskIndex < 0 || taskIndex >= tasks.length) {
            throw new Error(`Task index ${taskIndex} is out of bounds`);
          }

          const updatedTasks = tasks.filter((_, index) => index !== taskIndex);

          const updatedWorkOrder = {
            ...workOrder,
            tasks: updatedTasks
          };

          return this.mockDatabaseService.updateWorkOrder(workOrderId, updatedWorkOrder);
        })
      );
    } else {
      return this.apiService.delete<WorkOrder>(`${this.endpoint}/${workOrderId}/tasks/${taskIndex}`).pipe(
        map(response => response.data)
      );
    }
  }

  addIssue(workOrderId: string, issue: WorkOrderIssue): Observable<boolean> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrderById(workOrderId).pipe(
        switchMap(workOrder => {
          if (!workOrder) {
            throw new Error(`Work order with id ${workOrderId} not found`);
          }

          const newIssue: WorkOrderIssue = {
            id: crypto.randomUUID(),
            title: issue.title,
            description: issue.description,
            priority: issue.priority,
            severity: issue.severity,
            status: issue.status || 'open',
            reportedDate: issue.reportedDate || new Date(),
            reportedBy: issue.reportedBy || 'Current User',
            assignedTo: issue.assignedTo,
            resolutionDate: issue.resolutionDate,
            resolutionNotes: issue.resolutionNotes
          };

          const issues = workOrder.issues || [];
          const updatedWorkOrder = {
            ...workOrder,
            issues: [...issues, newIssue]
          };

          return this.mockDatabaseService.updateWorkOrder(workOrderId, updatedWorkOrder).pipe(
            map(() => true)
          );
        })
      );
    } else {
      return this.apiService.post<boolean>(`${this.endpoint}/${workOrderId}/issues`, issue).pipe(
        map(response => response.data)
      );
    }
  }

  updateIssue(workOrderId: string, issueId: string, updates: Partial<WorkOrderIssue>): Observable<boolean> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrderById(workOrderId).pipe(
        switchMap(workOrder => {
          if (!workOrder) {
            throw new Error(`Work order with id ${workOrderId} not found`);
          }

          const issues = workOrder.issues || [];
          const issueIndex = issues.findIndex(i => i.id === issueId);
          
          if (issueIndex === -1) {
            throw new Error(`Issue with id ${issueId} not found`);
          }

          const updatedIssues = [...issues];
          updatedIssues[issueIndex] = {
            ...updatedIssues[issueIndex],
            ...updates
          };

          const updatedWorkOrder = {
            ...workOrder,
            issues: updatedIssues
          };

          return this.mockDatabaseService.updateWorkOrder(workOrderId, updatedWorkOrder).pipe(
            map(() => true)
          );
        })
      );
    } else {
      return this.apiService.put<boolean>(`${this.endpoint}/${workOrderId}/issues/${issueId}`, updates).pipe(
        map(response => response.data)
      );
    }
  }

  deleteIssue(workOrderId: string, issueId: string): Observable<boolean> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrderById(workOrderId).pipe(
        switchMap(workOrder => {
          if (!workOrder) {
            throw new Error(`Work order with id ${workOrderId} not found`);
          }

          const issues = workOrder.issues || [];
          const filteredIssues = issues.filter(i => i.id !== issueId);

          const updatedWorkOrder = {
            ...workOrder,
            issues: filteredIssues
          };

          return this.mockDatabaseService.updateWorkOrder(workOrderId, updatedWorkOrder).pipe(
            map(() => true)
          );
        })
      );
    } else {
      return this.apiService.delete<boolean>(`${this.endpoint}/${workOrderId}/issues/${issueId}`).pipe(
        map(response => response.data)
      );
    }
  }

  assignMaterial(workOrderId: string, material: materialAssignment): Observable<boolean> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrderById(workOrderId).pipe(
        switchMap(workOrder => {
          if (!workOrder) {
            throw new Error(`Work order with id ${workOrderId} not found`);
          }

          const newMaterialAssignment: materialAssignment = {
            id: crypto.randomUUID(),
            materialType: material.materialType,
            purchasableMaterial: material.purchasableMaterial,
            receivableMaterial: material.receivableMaterial,
            workOrderNumber: workOrderId,
            assignDate: material.assignDate || new Date(),
            assignedBy: material.assignedBy || 'Current User',
            storingLocation: material.storingLocation
          };

          const materials = workOrder.materials || [];
          const updatedWorkOrder = {
            ...workOrder,
            materials: [...materials, newMaterialAssignment]
          };

          return this.mockDatabaseService.updateWorkOrder(workOrderId, updatedWorkOrder).pipe(
            map(() => true)
          );
        })
      );
    } else {
      return this.apiService.post<boolean>(`${this.endpoint}/${workOrderId}/materials`, material).pipe(
        map(response => response.data)
      );
    }
  }

  updateMaterialAssignment(workOrderId: string, assignmentId: string, updates: Partial<materialAssignment>): Observable<boolean> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrderById(workOrderId).pipe(
        switchMap(workOrder => {
          if (!workOrder) {
            throw new Error(`Work order with id ${workOrderId} not found`);
          }

          const materials = workOrder.materials || [];
          const materialIndex = materials.findIndex(m => m.id === assignmentId);
          
          if (materialIndex === -1) {
            throw new Error(`Material assignment with id ${assignmentId} not found`);
          }

          const updatedMaterials = [...materials];
          updatedMaterials[materialIndex] = {
            ...updatedMaterials[materialIndex],
            ...updates
          };

          const updatedWorkOrder = {
            ...workOrder,
            materials: updatedMaterials
          };

          return this.mockDatabaseService.updateWorkOrder(workOrderId, updatedWorkOrder).pipe(
            map(() => true)
          );
        })
      );
    } else {
      return this.apiService.put<boolean>(`${this.endpoint}/${workOrderId}/materials/${assignmentId}`, updates).pipe(
        map(response => response.data)
      );
    }
  }

  removeMaterialAssignment(workOrderId: string, assignmentId: string): Observable<boolean> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrderById(workOrderId).pipe(
        switchMap(workOrder => {
          if (!workOrder) {
            throw new Error(`Work order with id ${workOrderId} not found`);
          }

          const materials = workOrder.materials || [];
          const filteredMaterials = materials.filter(m => m.id !== assignmentId);

          const updatedWorkOrder = {
            ...workOrder,
            materials: filteredMaterials
          };

          return this.mockDatabaseService.updateWorkOrder(workOrderId, updatedWorkOrder).pipe(
            map(() => true)
          );
        })
      );
    } else {
      return this.apiService.delete<boolean>(`${this.endpoint}/${workOrderId}/materials/${assignmentId}`).pipe(
        map(response => response.data)
      );
    }
  }

  /**
   * Add an item to a work order and update expected cost (SSOT)
   */
  addItemToWorkOrder(workOrderId: string, newItem: any): Observable<WorkOrder> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getWorkOrderById(workOrderId).pipe(
        switchMap(workOrder => {
          if (!workOrder) {
            throw new Error(`Work order with id ${workOrderId} not found`);
          }
          const items = workOrder.items || [];
          // Wrap the new item in the correct structure
          const workOrderItem = {
            id: crypto.randomUUID(),
            itemDetail: {
              id: newItem.id,
              itemNumber: newItem.itemNumber,
              lineType: newItem.lineType,
              shortDescription: newItem.shortDescription,
              longDescription: newItem.longDescription,
              UOM: newItem.UOM,
              currency: newItem.currency,
              unitPrice: newItem.unitPrice,
              paymentType: newItem.paymentType,
              managementArea: newItem.managementArea
            },
            estimatedQuantity: newItem.estimatedQuantity,
            estimatedPrice: (newItem.unitPrice || 0) * (newItem.estimatedQuantity || 0),
            estimatedPriceWithVAT: 0,
            actualQuantity: 0,
            actualPrice: 0,
            actualPriceWithVAT: 0,
            reasonForFinalQuantity: ''
          };
          const updatedItems = [...items, workOrderItem];
          // Recalculate expected cost
          const totalEstimatedPrice = updatedItems.reduce((sum, item) => sum + (item.estimatedPrice || 0), 0);
          const updatedWorkOrder = {
            ...workOrder,
            items: updatedItems,
            details: {
              ...workOrder.details,
              estimatedPrice: totalEstimatedPrice
            }
          };
          return this.mockDatabaseService.updateWorkOrder(workOrderId, updatedWorkOrder);
        })
      );
    } else {
      // Real API call (assumes PUT endpoint for items)
      // Should also wrap the item as above if needed by backend
      return this.apiService.put<WorkOrder>(`${this.endpoint}/${workOrderId}/items`, { item: newItem }).pipe(
        map((response: any) => response.data)
      );
    }
  }

  updateWorkOrderPermits(workOrderId: string, permits: { type: string, status: string }[]): Observable<any> {
    return this.apiService.post<any>(`${this.endpoint}/${workOrderId}/permits`, permits);
  }
}
