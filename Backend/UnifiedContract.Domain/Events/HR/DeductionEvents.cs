using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Domain.Events.HR
{
    public class DeductionAddedEvent : DomainEvent
    {
        public Deduction Deduction { get; }

        public DeductionAddedEvent(Deduction deduction)
        {
            Deduction = deduction;
        }
    }

    public class DeductionUpdatedEvent : DomainEvent
    {
        public Deduction Deduction { get; }

        public DeductionUpdatedEvent(Deduction deduction)
        {
            Deduction = deduction;
        }
    }

    public class DeductionTerminatedEvent : DomainEvent
    {
        public Deduction Deduction { get; }

        public DeductionTerminatedEvent(Deduction deduction)
        {
            Deduction = deduction;
        }
    }
} 