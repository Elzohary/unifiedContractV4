using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Domain.Events.HR
{
    public class EducationAddedEvent : DomainEvent
    {
        public Education Education { get; }

        public EducationAddedEvent(Education education)
        {
            Education = education;
        }
    }

    public class EducationUpdatedEvent : DomainEvent
    {
        public Education Education { get; }

        public EducationUpdatedEvent(Education education)
        {
            Education = education;
        }
    }

    public class EducationCompletedEvent : DomainEvent
    {
        public Education Education { get; }

        public EducationCompletedEvent(Education education)
        {
            Education = education;
        }
    }

    public class EducationVerifiedEvent : DomainEvent
    {
        public Education Education { get; }

        public EducationVerifiedEvent(Education education)
        {
            Education = education;
        }
    }

    public class EducationRejectedEvent : DomainEvent
    {
        public Education Education { get; }

        public EducationRejectedEvent(Education education)
        {
            Education = education;
        }
    }
} 