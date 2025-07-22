using System;

namespace UnifiedContract.Domain.Common
{
    public abstract class DomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredOn { get; }
    }
} 