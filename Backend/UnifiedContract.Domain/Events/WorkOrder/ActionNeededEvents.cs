using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Domain.Events.WorkOrder
{
    public class ActionNeededCreatedEvent : DomainEvent
    {
        public ActionNeeded ActionNeeded { get; }

        public ActionNeededCreatedEvent(ActionNeeded actionNeeded)
        {
            ActionNeeded = actionNeeded;
        }
    }
    
    public class ActionNeededUpdatedEvent : DomainEvent
    {
        public ActionNeeded ActionNeeded { get; }

        public ActionNeededUpdatedEvent(ActionNeeded actionNeeded)
        {
            ActionNeeded = actionNeeded;
        }
    }
    
    public class ActionNeededResolvedEvent : DomainEvent
    {
        public ActionNeeded ActionNeeded { get; }
        public DateTime ResolutionDate { get; }

        public ActionNeededResolvedEvent(ActionNeeded actionNeeded, DateTime resolutionDate)
        {
            ActionNeeded = actionNeeded;
            ResolutionDate = resolutionDate;
        }
    }
    
    public class ActionNeededPriorityChangedEvent : DomainEvent
    {
        public ActionNeeded ActionNeeded { get; }
        public string PreviousPriority { get; }
        public string NewPriority { get; }

        public ActionNeededPriorityChangedEvent(ActionNeeded actionNeeded, string previousPriority, string newPriority)
        {
            ActionNeeded = actionNeeded;
            PreviousPriority = previousPriority;
            NewPriority = newPriority;
        }
    }
} 