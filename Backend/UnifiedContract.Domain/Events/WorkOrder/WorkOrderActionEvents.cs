using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Domain.Events.WorkOrder
{
    public class WorkOrderActionCreatedEvent : DomainEvent
    {
        public WorkOrderAction Action { get; }

        public WorkOrderActionCreatedEvent(WorkOrderAction action)
        {
            Action = action;
        }
    }
    
    public class WorkOrderActionUpdatedEvent : DomainEvent
    {
        public WorkOrderAction Action { get; }

        public WorkOrderActionUpdatedEvent(WorkOrderAction action)
        {
            Action = action;
        }
    }
    
    public class WorkOrderActionCompletedEvent : DomainEvent
    {
        public WorkOrderAction Action { get; }
        public DateTime CompletionDate { get; }

        public WorkOrderActionCompletedEvent(WorkOrderAction action, DateTime completionDate)
        {
            Action = action;
            CompletionDate = completionDate;
        }
    }
    
    public class WorkOrderActionAssignedEvent : DomainEvent
    {
        public WorkOrderAction Action { get; }
        public Guid AssigneeId { get; }

        public WorkOrderActionAssignedEvent(WorkOrderAction action, Guid assigneeId)
        {
            Action = action;
            AssigneeId = assigneeId;
        }
    }
} 