using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Domain.Events.HR
{
    public class AllowanceAddedEvent : DomainEvent
    {
        public Allowance Allowance { get; }

        public AllowanceAddedEvent(Allowance allowance)
        {
            Allowance = allowance;
        }
    }

    public class AllowanceUpdatedEvent : DomainEvent
    {
        public Allowance Allowance { get; }

        public AllowanceUpdatedEvent(Allowance allowance)
        {
            Allowance = allowance;
        }
    }

    public class AllowanceTerminatedEvent : DomainEvent
    {
        public Allowance Allowance { get; }

        public AllowanceTerminatedEvent(Allowance allowance)
        {
            Allowance = allowance;
        }
    }
} 