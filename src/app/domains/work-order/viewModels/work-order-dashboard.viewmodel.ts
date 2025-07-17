import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import { WorkOrder, WorkOrderStatus } from '../models/work-order.model';
import { WorkOrderService } from '../services/work-order.service';

export interface WorkOrderDashboardStats {
  total: number;
  active: number;
  pending: number;
  overdue: number;
  completed: number;
  cancelled: number;
  trends: { label: string; value: number; trend: number }[];
  recent: WorkOrder[];
}

@Injectable({ providedIn: 'root' })
export class WorkOrderDashboardViewModel {
  // Expose dashboard stats as observable
  public stats$: Observable<WorkOrderDashboardStats>;

  constructor(private workOrderService: WorkOrderService) {
    this.stats$ = this.workOrderService.workOrders$.pipe(
      map((workOrders: WorkOrder[]) => {
        const now = new Date();
        const total = workOrders.length;
        const active = workOrders.filter(wo => wo.details.status === WorkOrderStatus.InProgress).length;
        const pending = workOrders.filter(wo => wo.details.status === WorkOrderStatus.Pending).length;
        const overdue = workOrders.filter(wo => {
          const due = wo.details.dueDate ? new Date(wo.details.dueDate) : null;
          return due && due < now && wo.details.status !== WorkOrderStatus.Completed && wo.details.status !== WorkOrderStatus.Cancelled;
        }).length;
        const completed = workOrders.filter(wo => wo.details.status === WorkOrderStatus.Completed).length;
        const cancelled = workOrders.filter(wo => wo.details.status === WorkOrderStatus.Cancelled).length;

        // Trends: compare last 30 days to previous 30 days
        const getCountInRange = (start: Date, end: Date) =>
          workOrders.filter(wo => {
            const created = wo.details.createdDate ? new Date(wo.details.createdDate) : null;
            return created && created >= start && created < end;
          }).length;
        const today = new Date();
        const last30 = new Date(today); last30.setDate(today.getDate() - 30);
        const prev30 = new Date(today); prev30.setDate(today.getDate() - 60);
        const countLast30 = getCountInRange(last30, today);
        const countPrev30 = getCountInRange(prev30, last30);
        const trend = countPrev30 === 0 ? 0 : Math.round(((countLast30 - countPrev30) / countPrev30) * 100);

        // Recent work orders (last 5, sorted by createdDate desc)
        const recent = [...workOrders].sort((a, b) => {
          const aDate = a.details.createdDate ? new Date(a.details.createdDate).getTime() : 0;
          const bDate = b.details.createdDate ? new Date(b.details.createdDate).getTime() : 0;
          return bDate - aDate;
        }).slice(0, 5);

        return {
          total,
          active,
          pending,
          overdue,
          completed,
          cancelled,
          trends: [
            { label: 'Last 30 days', value: countLast30, trend },
            { label: 'Previous 30 days', value: countPrev30, trend: 0 }
          ],
          recent
        };
      })
    );
  }
} 