using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Domain.Events.WorkOrder
{
    public class WorkOrderCreatedEvent : DomainEvent
    {
        public Entities.WorkOrder.WorkOrder WorkOrder { get; }

        public WorkOrderCreatedEvent(Entities.WorkOrder.WorkOrder workOrder)
        {
            WorkOrder = workOrder;
        }
    }
    
    public class WorkOrderUpdatedEvent : DomainEvent
    {
        public Entities.WorkOrder.WorkOrder WorkOrder { get; }

        public WorkOrderUpdatedEvent(Entities.WorkOrder.WorkOrder workOrder)
        {
            WorkOrder = workOrder;
        }
    }
    
    public class WorkOrderStatusChangedEvent : DomainEvent
    {
        public Entities.WorkOrder.WorkOrder WorkOrder { get; }
        public string PreviousStatus { get; }
        public string NewStatus { get; }

        public WorkOrderStatusChangedEvent(Entities.WorkOrder.WorkOrder workOrder, string previousStatus, string newStatus)
        {
            WorkOrder = workOrder;
            PreviousStatus = previousStatus;
            NewStatus = newStatus;
        }
    }
    
    public class WorkOrderCompletedEvent : DomainEvent
    {
        public Entities.WorkOrder.WorkOrder WorkOrder { get; }

        public WorkOrderCompletedEvent(Entities.WorkOrder.WorkOrder workOrder)
        {
            WorkOrder = workOrder;
        }
    }
    
    public class WorkOrderCanceledEvent : DomainEvent
    {
        public Entities.WorkOrder.WorkOrder WorkOrder { get; }
        public string CancellationReason { get; }

        public WorkOrderCanceledEvent(Entities.WorkOrder.WorkOrder workOrder, string cancellationReason)
        {
            WorkOrder = workOrder;
            CancellationReason = cancellationReason;
        }
    }
} 