using System;

namespace UnifiedContract.Application.DTOs
{
    public class DashboardMetricsDto
    {
        public int CompletedWorkOrders { get; set; }
        public int OngoingWorkOrders { get; set; }
        public decimal TotalPartiallyInvoicedAmount { get; set; }
        public decimal TotalInvoicedAmount { get; set; }
        public decimal TotalRemainingAmountToBeInvoiced { get; set; }
        public decimal TotalExpectedAmount { get; set; }
        public int WorkOrdersWithoutPO { get; set; }
        public int TotalWorkOrders { get; set; }
        public int PendingWorkOrders { get; set; }
        public int OverdueWorkOrders { get; set; }
        public int CancelledWorkOrders { get; set; }
    }
} 