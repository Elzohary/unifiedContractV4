import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Iitem } from '../models/work-order-item.model';
import { environment } from '../../../../environments/environment';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class WorkOrderItemService {
  constructor(private http: HttpClient) {}

  getItems(workOrderId: string): Observable<Iitem[]> {
    // Call backend API: GET /api/work-orders/{id}/items
    return this.http.get<any>(`${environment.apiUrl}/work-orders/${workOrderId}/items`).pipe(
      // The backend returns ApiResponse<{ data: WorkOrderItem[] }>
      map(response => (response.data || []).map((item: any) => ({
        id: item.id,
        itemNumber: item.itemNumber,
        lineType: 'Description',
        shortDescription: item.description,
        longDescription: item.description,
        UOM: item.unit,
        currency: item.currency || 'SAR',
        unitPrice: item.unitPrice,
        paymentType: item.paymentType || 'Fixed Price',
        managementArea: item.managementArea || '',
      } as Iitem)))
    );
  }

  getAllItems(): Observable<Iitem[]> {
    // For the items list page, we need to get all available items from the master items table
    // This should call the endpoint that returns all items from dbo.items (master catalog)
    return this.http.get<any>(`${environment.apiUrl}/work-orders/available-items`).pipe(
      map(response => (response.data || []).map((item: any) => ({
        id: item.id,
        itemNumber: item.itemNumber,
        lineType: 'Description',
        shortDescription: item.description,
        longDescription: item.description,
        UOM: item.unit,
        currency: item.currency || 'SAR',
        unitPrice: item.unitPrice,
        paymentType: item.paymentType || 'Fixed Price',
        managementArea: item.managementArea || '',
      } as Iitem)))
    );
  }

  getAvailableItems(): Observable<Iitem[]> {
    // For the work order form dropdown, get all available items
    console.log('Making API call to:', `${environment.apiUrl}/work-orders/available-items`);
    return this.http.get<any>(`${environment.apiUrl}/work-orders/available-items`).pipe(
      map(response => {
        console.log('API response:', response);
        return (response.data || []).map((item: any) => ({
          id: item.id,
          itemNumber: item.itemNumber,
          lineType: 'Description',
          shortDescription: item.description,
          longDescription: item.description,
          UOM: item.unit,
          currency: item.currency || 'SAR',
          unitPrice: item.unitPrice,
          paymentType: item.paymentType || 'Fixed Price',
          managementArea: item.managementArea || '',
        } as Iitem));
      })
    );
  }

  getAvailableItemsForWorkOrder(workOrderId: string): Observable<Iitem[]> {
    // For the work order assignment dialog, get available items for a specific work order
    console.log('Making API call to:', `${environment.apiUrl}/work-orders/${workOrderId}/available-items`);
    return this.http.get<any>(`${environment.apiUrl}/work-orders/${workOrderId}/available-items`).pipe(
      map(response => {
        console.log('API response for work order items:', response);
        return (response.data || []).map((item: any) => ({
          id: item.id,
          itemNumber: item.itemNumber,
          lineType: 'Description',
          shortDescription: item.description,
          longDescription: item.description,
          UOM: item.unit,
          currency: item.currency || 'SAR',
          unitPrice: item.unitPrice,
          paymentType: item.paymentType || 'Fixed Price',
          managementArea: item.managementArea || '',
        } as Iitem));
      })
    );
  }

  getItemById(id: string): Observable<Iitem | null> {
    // Call backend API to get master item by ID
    return this.http.get<any>(`${environment.apiUrl}/work-orders/master-items/${id}`).pipe(
      map(response => {
        if (response.data) {
          const item = response.data;
          return {
            id: item.id,
            itemNumber: item.itemNumber,
            lineType: 'Description',
            shortDescription: item.description,
            longDescription: item.description,
            UOM: item.unit,
            currency: item.currency || 'SAR',
            unitPrice: item.unitPrice,
            paymentType: item.paymentType || 'Fixed Price',
            managementArea: item.managementArea || '',
          } as Iitem;
        }
        return null;
      })
    );
  }

  createItem(item: Partial<Iitem>): Observable<Iitem> {
    const payload = {
      itemNumber: item.itemNumber,
      description: item.shortDescription || item.longDescription || '',
      unit: item.UOM,
      unitPrice: item.unitPrice,
      paymentType: item.paymentType,
      managementArea: item.managementArea,
      currency: item.currency,
      clientId: item.clientId || '00000000-0000-0000-0000-000000000000', // Default client ID
    };
    
    console.log('=== DEBUG: createItem payload ===');
    console.log('Item data:', item);
    console.log('Payload being sent:', payload);
    
    return this.http.post<any>(`${environment.apiUrl}/work-orders/master-items`, payload)
      .pipe(
        map(response => {
          console.log('=== DEBUG: createItem response ===');
          console.log('Response:', response);
          return {
            id: response.data.id,
            itemNumber: response.data.itemNumber,
            lineType: 'Description',
            shortDescription: response.data.description,
            longDescription: response.data.description,
            UOM: response.data.unit,
            currency: response.data.currency || 'SAR',
            unitPrice: response.data.unitPrice,
            paymentType: response.data.paymentType || 'Fixed Price',
            managementArea: response.data.managementArea || '',
          } as Iitem;
        }),
        catchError(error => {
          console.error('=== DEBUG: createItem error ===');
          console.error('Error response:', error);
          console.error('Error status:', error.status);
          console.error('Error message:', error.message);
          if (error.error) {
            console.error('Error details:', error.error);
          }
          throw error;
        })
      );
  }

  updateItem(id: string, item: Partial<Iitem>): Observable<Iitem> {
    const payload = {
      itemNumber: item.itemNumber,
      description: item.shortDescription || item.longDescription || '',
      unit: item.UOM,
      unitPrice: item.unitPrice,
      paymentType: item.paymentType,
      managementArea: item.managementArea,
      currency: item.currency,
      isActive: item.isActive !== false, // Default to true if not specified
      clientId: item.clientId || '00000000-0000-0000-0000-000000000000', // Default client ID
    };
    return this.http.put<any>(`${environment.apiUrl}/work-orders/master-items/${id}`, payload)
      .pipe(
        map(response => ({
          id: response.data.id,
          itemNumber: response.data.itemNumber,
          lineType: 'Description',
          shortDescription: response.data.description,
          longDescription: response.data.description,
          UOM: response.data.unit,
          currency: response.data.currency || 'SAR',
          unitPrice: response.data.unitPrice,
          paymentType: response.data.paymentType || 'Fixed Price',
          managementArea: response.data.managementArea || '',
        } as Iitem))
      );
  }

  deleteItem(id: string): Observable<boolean> {
    return this.http.delete<any>(`${environment.apiUrl}/work-orders/master-items/${id}`)
      .pipe(
        map(response => true)
      );
  }

  // Assign items to a work order
  createItemsFromWorkOrder(workOrderItems: any[], workOrderId: string): Observable<Iitem[]> {
    console.log('=== DEBUG: createItemsFromWorkOrder called ===');
    console.log('Work order items:', workOrderItems);
    console.log('Work order ID:', workOrderId);
    
    if (!workOrderItems || workOrderItems.length === 0) {
      console.log('=== DEBUG: No items to assign ===');
      return of([]);
    }

    const assignedItems: Iitem[] = [];

    // Assign items one by one to the work order
    workOrderItems.forEach((workOrderItem, index) => {
      console.log(`=== DEBUG: Processing item ${index + 1} ===`);
      console.log('Item data:', workOrderItem);
      
      // Create the item object with the estimated quantity
      const itemToAssign = {
        ...workOrderItem,
        estimatedQuantity: workOrderItem.estimatedQuantity || 1
      };

      console.log('Item to assign:', itemToAssign);

      this.assignItemToWorkOrder(workOrderId, itemToAssign).subscribe({
        next: (assignedItem) => {
          console.log(`=== DEBUG: Successfully assigned item ${index + 1} ===`);
          console.log('Assigned item response:', assignedItem);
          assignedItems.push(assignedItem);
        },
        error: (error) => {
          console.error(`=== DEBUG: Error assigning item ${index + 1} ===`, error);
        }
      });
    });

    return of(assignedItems);
  }

  assignItemToWorkOrder(workOrderId: string, item: Iitem): Observable<any> {
    console.log('=== DEBUG: assignItemToWorkOrder called ===');
    console.log('Work order ID:', workOrderId);
    console.log('Item:', item);
    
    const quantity = (item as any).estimatedQuantity || 1;
    const payload = {
      itemId: item.id, // Use the item ID from the master catalog
      estimatedQuantity: quantity,
      reasonForFinalQuantity: ''
    };
    
    console.log('Payload to send:', payload);
    console.log('API URL:', `${environment.apiUrl}/work-orders/${workOrderId}/assign-item`);
    
    return this.http.post(`${environment.apiUrl}/work-orders/${workOrderId}/assign-item`, payload);
  }
}
