using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Enums;

namespace UnifiedContract.Domain.Entities.WorkOrder
{
    public class WorkOrderAction : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ActionStatus Status { get; set; }
        public WorkOrderPriority Priority { get; set; }
        public Guid AssignedToId { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public Guid? CompletedById { get; set; }
        public Guid WorkOrderId { get; set; }

        // Navigation properties will be defined in the configurations
    }
} 