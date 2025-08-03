import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, combineLatest } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { WorkOrder, WorkOrderStatus } from '../models/work-order.model';
import { WorkOrderService } from '../services/work-order.service';
import { DashboardMetrics } from '../models/dashboard-metrics.model';

export interface WorkOrderDashboardStats {
  total: number;
  active: number;
  pending: number;
  overdue: number;
  completed: number;
  cancelled: number;
  trends: { label: string; value: number; trend: number }[];
  recent: WorkOrder[];
  // New financial metrics
  totalPartiallyInvoicedAmount: number;
  totalInvoicedAmount: number;
  totalRemainingAmountToBeInvoiced: number;
  totalExpectedAmount: number;
  workOrdersWithoutPO: number;
}

@Injectable({ providedIn: 'root' })
export class WorkOrderDashboardViewModel {
  // Expose dashboard stats as observable
  public stats$: Observable<WorkOrderDashboardStats>;

  constructor(private workOrderService: WorkOrderService) {
    // Get dashboard metrics from database
    const dashboardMetrics$ = this.workOrderService.getDashboardMetrics().pipe(
      map((response: any) => response.data as DashboardMetrics),
      catchError(error => {
        console.error('Error fetching dashboard metrics:', error);
        // Return default values if API fails
        return [{
          completedWorkOrders: 0,
          ongoingWorkOrders: 0,
          totalPartiallyInvoicedAmount: 0,
          totalInvoicedAmount: 0,
          totalRemainingAmountToBeInvoiced: 0,
          totalExpectedAmount: 0,
          workOrdersWithoutPO: 0,
          totalWorkOrders: 0,
          pendingWorkOrders: 0,
          overdueWorkOrders: 0,
          cancelledWorkOrders: 0
        }];
      })
    );

    // Combine database metrics with work orders for trends and recent data
    this.stats$ = combineLatest([
      dashboardMetrics$,
      this.workOrderService.workOrders$
    ]).pipe(
      map(([metrics, workOrders]: [DashboardMetrics, WorkOrder[]]) => {
        const now = new Date();

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
          total: metrics.totalWorkOrders,
          active: metrics.ongoingWorkOrders,
          pending: metrics.pendingWorkOrders,
          overdue: metrics.overdueWorkOrders,
          completed: metrics.completedWorkOrders,
          cancelled: metrics.cancelledWorkOrders,
          trends: [
            { label: 'Last 30 days', value: countLast30, trend },
            { label: 'Previous 30 days', value: countPrev30, trend: 0 }
          ],
          recent,
          // Financial metrics from database
          totalPartiallyInvoicedAmount: metrics.totalPartiallyInvoicedAmount,
          totalInvoicedAmount: metrics.totalInvoicedAmount,
          totalRemainingAmountToBeInvoiced: metrics.totalRemainingAmountToBeInvoiced,
          totalExpectedAmount: metrics.totalExpectedAmount,
          workOrdersWithoutPO: metrics.workOrdersWithoutPO
        };
      })
    );
  }
} 