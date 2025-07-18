using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.Common;

namespace UnifiedContract.Domain.Events.Common
{
    public class LookupCreatedEvent : DomainEvent
    {
        public Lookup Lookup { get; }

        public LookupCreatedEvent(Lookup lookup)
        {
            Lookup = lookup;
        }
    }
    
    public class LookupUpdatedEvent : DomainEvent
    {
        public Lookup Lookup { get; }

        public LookupUpdatedEvent(Lookup lookup)
        {
            Lookup = lookup;
        }
    }
    
    public class LookupDeactivatedEvent : DomainEvent
    {
        public Lookup Lookup { get; }

        public LookupDeactivatedEvent(Lookup lookup)
        {
            Lookup = lookup;
        }
    }
    
    public class LookupReactivatedEvent : DomainEvent
    {
        public Lookup Lookup { get; }

        public LookupReactivatedEvent(Lookup lookup)
        {
            Lookup = lookup;
        }
    }
} 