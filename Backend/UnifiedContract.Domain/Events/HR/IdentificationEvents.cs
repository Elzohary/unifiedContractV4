using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Domain.Events.HR
{
    public class IdentificationAddedEvent : DomainEvent
    {
        public Identification Identification { get; }

        public IdentificationAddedEvent(Identification identification)
        {
            Identification = identification;
        }
    }

    public class IdentificationUpdatedEvent : DomainEvent
    {
        public Identification Identification { get; }

        public IdentificationUpdatedEvent(Identification identification)
        {
            Identification = identification;
        }
    }

    public class IdentificationVerifiedEvent : DomainEvent
    {
        public Identification Identification { get; }

        public IdentificationVerifiedEvent(Identification identification)
        {
            Identification = identification;
        }
    }

    public class IdentificationRejectedEvent : DomainEvent
    {
        public Identification Identification { get; }

        public IdentificationRejectedEvent(Identification identification)
        {
            Identification = identification;
        }
    }
} 