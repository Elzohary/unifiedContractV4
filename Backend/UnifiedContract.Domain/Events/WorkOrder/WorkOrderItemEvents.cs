using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Domain.Events.WorkOrder
{
    public class WorkOrderItemCreatedEvent : DomainEvent
    {
        public WorkOrderItem Item { get; }

        public WorkOrderItemCreatedEvent(WorkOrderItem item)
        {
            Item = item;
        }
    }
    
    public class WorkOrderItemUpdatedEvent : DomainEvent
    {
        public WorkOrderItem Item { get; }

        public WorkOrderItemUpdatedEvent(WorkOrderItem item)
        {
            Item = item;
        }
    }
    
    public class WorkOrderItemRemovedEvent : DomainEvent
    {
        public WorkOrderItem Item { get; }

        public WorkOrderItemRemovedEvent(WorkOrderItem item)
        {
            Item = item;
        }
    }
    
    public class WorkOrderItemQuantityChangedEvent : DomainEvent
    {
        public WorkOrderItem Item { get; }
        public double PreviousQuantity { get; }
        public double NewQuantity { get; }

        public WorkOrderItemQuantityChangedEvent(WorkOrderItem item, double previousQuantity, double newQuantity)
        {
            Item = item;
            PreviousQuantity = previousQuantity;
            NewQuantity = newQuantity;
        }
    }
} 