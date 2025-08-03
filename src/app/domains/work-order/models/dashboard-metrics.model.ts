export interface DashboardMetrics {
  completedWorkOrders: number;
  ongoingWorkOrders: number;
  totalPartiallyInvoicedAmount: number;
  totalInvoicedAmount: number;
  totalRemainingAmountToBeInvoiced: number;
  totalExpectedAmount: number;
  workOrdersWithoutPO: number;
  totalWorkOrders: number;
  pendingWorkOrders: number;
  overdueWorkOrders: number;
  cancelledWorkOrders: number;
} 