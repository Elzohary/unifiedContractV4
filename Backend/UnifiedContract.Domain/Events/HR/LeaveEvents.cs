using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Domain.Events.HR
{
    public class LeaveRequestedEvent : DomainEvent
    {
        public Leave Leave { get; }

        public LeaveRequestedEvent(Leave leave)
        {
            Leave = leave;
        }
    }

    public class LeaveApprovedEvent : DomainEvent
    {
        public Leave Leave { get; }

        public LeaveApprovedEvent(Leave leave)
        {
            Leave = leave;
        }
    }

    public class LeaveRejectedEvent : DomainEvent
    {
        public Leave Leave { get; }

        public LeaveRejectedEvent(Leave leave)
        {
            Leave = leave;
        }
    }

    public class LeaveCancelledEvent : DomainEvent
    {
        public Leave Leave { get; }

        public LeaveCancelledEvent(Leave leave)
        {
            Leave = leave;
        }
    }

    public class LeaveUpdatedEvent : DomainEvent
    {
        public Leave Leave { get; }

        public LeaveUpdatedEvent(Leave leave)
        {
            Leave = leave;
        }
    }
} 