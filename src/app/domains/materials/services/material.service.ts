 
import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { map, tap, catchError, switchMap } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { BaseMaterial, ClientType, MaterialType, SecMaterial } from '../models/material.model';
import { MockDatabaseService } from '../../../core/services/mock-database.service';
import { ApiService } from '../../../core/services/api.service';
import { WorkOrderService } from '../../work-order/services/work-order.service';
import { materialAssignment } from '../../work-order/models/work-order.model';

@Injectable({
  providedIn: 'root'
})
export class MaterialService {
  private materialsSubject = new BehaviorSubject<BaseMaterial[]>([]);
  public materials$ = this.materialsSubject.asObservable();

  constructor(
    private mockDatabaseService: MockDatabaseService,
    private apiService: ApiService,
    private workOrderService: WorkOrderService
  ) {
    console.log('[DEBUG] MaterialService constructor called');
    this.loadMaterials();
  }

  /**
   * Load materials from the centralized database
   */
  loadMaterials(clientType?: ClientType): Observable<BaseMaterial[]> {
    console.log('[DEBUG] Loading materials, clientType:', clientType);
    
    if (environment.useMockData) {
      return this.mockDatabaseService.getMaterials().pipe(
        map(materials => {
          console.log('[DEBUG] Got materials from mock database:', materials.length);
          let filteredMaterials = [...materials];
          
          if (clientType) {
            filteredMaterials = filteredMaterials.filter(m => m.clientType === clientType);
          }
          
          this.materialsSubject.next(filteredMaterials);
          console.log('[DEBUG] Materials subject updated with:', filteredMaterials.length);
          return filteredMaterials;
        })
      );
    } else {
      const endpoint = 'materials';
      const queryParams = clientType ? `?clientType=${clientType}` : '';
      
      return this.apiService.get<BaseMaterial[]>(`${endpoint}${queryParams}`).pipe(
        map(response => response.data),
        tap(materials => this.materialsSubject.next(materials))
      );
    }
  }

  /**
   * Add a new material to the system
   */
  addMaterial(material: BaseMaterial): Observable<BaseMaterial> {
    if (environment.useMockData) {
      return this.mockDatabaseService.createMaterial(material).pipe(
        tap(() => this.refreshMaterials())
      );
    } else {
      return this.apiService.post<BaseMaterial>('materials', material).pipe(
        map(response => response.data),
        tap(() => this.refreshMaterials())
      );
    }
  }

  /**
   * Update an existing material
   */
  updateMaterial(material: BaseMaterial): Observable<BaseMaterial> {
    if (environment.useMockData) {
      return this.mockDatabaseService.updateMaterial(material.id!, material).pipe(
        tap(() => this.refreshMaterials())
      );
    } else {
      return this.apiService.put<BaseMaterial>(`materials/${material.id}`, material).pipe(
        map(response => response.data),
        tap(() => this.refreshMaterials())
      );
    }
  }

  /**
   * Delete a material by ID
   */
  deleteMaterial(id: string): Observable<boolean> {
    if (environment.useMockData) {
      return this.mockDatabaseService.deleteMaterial(id).pipe(
        tap(() => this.refreshMaterials())
      );
    } else {
      return this.apiService.delete<boolean>(`materials/${id}`).pipe(
        map(response => response.data),
        tap(() => this.refreshMaterials())
      );
    }
  }

  /**
   * Get material by ID
   */
  getMaterialById(id: string): Observable<BaseMaterial | undefined> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getMaterialById(id);
    } else {
      return this.apiService.get<BaseMaterial>(`materials/${id}`).pipe(
        map(response => response.data)
      );
    }
  }

  /**
   * Refresh the materials list from the centralized database
   */
  private refreshMaterials(): void {
    console.log('[DEBUG] Refreshing materials list');
    this.loadMaterials().subscribe(materials => {
      console.log('[DEBUG] Got updated materials list. Count:', materials.length);
      this.materialsSubject.next(materials);
    });
  }

  /**
   * Get materials filtered by type
   */
  getMaterialsByType(materialType: MaterialType): Observable<BaseMaterial[]> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getMaterials().pipe(
        map(materials => materials.filter(m => m.materialType === materialType))
      );
    } else {
      return this.apiService.get<BaseMaterial[]>(`materials?type=${materialType}`).pipe(
        map(response => response.data)
      );
    }
  }

  /**
   * Get SEC materials specifically
   */
  getSecMaterials(): Observable<SecMaterial[]> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getMaterials().pipe(
        map(materials => materials.filter(m => m.clientType === ClientType.SEC) as SecMaterial[])
      );
    } else {
      return this.apiService.get<SecMaterial[]>(`materials?clientType=${ClientType.SEC}`).pipe(
        map(response => response.data)
      );
    }
  }

  /**
   * Assign material to a work order
   * This maintains the business logic relationship between materials and work orders
   */
  assignMaterialToWorkOrder(materialId: string, workOrderId: string, quantity: number): Observable<boolean> {
    if (environment.useMockData) {
      // Get the material first
      return this.mockDatabaseService.getMaterialById(materialId).pipe(
        switchMap(material => {
          if (!material) {
            throw new Error(`Material with id ${materialId} not found`);
          }

          // Create material assignment for work order
          const materialAssignment: materialAssignment = {
            id: crypto.randomUUID(),
            materialType: material.materialType === MaterialType.PURCHASABLE ? 'purchasable' : 'receivable',
            workOrderNumber: workOrderId,
            assignDate: new Date(),
            assignedBy: 'Current User',
            storingLocation: 'Main Warehouse'
          };

          // Update material stock (reserve the quantity)
          const updatedMaterial = {
            ...material,
            availableStock: (material.availableStock || 0) - quantity,
            reservedStock: (material.reservedStock || 0) + quantity
          };

          // Coordinate both operations: update material stock AND assign to work order
          return this.mockDatabaseService.updateMaterial(materialId, updatedMaterial).pipe(
            switchMap(() => this.workOrderService.assignMaterial(workOrderId, materialAssignment))
          );
        })
      );
    } else {
      return this.apiService.post<boolean>('materials/assign', {
        materialId,
        workOrderId,
        quantity
      }).pipe(
        map(response => response.data)
      );
    }
  }

  /**
   * Track material usage in a work order
   * This updates the material stock when used in work orders
   */
  trackMaterialUsage(materialId: string, workOrderId: string, quantity: number): Observable<boolean> {
    if (environment.useMockData) {
      // Get the material first
      return this.mockDatabaseService.getMaterialById(materialId).pipe(
        switchMap(material => {
          if (!material) {
            throw new Error(`Material with id ${materialId} not found`);
          }

          // Update material stock (reduce reserved and total stock)
          const updatedMaterial = {
            ...material,
            totalStock: (material.totalStock || 0) - quantity,
            reservedStock: Math.max(0, (material.reservedStock || 0) - quantity),
            availableStock: Math.max(0, (material.availableStock || 0) - quantity),
            lastUsedAt: new Date()
          };

          return this.mockDatabaseService.updateMaterial(materialId, updatedMaterial).pipe(
            map(() => true)
          );
        })
      );
    } else {
      return this.apiService.post<boolean>('materials/usage', {
        materialId,
        workOrderId,
        quantity
      }).pipe(
        map(response => response.data)
      );
    }
  }

  /**
   * Get materials that are low in stock
   */
  getLowStockMaterials(): Observable<BaseMaterial[]> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getMaterials().pipe(
        map(materials => materials.filter(m => 
          m.totalStock !== undefined && 
          m.minimumStock !== undefined && 
          m.totalStock <= m.minimumStock
        ))
      );
    } else {
      return this.apiService.get<BaseMaterial[]>('materials/low-stock').pipe(
        map(response => response.data)
      );
    }
  }

  /**
   * Get materials that are out of stock
   */
  getOutOfStockMaterials(): Observable<BaseMaterial[]> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getMaterials().pipe(
        map(materials => materials.filter(m => 
          !m.totalStock || m.totalStock === 0
        ))
      );
    } else {
      return this.apiService.get<BaseMaterial[]>('materials/out-of-stock').pipe(
        map(response => response.data)
      );
    }
  }

  /**
   * Update material stock levels
   */
  updateMaterialStock(materialId: string, newStock: number, adjustmentType: 'increase' | 'decrease' | 'set'): Observable<BaseMaterial> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getMaterialById(materialId).pipe(
        switchMap(material => {
          if (!material) {
            throw new Error(`Material with id ${materialId} not found`);
          }

          let updatedStock = material.totalStock || 0;
          
          switch (adjustmentType) {
            case 'increase':
              updatedStock += newStock;
              break;
            case 'decrease':
              updatedStock = Math.max(0, updatedStock - newStock);
              break;
            case 'set':
              updatedStock = newStock;
              break;
          }

          const updatedMaterial = {
            ...material,
            totalStock: updatedStock,
            availableStock: Math.max(0, updatedStock - (material.reservedStock || 0))
          };

          return this.mockDatabaseService.updateMaterial(materialId, updatedMaterial);
        })
      );
    } else {
      return this.apiService.put<BaseMaterial>(`materials/${materialId}/stock`, {
        newStock,
        adjustmentType
      }).pipe(
        map(response => response.data)
      );
    }
  }
}
