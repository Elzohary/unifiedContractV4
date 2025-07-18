using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Enums;

namespace UnifiedContract.Domain.Entities.WorkOrder
{
    public class WorkOrderTask : BaseEntity
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public WorkOrderPriority Priority { get; set; }
        public UnifiedContract.Domain.Enums.TaskStatus Status { get; set; }
        public bool Completed { get; set; }
        public Guid WorkOrderId { get; set; }
        public Guid? ConfirmedById { get; set; }

        // Navigation properties will be defined in the configurations
        
        // Required by EF Core
        private WorkOrderTask() { }
        
        public WorkOrderTask(
            string title,
            string description,
            Guid workOrderId,
            WorkOrderPriority priority = WorkOrderPriority.Medium,
            UnifiedContract.Domain.Enums.TaskStatus status = UnifiedContract.Domain.Enums.TaskStatus.Pending)
        {
            Title = title;
            Description = description;
            WorkOrderId = workOrderId;
            Priority = priority;
            Status = status;
            Completed = false;
        }
    }
} 