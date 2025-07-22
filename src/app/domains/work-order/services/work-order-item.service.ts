import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { Iitem } from '../models/work-order-item.model';
import { environment } from '../../../../environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class WorkOrderItemService {
  private mockItems: Iitem[] = [
    {
      id: '1',
      itemNumber: 'WOI-001',
      lineType: 'Description',
      shortDescription: 'Concrete Mix',
      longDescription: 'Concrete Mix - Grade 30 for foundation work',
      UOM: 'm³',
      currency: 'SAR',
      unitPrice: 100,
      paymentType: 'Fixed Price',
      managementArea: 'Western Region'
    },
    {
      id: '2',
      itemNumber: 'WOI-002',
      lineType: 'Description',
      shortDescription: 'Steel Bars',
      longDescription: 'Steel Reinforcement Bars - 12mm for structural support',
      UOM: 'ton',
      currency: 'SAR',
      unitPrice: 120,
      paymentType: 'Fixed Price',
      managementArea: 'Western Region'
    },
    {
      id: '3',
      itemNumber: 'WOI-003',
      lineType: 'Description',
      shortDescription: 'Electrical Wiring',
      longDescription: 'Electrical Wiring - 2.5mm² for power distribution',
      UOM: 'm',
      currency: 'SAR',
      unitPrice: 1250,
      paymentType: 'Fixed Price',
      managementArea: 'Western Region'
    },
    {
      id: '4',
      itemNumber: 'WOI-004',
      lineType: 'Description',
      shortDescription: 'PVC Pipes',
      longDescription: 'PVC Pipes - 50mm for plumbing installation',
      UOM: 'm',
      currency: 'SAR',
      unitPrice: 300,
      paymentType: 'Fixed Price',
      managementArea: 'Western Region'
    },
    {
      id: '5',
      itemNumber: 'WOI-005',
      lineType: 'Description',
      shortDescription: 'Interior Paint',
      longDescription: 'Paint - Interior White for wall finishing',
      UOM: 'L',
      currency: 'SAR',
      unitPrice: 710,
      paymentType: 'Fixed Price',
      managementArea: 'Western Region'
    },
    {
      id: '6',
      itemNumber: 'WOI-005',
      lineType: 'Description',
      shortDescription: 'Interior Paint',
      longDescription: 'Paint - Interior White for wall finishing',
      UOM: 'L',
      currency: 'SAR',
      unitPrice: 582,
      paymentType: 'Fixed Price',
      managementArea: 'Eastern Region'
    }
  ];

  // Subject to notify subscribers when items change
  private itemsSubject = new BehaviorSubject<Iitem[]>(this.mockItems);
  items$ = this.itemsSubject.asObservable();

  constructor(private http: HttpClient) {}

  getItems(workOrderId: string): Observable<Iitem[]> {
    if (environment.useMockData) {
      return this.items$;
    } else {
      // Call backend API: GET /api/work-orders/{id}/items
      return this.http.get<any>(`${environment.apiUrl}/work-orders/${workOrderId}/items`).pipe(
        // The backend returns ApiResponse<{ data: WorkOrderItem[] }>
        map(response => (response.data || []).map((item: any) => ({
          id: item.id,
          itemNumber: item.itemNumber,
          lineType: 'Description', // or map if available
          shortDescription: item.description,
          longDescription: item.description,
          UOM: item.unit,
          currency: 'SAR', // or map if available
          unitPrice: item.unitPrice,
          paymentType: 'Fixed Price', // or map if available
          managementArea: '', // or map if available
        } as Iitem)))
      );
    }
  }

  getAllItems(): Observable<Iitem[]> {
    return this.http.get<any>(`${environment.apiUrl}/work-order-items`).pipe(
      map(response => (response.data || []).map((item: any) => ({
        id: item.id,
        itemNumber: item.itemNumber,
        lineType: 'Description',
        shortDescription: item.description,
        longDescription: item.description,
        UOM: item.unit,
        currency: 'SAR',
        unitPrice: item.unitPrice,
        paymentType: 'Fixed Price',
        managementArea: '',
      } as Iitem)))
    );
  }

  getItemById(id: string): Observable<Iitem | null> {
    const item = this.mockItems.find(item => item.id === id);
    return of(item || null);
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
    };
    return this.http.post<any>(`${environment.apiUrl}/work-order-items`, payload)
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

  updateItem(id: string, item: Partial<Iitem>): Observable<Iitem> {
    const index = this.mockItems.findIndex(i => i.id === id);
    if (index !== -1) {
      this.mockItems[index] = {
        ...this.mockItems[index],
        ...item
      };
      this.itemsSubject.next(this.mockItems);
      return of(this.mockItems[index]);
    }
    return of({} as Iitem);
  }

  deleteItem(id: string): Observable<boolean> {
    const index = this.mockItems.findIndex(item => item.id === id);
    if (index !== -1) {
      this.mockItems.splice(index, 1);
      this.itemsSubject.next(this.mockItems);
      return of(true);
    }
    return of(false);
  }

  // Create items from a work order
  createItemsFromWorkOrder(workOrderItems: any[], workOrderId: string): Observable<Iitem[]> {
    if (!workOrderItems || workOrderItems.length === 0) {
      return of([]);
    }

    const newItems: Iitem[] = [];

    workOrderItems.forEach((workOrderItem, index) => {
      // Create a new item based on the work order item
      const newItem: Iitem = {
        id: (this.mockItems.length + index + 1).toString(),
        itemNumber: `WOI-${workOrderId}-${String(index + 1).padStart(3, '0')}`,
        lineType: 'Description',
        shortDescription: workOrderItem.name || `Item ${index + 1}`,
        longDescription: workOrderItem.description || '',
        UOM: workOrderItem.uom || '',
        currency: workOrderItem.currency || 'SAR',
        paymentType: workOrderItem.paymentType || 'Fixed Price',
        managementArea: workOrderItem.managementArea || '',
        unitPrice: workOrderItem.unitPrice || 0
      };

      this.mockItems.push(newItem);
      newItems.push(newItem);
    });

    // Notify subscribers of the changes
    this.itemsSubject.next(this.mockItems);

    return of(newItems);
  }

  assignItemToWorkOrder(workOrderId: string, item: Iitem): Observable<any> {
    const quantity = (item as any).estimatedQuantity || 1;
    const payload = {
      itemNumber: item.itemNumber,
      description: item.shortDescription || item.longDescription || '',
      unit: item.UOM,
      unitPrice: item.unitPrice,
      estimatedQuantity: quantity,
      estimatedPrice: quantity * item.unitPrice,
      estimatedPriceWithVAT: quantity * item.unitPrice * 1.15,
      actualQuantity: 0,
      actualPrice: 0,
      actualPriceWithVAT: 0,
      reasonForFinalQuantity: '',
    };
    return this.http.post(`${environment.apiUrl}/work-orders/${workOrderId}/items`, payload);
  }
}
