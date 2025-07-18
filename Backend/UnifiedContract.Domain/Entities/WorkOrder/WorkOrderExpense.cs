using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Enums;

namespace UnifiedContract.Domain.Entities.WorkOrder
{
    public class WorkOrderExpense : BaseEntity
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public Guid SubmittedById { get; set; }
        public ExpenseStatus Status { get; set; }
        public Guid? ApprovedById { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string Receipt { get; set; }
        public Guid WorkOrderId { get; set; }

        // Navigation properties will be defined in the configurations
    }
} 