using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Enums;
using UnifiedContract.Domain.Entities.Auth;

namespace UnifiedContract.Domain.Entities.WorkOrder
{
    public class ActionNeeded : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public WorkOrderPriority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string Notes { get; set; }
        public Guid WorkOrderId { get; set; }
        public Guid? AssignedToId { get; set; }
        public Guid? CompletedById { get; set; }
        
        // Navigation properties
        public virtual WorkOrder WorkOrder { get; set; }
        public virtual User AssignedTo { get; set; }
        public virtual User CompletedBy { get; set; }
    }
} 