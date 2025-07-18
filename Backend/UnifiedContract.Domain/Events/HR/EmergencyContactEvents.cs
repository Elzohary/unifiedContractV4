using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Domain.Events.HR
{
    public class EmergencyContactAddedEvent : DomainEvent
    {
        public EmergencyContact EmergencyContact { get; }

        public EmergencyContactAddedEvent(EmergencyContact emergencyContact)
        {
            EmergencyContact = emergencyContact;
        }
    }

    public class EmergencyContactUpdatedEvent : DomainEvent
    {
        public EmergencyContact EmergencyContact { get; }

        public EmergencyContactUpdatedEvent(EmergencyContact emergencyContact)
        {
            EmergencyContact = emergencyContact;
        }
    }

    public class EmergencyContactSetAsPrimaryEvent : DomainEvent
    {
        public EmergencyContact EmergencyContact { get; }

        public EmergencyContactSetAsPrimaryEvent(EmergencyContact emergencyContact)
        {
            EmergencyContact = emergencyContact;
        }
    }

    public class EmergencyContactUnsetAsPrimaryEvent : DomainEvent
    {
        public EmergencyContact EmergencyContact { get; }

        public EmergencyContactUnsetAsPrimaryEvent(EmergencyContact emergencyContact)
        {
            EmergencyContact = emergencyContact;
        }
    }
} 