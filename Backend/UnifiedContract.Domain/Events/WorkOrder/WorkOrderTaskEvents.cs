using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Domain.Events.WorkOrder
{
    public class WorkOrderTaskCreatedEvent : DomainEvent
    {
        public WorkOrderTask Task { get; }

        public WorkOrderTaskCreatedEvent(WorkOrderTask task)
        {
            Task = task;
        }
    }
    
    public class WorkOrderTaskUpdatedEvent : DomainEvent
    {
        public WorkOrderTask Task { get; }

        public WorkOrderTaskUpdatedEvent(WorkOrderTask task)
        {
            Task = task;
        }
    }
    
    public class WorkOrderTaskCompletedEvent : DomainEvent
    {
        public WorkOrderTask Task { get; }

        public WorkOrderTaskCompletedEvent(WorkOrderTask task)
        {
            Task = task;
        }
    }
    
    public class WorkOrderTaskReassignedEvent : DomainEvent
    {
        public WorkOrderTask Task { get; }
        public Guid PreviousAssigneeId { get; }
        public Guid NewAssigneeId { get; }

        public WorkOrderTaskReassignedEvent(WorkOrderTask task, Guid previousAssigneeId, Guid newAssigneeId)
        {
            Task = task;
            PreviousAssigneeId = previousAssigneeId;
            NewAssigneeId = newAssigneeId;
        }
    }
} 