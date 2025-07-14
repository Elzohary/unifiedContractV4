import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

// Import all models
import { WorkOrder } from '../../domains/work-order/models/work-order.model';
import { Iitem } from '../../domains/work-order/models/work-order-item.model';
import { BaseMaterial } from '../../domains/materials/models/material.model';
import { User } from '../../users/services/user.service';
import { Employee } from '../../modules/hr/models/employee.model';
import { Notification } from '../../shared/services/notification.service';
import { Task } from '../../domains/work-order/models/work-order.model';
import { WorkOrderRemark } from '../../domains/work-order/models/work-order.model';
import { UploadedDocument } from '../../shared/services/document.service';
import { ActivityLog } from '../../shared/models/activity-log.model';
import { Equipment } from '../../modules/resources/models/equipment.model';
import { MaterialReallocation } from '../../domains/materials/models/material-reallocation.model';

// Centralized database interface
interface MockDatabase {
  workOrders: WorkOrder[];
  workOrderItems: Iitem[];
  materials: BaseMaterial[];
  users: User[];
  employees: Employee[];
  notifications: Notification[];
  tasks: Task[];
  remarks: WorkOrderRemark[];
  documents: UploadedDocument[];
  activityLogs: ActivityLog[];
  equipment: Equipment[];
  materialReallocations: MaterialReallocation[];
}

const DB_KEY = 'unifiedContractMockDatabase';

@Injectable({ providedIn: 'root' })
export class MockDatabaseService {
  private data: MockDatabase = {
    workOrders: [],
    workOrderItems: [],
    materials: [],
    users: [],
    employees: [],
    notifications: [],
    tasks: [],
    remarks: [],
    documents: [],
    activityLogs: [],
    equipment: [],
    materialReallocations: []
  };

  // Subjects for reactive updates
  private workOrdersSubject = new BehaviorSubject<WorkOrder[]>([]);
  private workOrderItemsSubject = new BehaviorSubject<Iitem[]>([]);
  private materialsSubject = new BehaviorSubject<BaseMaterial[]>([]);
  private usersSubject = new BehaviorSubject<User[]>([]);
  private employeesSubject = new BehaviorSubject<Employee[]>([]);
  private notificationsSubject = new BehaviorSubject<Notification[]>([]);
  private tasksSubject = new BehaviorSubject<Task[]>([]);
  private remarksSubject = new BehaviorSubject<WorkOrderRemark[]>([]);
  private documentsSubject = new BehaviorSubject<UploadedDocument[]>([]);
  private activityLogsSubject = new BehaviorSubject<ActivityLog[]>([]);
  private equipmentSubject = new BehaviorSubject<Equipment[]>([]);
  private materialReallocationsSubject = new BehaviorSubject<MaterialReallocation[]>([]);

  // Public observables
  workOrders$ = this.workOrdersSubject.asObservable();
  workOrderItems$ = this.workOrderItemsSubject.asObservable();
  materials$ = this.materialsSubject.asObservable();
  users$ = this.usersSubject.asObservable();
  employees$ = this.employeesSubject.asObservable();
  notifications$ = this.notificationsSubject.asObservable();
  tasks$ = this.tasksSubject.asObservable();
  remarks$ = this.remarksSubject.asObservable();
  documents$ = this.documentsSubject.asObservable();
  activityLogs$ = this.activityLogsSubject.asObservable();
  equipment$ = this.equipmentSubject.asObservable();
  materialReallocations$ = this.materialReallocationsSubject.asObservable();

  constructor() {
    this.loadFromStorage();
    this.initializeWithMockData();
  }

  // ===== PRIVATE METHODS =====

  private loadFromStorage(): void {
    try {
      const saved = localStorage.getItem(DB_KEY);
      if (saved) {
        const loadedData = JSON.parse(saved);
        // Ensure all required arrays exist
        this.data = {
          workOrders: loadedData.workOrders || [],
          workOrderItems: loadedData.workOrderItems || [],
          materials: loadedData.materials || [],
          users: loadedData.users || [],
          employees: loadedData.employees || [],
          notifications: loadedData.notifications || [],
          tasks: loadedData.tasks || [],
          remarks: loadedData.remarks || [],
          documents: loadedData.documents || [],
          activityLogs: loadedData.activityLogs || [],
          equipment: loadedData.equipment || [],
          materialReallocations: loadedData.materialReallocations || []
        };
        this.updateAllSubjects();
      }
    } catch (error) {
      console.warn('Failed to load mock database from storage:', error);
    }
  }

  private saveToStorage(): void {
    try {
      localStorage.setItem(DB_KEY, JSON.stringify(this.data));
    } catch (error) {
      console.warn('Failed to save mock database to storage:', error);
    }
  }

  private updateAllSubjects(): void {
    this.workOrdersSubject.next([...this.data.workOrders]);
    this.workOrderItemsSubject.next([...this.data.workOrderItems]);
    this.materialsSubject.next([...this.data.materials]);
    this.usersSubject.next([...this.data.users]);
    this.employeesSubject.next([...this.data.employees]);
    this.notificationsSubject.next([...this.data.notifications]);
    this.tasksSubject.next([...this.data.tasks]);
    this.remarksSubject.next([...this.data.remarks]);
    this.documentsSubject.next([...this.data.documents]);
    this.activityLogsSubject.next([...this.data.activityLogs]);
    this.equipmentSubject.next([...this.data.equipment]);
    this.materialReallocationsSubject.next([...this.data.materialReallocations]);
  }

  private initializeWithMockData(): void {
    // Only initialize if no data exists
    if (this.data.workOrders.length === 0) {
      this.initializeWorkOrders();
    }
    if (this.data.workOrderItems.length === 0) {
      this.initializeWorkOrderItems();
    }
    if (this.data.materials.length === 0) {
      this.initializeMaterials();
    }
    if (this.data.users.length === 0) {
      this.initializeUsers();
    }
    if (this.data.employees.length === 0) {
      this.initializeEmployees();
    }
    if (this.data.notifications.length === 0) {
      this.initializeNotifications();
    }
    if (this.data.tasks.length === 0) {
      this.initializeTasks();
    }
    if (this.data.remarks.length === 0) {
      this.initializeRemarks();
    }
    if (this.data.equipment.length === 0) {
      this.initializeEquipment();
    }
    if (this.data.materialReallocations.length === 0) {
      this.initializeMaterialReallocations();
    }
    
    this.saveToStorage();
    this.updateAllSubjects();
  }

  // ===== WORK ORDERS =====

  getWorkOrders(): Observable<WorkOrder[]> {
    return this.workOrders$.pipe(delay(environment.mockDataDelay));
  }

  getWorkOrderById(id: string): Observable<WorkOrder | undefined> {
    const workOrder = this.data.workOrders.find(wo => wo.id === id);
    return of(workOrder).pipe(delay(environment.mockDataDelay));
  }

  createWorkOrder(workOrder: Partial<WorkOrder>): Observable<WorkOrder> {
    const newWorkOrder: WorkOrder = {
      ...workOrder,
      id: crypto.randomUUID(),
      details: {
        ...workOrder.details,
        createdDate: new Date().toISOString()
      }
    } as WorkOrder;

    this.data.workOrders.push(newWorkOrder);
    this.workOrdersSubject.next([...this.data.workOrders]);
    this.saveToStorage();
    
    return of(newWorkOrder).pipe(delay(environment.mockDataDelay));
  }

  updateWorkOrder(id: string, updates: Partial<WorkOrder>): Observable<WorkOrder> {
    const index = this.data.workOrders.findIndex(wo => wo.id === id);
    if (index !== -1) {
      // Deep merge for permits array
      const existing = this.data.workOrders[index];
      let merged: WorkOrder;
      if ('permits' in updates) {
        merged = { ...existing, ...updates, permits: updates.permits };
      } else {
        merged = { ...existing, ...updates, permits: existing.permits };
      }
      this.data.workOrders[index] = merged;
      this.workOrdersSubject.next([...this.data.workOrders]);
      this.saveToStorage();
      return of(this.data.workOrders[index]).pipe(delay(environment.mockDataDelay));
    }
    throw new Error(`Work order with id ${id} not found`);
  }

  deleteWorkOrder(id: string): Observable<boolean> {
    const index = this.data.workOrders.findIndex(wo => wo.id === id);
    if (index !== -1) {
      this.data.workOrders.splice(index, 1);
      this.workOrdersSubject.next([...this.data.workOrders]);
      this.saveToStorage();
      return of(true).pipe(delay(environment.mockDataDelay));
    }
    return of(false).pipe(delay(environment.mockDataDelay));
  }

  // ===== WORK ORDER ITEMS =====

  getWorkOrderItems(): Observable<Iitem[]> {
    return this.workOrderItems$.pipe(delay(environment.mockDataDelay));
  }

  getWorkOrderItemById(id: string): Observable<Iitem | undefined> {
    const item = this.data.workOrderItems.find(item => item.id === id);
    return of(item).pipe(delay(environment.mockDataDelay));
  }

  createWorkOrderItem(item: Partial<Iitem>): Observable<Iitem> {
    const newItem: Iitem = {
      ...item,
      id: crypto.randomUUID()
    } as Iitem;

    this.data.workOrderItems.push(newItem);
    this.workOrderItemsSubject.next([...this.data.workOrderItems]);
    this.saveToStorage();
    
    return of(newItem).pipe(delay(environment.mockDataDelay));
  }

  updateWorkOrderItem(id: string, updates: Partial<Iitem>): Observable<Iitem> {
    const index = this.data.workOrderItems.findIndex(item => item.id === id);
    if (index !== -1) {
      this.data.workOrderItems[index] = { ...this.data.workOrderItems[index], ...updates };
      this.workOrderItemsSubject.next([...this.data.workOrderItems]);
      this.saveToStorage();
      return of(this.data.workOrderItems[index]).pipe(delay(environment.mockDataDelay));
    }
    throw new Error(`Work order item with id ${id} not found`);
  }

  deleteWorkOrderItem(id: string): Observable<boolean> {
    const index = this.data.workOrderItems.findIndex(item => item.id === id);
    if (index !== -1) {
      this.data.workOrderItems.splice(index, 1);
      this.workOrderItemsSubject.next([...this.data.workOrderItems]);
      this.saveToStorage();
      return of(true).pipe(delay(environment.mockDataDelay));
    }
    return of(false).pipe(delay(environment.mockDataDelay));
  }

  // ===== MATERIALS =====

  getMaterials(): Observable<BaseMaterial[]> {
    return this.materials$.pipe(delay(environment.mockDataDelay));
  }

  getMaterialById(id: string): Observable<BaseMaterial | undefined> {
    const material = this.data.materials.find(m => m.id === id);
    return of(material).pipe(delay(environment.mockDataDelay));
  }

  createMaterial(material: Partial<BaseMaterial>): Observable<BaseMaterial> {
    const newMaterial: BaseMaterial = {
      ...material,
      id: crypto.randomUUID()
    } as BaseMaterial;

    this.data.materials.push(newMaterial);
    this.materialsSubject.next([...this.data.materials]);
    this.saveToStorage();
    
    return of(newMaterial).pipe(delay(environment.mockDataDelay));
  }

  updateMaterial(id: string, updates: Partial<BaseMaterial>): Observable<BaseMaterial> {
    const index = this.data.materials.findIndex(m => m.id === id);
    if (index !== -1) {
      this.data.materials[index] = { ...this.data.materials[index], ...updates };
      this.materialsSubject.next([...this.data.materials]);
      this.saveToStorage();
      return of(this.data.materials[index]).pipe(delay(environment.mockDataDelay));
    }
    throw new Error(`Material with id ${id} not found`);
  }

  deleteMaterial(id: string): Observable<boolean> {
    const index = this.data.materials.findIndex(m => m.id === id);
    if (index !== -1) {
      this.data.materials.splice(index, 1);
      this.materialsSubject.next([...this.data.materials]);
      this.saveToStorage();
      return of(true).pipe(delay(environment.mockDataDelay));
    }
    return of(false).pipe(delay(environment.mockDataDelay));
  }

  // ===== USERS =====

  getUsers(): Observable<User[]> {
    return this.users$.pipe(delay(environment.mockDataDelay));
  }

  getUserById(id: number): Observable<User | undefined> {
    const user = this.data.users.find(u => u.id === id);
    return of(user).pipe(delay(environment.mockDataDelay));
  }

  createUser(user: Partial<User>): Observable<User> {
    const newUser: User = {
      ...user,
      id: this.data.users.length + 1
    } as User;

    this.data.users.push(newUser);
    this.usersSubject.next([...this.data.users]);
    this.saveToStorage();
    
    return of(newUser).pipe(delay(environment.mockDataDelay));
  }

  updateUser(id: number, updates: Partial<User>): Observable<User> {
    const index = this.data.users.findIndex(u => u.id === id);
    if (index !== -1) {
      this.data.users[index] = { ...this.data.users[index], ...updates };
      this.usersSubject.next([...this.data.users]);
      this.saveToStorage();
      return of(this.data.users[index]).pipe(delay(environment.mockDataDelay));
    }
    throw new Error(`User with id ${id} not found`);
  }

  deleteUser(id: number): Observable<boolean> {
    const index = this.data.users.findIndex(u => u.id === id);
    if (index !== -1) {
      this.data.users.splice(index, 1);
      this.usersSubject.next([...this.data.users]);
      this.saveToStorage();
      return of(true).pipe(delay(environment.mockDataDelay));
    }
    return of(false).pipe(delay(environment.mockDataDelay));
  }

  // ===== EMPLOYEES =====

  getEmployees(): Observable<Employee[]> {
    return this.employees$.pipe(delay(environment.mockDataDelay));
  }

  getEmployeeById(id: string): Observable<Employee | undefined> {
    const employee = this.data.employees.find(e => e.id === id);
    return of(employee).pipe(delay(environment.mockDataDelay));
  }

  createEmployee(employee: Partial<Employee>): Observable<Employee> {
    const newEmployee: Employee = {
      ...employee,
      id: crypto.randomUUID(),
      createdAt: new Date(),
      updatedAt: new Date()
    } as Employee;

    this.data.employees.push(newEmployee);
    this.employeesSubject.next([...this.data.employees]);
    this.saveToStorage();
    
    return of(newEmployee).pipe(delay(environment.mockDataDelay));
  }

  updateEmployee(id: string, updates: Partial<Employee>): Observable<Employee> {
    const index = this.data.employees.findIndex(e => e.id === id);
    if (index !== -1) {
      this.data.employees[index] = { 
        ...this.data.employees[index], 
        ...updates,
        updatedAt: new Date()
      };
      this.employeesSubject.next([...this.data.employees]);
      this.saveToStorage();
      return of(this.data.employees[index]).pipe(delay(environment.mockDataDelay));
    }
    throw new Error(`Employee with id ${id} not found`);
  }

  deleteEmployee(id: string): Observable<boolean> {
    const index = this.data.employees.findIndex(e => e.id === id);
    if (index !== -1) {
      this.data.employees.splice(index, 1);
      this.employeesSubject.next([...this.data.employees]);
      this.saveToStorage();
      return of(true).pipe(delay(environment.mockDataDelay));
    }
    return of(false).pipe(delay(environment.mockDataDelay));
  }

  // ===== MATERIAL REALLOCATIONS =====
  createMaterialReallocation(reallocation: MaterialReallocation): Observable<MaterialReallocation> {
    this.data.materialReallocations.push(reallocation);
    this.materialReallocationsSubject.next([...this.data.materialReallocations]);
    this.saveToStorage();
    return of(reallocation).pipe(delay(environment.mockDataDelay));
  }

  getMaterialReallocationById(id: string): Observable<MaterialReallocation | undefined> {
    const found = this.data.materialReallocations.find(r => r.id === id);
    return of(found).pipe(delay(environment.mockDataDelay));
  }

  updateMaterialReallocation(id: string, updates: Partial<MaterialReallocation>): Observable<MaterialReallocation> {
    const index = this.data.materialReallocations.findIndex(r => r.id === id);
    if (index !== -1) {
      this.data.materialReallocations[index] = { ...this.data.materialReallocations[index], ...updates };
      this.materialReallocationsSubject.next([...this.data.materialReallocations]);
      this.saveToStorage();
      return of(this.data.materialReallocations[index]).pipe(delay(environment.mockDataDelay));
    }
    throw new Error(`Material reallocation with id ${id} not found`);
  }

  getMaterialReallocations(): Observable<MaterialReallocation[]> {
    return this.materialReallocations$.pipe(delay(environment.mockDataDelay));
  }

  // ===== UTILITY METHODS =====

  // For testing/dev: reset the mock database
  resetDatabase(): void {
    this.data = {
      workOrders: [],
      workOrderItems: [],
      materials: [],
      users: [],
      employees: [],
      notifications: [],
      tasks: [],
      remarks: [],
      documents: [],
      activityLogs: [],
      equipment: [],
      materialReallocations: []
    };
    this.saveToStorage();
    this.updateAllSubjects();
  }

  // Export database for backup
  exportDatabase(): string {
    return JSON.stringify(this.data, null, 2);
  }

  // Import database from backup
  importDatabase(jsonData: string): void {
    try {
      this.data = JSON.parse(jsonData);
      this.saveToStorage();
      this.updateAllSubjects();
    } catch (error) {
      throw new Error('Invalid database format');
    }
  }

  // ===== INITIALIZATION METHODS =====

  private initializeWorkOrders(): void {
    // Initialize with mock work orders data
    this.data.workOrders = [
      {
        id: 'wo-001',
        details: {
          workOrderNumber: 'WO-2024-001',
          internalOrderNumber: 'INT-2024-001',
          title: 'Road Construction Project - Phase 1',
          description: 'Construction of main road connecting industrial area to highway',
          client: 'Ministry of Transportation',
          location: 'Industrial District, Riyadh',
          status: 'pending' as any,
          priority: 'high' as any,
          category: 'Infrastructure',
          completionPercentage: 0,
          receivedDate: new Date('2024-01-15'),
          startDate: new Date('2024-02-01'),
          dueDate: new Date('2024-06-30'),
          targetEndDate: new Date('2024-06-15'),
          createdDate: new Date('2024-01-15'),
          createdBy: 'Project Manager'
        },
        items: [],
        remarks: [],
        issues: [],
        materials: [
          {
            id: 'mat-assign-003',
            materialType: 'purchasable',
            purchasableMaterial: {
              id: '4', // ASP-001 - Asphalt Mix Type A
              name: 'Asphalt Mix Type A',
              description: 'Hot mix asphalt for road construction',
              quantity: 1000,
              unit: 'TON',
              unitCost: 150,
              totalCost: 150000,
              status: 'pending',
              supplier: 'Construction Materials Ltd.'
            },
            workOrderNumber: 'wo-001',
            assignDate: new Date('2024-01-20'),
            assignedBy: 'Project Manager',
            storingLocation: 'Construction Site'
          },
          {
            id: 'mat-assign-004',
            materialType: 'receivable',
            receivableMaterial: {
              id: '2', // SEC-002 - Cable 16mm
              name: 'Cable 16mm',
              description: 'Cable 16mm',
              unit: 'M',
              estimatedQuantity: 500,
              status: 'pending'
            },
            workOrderNumber: 'wo-001',
            assignDate: new Date('2024-01-25'),
            assignedBy: 'Project Manager',
            storingLocation: 'Site Warehouse'
          }
        ],
        permits: [],
        tasks: []
      },
      {
        id: 'wo-002',
        details: {
          workOrderNumber: 'WO-2024-002',
          internalOrderNumber: 'INT-2024-002',
          title: 'Electrical Substation Installation',
          description: 'Installation of new electrical substation for industrial complex',
          client: 'SEC',
          location: 'Industrial Complex, Jeddah',
          status: 'in-progress' as any,
          priority: 'medium' as any,
          category: 'Electrical',
          completionPercentage: 0,
          receivedDate: new Date('2024-02-01'),
          startDate: new Date('2024-02-15'),
          dueDate: new Date('2024-05-30'),
          targetEndDate: new Date('2024-05-15'),
          createdDate: new Date('2024-02-01'),
          createdBy: 'Electrical Engineer'
        },
        items: [],
        remarks: [
          {
            id: 'remark-001',
            content: 'Site survey completed. All measurements verified.',
            createdDate: new Date('2024-02-20').toISOString(),
            createdBy: 'Site Engineer',
            type: 'technical',
            workOrderId: 'wo-002',
            peopleInvolved: ['Site Engineer', 'Project Manager']
          }
        ],
        issues: [],
        materials: [
          {
            id: 'mat-assign-001',
            materialType: 'receivable',
            receivableMaterial: {
              id: '1', // SEC-001 - Electrical Meter Type A
              name: 'Electrical Meter Type A',
              description: 'High voltage electrical cables for substation installation',
              unit: 'PCS',
              estimatedQuantity: 10,
              receivedQuantity: 10,
              actualQuantity: 10,
              remainingQuantity: 10,
              status: 'received',
              receivedDate: new Date('2024-02-20').toISOString()
            },
            workOrderNumber: 'wo-002',
            assignDate: new Date('2024-02-15'),
            assignedBy: 'Material Manager',
            storingLocation: 'Site Warehouse'
          },
          {
            id: 'mat-assign-002',
            materialType: 'purchasable',
            purchasableMaterial: {
              id: '5', // CON-001 - Concrete Mix 40MPa
              name: 'Concrete Mix 40MPa',
              description: 'High voltage circuit breakers for electrical protection',
              quantity: 4,
              unit: 'M3',
              unitCost: 200,
              totalCost: 800,
              status: 'ordered',
              supplier: 'Concrete Supply Co.',
              orderDate: new Date('2024-02-25').toISOString(),
              deliveryDate: new Date('2024-03-10').toISOString()
            },
            workOrderNumber: 'wo-002',
            assignDate: new Date('2024-02-20'),
            assignedBy: 'Material Manager',
            storingLocation: 'Main Warehouse'
          }
        ],
        permits: [
          { id: 'permit-1', type: 'Initial', title: 'Initial Permit', description: '', number: '', issueDate: new Date(), expiryDate: new Date(), status: 'pending', issuedBy: '', authority: '', documentRef: '' },
          { id: 'permit-2', type: 'Municipality', title: 'Baladya', description: '', number: '', issueDate: new Date(), expiryDate: new Date(), status: 'pending', issuedBy: '', authority: '', documentRef: '' },
          { id: 'permit-3', type: 'RoadDepartment', title: 'Road Department', description: '', number: '', issueDate: new Date(), expiryDate: new Date(), status: 'pending', issuedBy: '', authority: '', documentRef: '' },
          { id: 'permit-4', type: 'Traffic', title: 'Traffic', description: '', number: '', issueDate: new Date(), expiryDate: new Date(), status: 'pending', issuedBy: '', authority: '', documentRef: '' }
        ],
        tasks: [
          {
            id: 'task-001',
            title: 'Foundation Preparation',
            description: 'Prepare foundation for substation equipment',
            dueDate: new Date('2024-03-15'),
            startDate: new Date('2024-02-20'),
            priority: 'high',
            status: 'in-progress',
            completed: false,
            workOrderId: 'wo-002',
            manpower: [],
            equipment: [],
            createdBy: 'Project Manager',
            createdAt: new Date('2024-02-15')
          }
        ]
      },
      {
        id: 'ddf8943e-d144-442f-8040-2f774b722b5f',
        details: {
          workOrderNumber: 'WO-2024-003',
          internalOrderNumber: 'INT-2024-003',
          title: 'Test Work Order for Permits',
          description: 'This work order is for testing the Required Permits card and event flow.',
          client: 'Test Client',
          location: 'Test Location',
          status: 'pending' as any,
          priority: 'medium' as any,
          category: 'Test',
          completionPercentage: 0,
          receivedDate: new Date('2024-03-01'),
          startDate: new Date('2024-03-05'),
          dueDate: new Date('2024-04-01'),
          targetEndDate: new Date('2024-03-30'),
          createdDate: new Date('2024-03-01'),
          createdBy: 'Test User'
        },
        items: [],
        remarks: [],
        issues: [],
        materials: [],
        permits: [
          { id: 'permit-1', type: 'Initial', title: 'Initial Permit', description: '', number: '', issueDate: new Date(), expiryDate: new Date(), status: 'pending', issuedBy: '', authority: '', documentRef: '' },
          { id: 'permit-2', type: 'Municipality', title: 'Baladya', description: '', number: '', issueDate: new Date(), expiryDate: new Date(), status: 'pending', issuedBy: '', authority: '', documentRef: '' },
          { id: 'permit-3', type: 'RoadDepartment', title: 'Road Department', description: '', number: '', issueDate: new Date(), expiryDate: new Date(), status: 'pending', issuedBy: '', authority: '', documentRef: '' },
          { id: 'permit-4', type: 'Traffic', title: 'Traffic', description: '', number: '', issueDate: new Date(), expiryDate: new Date(), status: 'pending', issuedBy: '', authority: '', documentRef: '' }
        ],
        tasks: []
      }
    ];
    this.workOrdersSubject.next([...this.data.workOrders]);
    this.saveToStorage();
  }

  private initializeWorkOrderItems(): void {
    // Initialize with basic items if needed
    this.data.workOrderItems = [];
  }

  private initializeMaterials(): void {
    // Initialize with mock materials data directly
    const mockMaterials = [
      // SEC materials
      {
        id: '1',
        code: 'SEC-001',
        description: 'Electrical Meter Type A',
        unit: 'PCS',
        materialType: 'receivable' as any,
        clientType: 'SEC' as any,
        groupCode: 'ELM',
        groupCodeDescription: 'Electrical Meters',
        SEQ: 1001,
        materialMasterCode: 'SEC-ELM-001',
        totalStock: 150,
        availableStock: 120,
        reservedStock: 30,
        minimumStock: 20,
        maximumStock: 200,
        reorderPoint: 25,
        averageCost: 250,
        lastPurchaseCost: 250,
        standardCost: 240,
        createdAt: new Date(),
        updatedAt: new Date(),
        lastUsedAt: new Date(Date.now() - 7 * 24 * 60 * 60 * 1000)
      },
      {
        id: '2',
        code: 'SEC-002',
        description: 'Cable 16mm',
        unit: 'M',
        materialType: 'receivable' as any,
        clientType: 'SEC' as any,
        groupCode: 'ELC',
        groupCodeDescription: 'Electrical Cables',
        SEQ: 2001,
        materialMasterCode: 'SEC-ELC-001',
        totalStock: 2000,
        availableStock: 1800,
        reservedStock: 200,
        minimumStock: 100,
        maximumStock: 3000,
        reorderPoint: 150,
        averageCost: 15,
        lastPurchaseCost: 15,
        standardCost: 14,
        createdAt: new Date(),
        updatedAt: new Date(),
        lastUsedAt: new Date(Date.now() - 3 * 24 * 60 * 60 * 1000)
      },
      // Purchasable materials
      {
        id: '4',
        code: 'ASP-001',
        description: 'Asphalt Mix Type A',
        unit: 'TON',
        materialType: 'purchasable' as any,
        clientType: 'OTHER' as any,
        totalStock: 500,
        availableStock: 450,
        reservedStock: 50,
        minimumStock: 50,
        maximumStock: 1000,
        reorderPoint: 75,
        averageCost: 150,
        lastPurchaseCost: 150,
        standardCost: 145,
        createdAt: new Date(),
        updatedAt: new Date(),
        lastUsedAt: new Date(Date.now() - 1 * 24 * 60 * 60 * 1000)
      },
      {
        id: '5',
        code: 'CON-001',
        description: 'Concrete Mix 40MPa',
        unit: 'M3',
        materialType: 'purchasable' as any,
        clientType: 'OTHER' as any,
        totalStock: 300,
        availableStock: 250,
        reservedStock: 50,
        minimumStock: 30,
        maximumStock: 600,
        reorderPoint: 40,
        averageCost: 200,
        lastPurchaseCost: 200,
        standardCost: 195,
        createdAt: new Date(),
        updatedAt: new Date(),
        lastUsedAt: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000)
      },
      {
        id: '6',
        code: 'BAS-001',
        description: 'Base Course Aggregate',
        unit: 'M3',
        materialType: 'purchasable' as any,
        clientType: 'OTHER' as any,
        totalStock: 800,
        availableStock: 700,
        reservedStock: 100,
        minimumStock: 80,
        maximumStock: 1500,
        reorderPoint: 100,
        averageCost: 80,
        lastPurchaseCost: 80,
        standardCost: 78,
        createdAt: new Date(),
        updatedAt: new Date(),
        lastUsedAt: new Date(Date.now() - 5 * 24 * 60 * 60 * 1000)
      }
    ];
    
    this.data.materials = mockMaterials;
    this.materialsSubject.next([...this.data.materials]);
    this.saveToStorage();
  }

  private initializeUsers(): void {
    // Initialize with mock users data
    this.data.users = [
      { 
        id: 1, 
        name: 'Alice Johnson', 
        email: 'alice@example.com', 
        role: 'Admin',
        avatar: 'assets/avatars/alice.jpg',
        isActive: true,
        createdAt: new Date('2024-01-15'),
        lastLoginAt: new Date(Date.now() - 2 * 60 * 60 * 1000) // 2 hours ago
      },
      { 
        id: 2, 
        name: 'Bob Smith', 
        email: 'bob@example.com', 
        role: 'User',
        avatar: 'assets/avatars/bob.jpg',
        isActive: true,
        createdAt: new Date('2024-02-20'),
        lastLoginAt: new Date(Date.now() - 24 * 60 * 60 * 1000) // 1 day ago
      },
      { 
        id: 3, 
        name: 'Charlie Brown', 
        email: 'charlie@example.com', 
        role: 'Manager',
        avatar: 'assets/avatars/charlie.jpg',
        isActive: true,
        createdAt: new Date('2024-03-10'),
        lastLoginAt: new Date(Date.now() - 6 * 60 * 60 * 1000) // 6 hours ago
      },
      { 
        id: 4, 
        name: 'Diana Prince', 
        email: 'diana@example.com', 
        role: 'Engineer',
        avatar: 'assets/avatars/diana.jpg',
        isActive: true,
        createdAt: new Date('2024-04-05'),
        lastLoginAt: new Date(Date.now() - 12 * 60 * 60 * 1000) // 12 hours ago
      },
      { 
        id: 5, 
        name: 'Edward Norton', 
        email: 'edward@example.com', 
        role: 'Foreman',
        avatar: 'assets/avatars/edward.jpg',
        isActive: false,
        createdAt: new Date('2024-05-12'),
        lastLoginAt: new Date(Date.now() - 7 * 24 * 60 * 60 * 1000) // 1 week ago
      }
    ];
    this.usersSubject.next([...this.data.users]);
    this.saveToStorage();
  }

  private initializeEmployees(): void {
    // Initialize with mock employees data for HR module
    this.data.employees = [
      {
        id: 'emp-001',
        firstName: 'John',
        lastName: 'Doe',
        email: 'john.doe@company.com',
        phone: '+966-50-123-4567',
        department: 'Engineering',
        position: 'Senior Engineer',
        role: 'employee' as any,
        status: 'active' as any,
        hireDate: new Date('2023-01-15'),
        skills: ['Angular', 'TypeScript', 'Node.js'],
        certifications: [
          {
            id: 'cert-001',
            name: 'AWS Certified Developer',
            issuer: 'Amazon Web Services',
            issueDate: new Date('2023-06-15'),
            expiryDate: new Date('2026-06-15')
          }
        ],
        emergencyContact: {
          name: 'Jane Doe',
          relationship: 'Spouse',
          phone: '+966-50-987-6543',
          email: 'jane.doe@email.com'
        },
        address: {
          street: '123 Main Street',
          city: 'Riyadh',
          state: 'Riyadh Province',
          country: 'Saudi Arabia',
          postalCode: '12345'
        },
        documents: [],
        leaves: [],
        performanceReviews: [],
        training: [],
        salary: {
          baseSalary: 15000,
          currency: 'SAR',
          paymentFrequency: 'monthly' as any,
          bankDetails: {
            accountNumber: '1234567890',
            bankName: 'Saudi National Bank',
            branch: 'Riyadh Main',
            accountType: 'Savings'
          },
          allowances: [
            { type: 'Housing', amount: 3000, frequency: 'monthly' as any }
          ],
          deductions: []
        },
        createdAt: new Date('2023-01-15'),
        updatedAt: new Date()
      },
      {
        id: 'emp-002',
        firstName: 'Sarah',
        lastName: 'Johnson',
        email: 'sarah.johnson@company.com',
        phone: '+966-50-234-5678',
        department: 'Management',
        position: 'Project Manager',
        role: 'manager' as any,
        status: 'active' as any,
        hireDate: new Date('2022-08-20'),
        skills: ['Project Management', 'Agile', 'Leadership'],
        certifications: [
          {
            id: 'cert-002',
            name: 'PMP Certification',
            issuer: 'Project Management Institute',
            issueDate: new Date('2022-12-10'),
            expiryDate: new Date('2025-12-10')
          }
        ],
        emergencyContact: {
          name: 'Mike Johnson',
          relationship: 'Spouse',
          phone: '+966-50-876-5432',
          email: 'mike.johnson@email.com'
        },
        address: {
          street: '456 Oak Avenue',
          city: 'Jeddah',
          state: 'Makkah Province',
          country: 'Saudi Arabia',
          postalCode: '54321'
        },
        documents: [],
        leaves: [],
        performanceReviews: [],
        training: [],
        salary: {
          baseSalary: 20000,
          currency: 'SAR',
          paymentFrequency: 'monthly' as any,
          bankDetails: {
            accountNumber: '0987654321',
            bankName: 'Al Rajhi Bank',
            branch: 'Jeddah Central',
            accountType: 'Current'
          },
          allowances: [
            { type: 'Transportation', amount: 1500, frequency: 'monthly' as any }
          ],
          deductions: []
        },
        createdAt: new Date('2022-08-20'),
        updatedAt: new Date()
      }
    ];
    this.employeesSubject.next([...this.data.employees]);
    this.saveToStorage();
  }

  private initializeNotifications(): void {
    // Initialize with basic notifications if needed
    this.data.notifications = [];
  }

  private initializeTasks(): void {
    // Initialize with basic tasks if needed
    this.data.tasks = [];
  }

  private initializeRemarks(): void {
    // Initialize with basic remarks if needed
    this.data.remarks = [];
  }

  private initializeEquipment(): void {
    // Initialize with basic equipment if needed
    this.data.equipment = [];
  }

  private initializeMaterialReallocations(): void {
    // Initialize with basic material reallocations if needed
    this.data.materialReallocations = [];
  }
} 