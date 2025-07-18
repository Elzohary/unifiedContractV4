using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Domain.Events.HR
{
    public class WorkExperienceAddedEvent : DomainEvent
    {
        public WorkExperience WorkExperience { get; }

        public WorkExperienceAddedEvent(WorkExperience workExperience)
        {
            WorkExperience = workExperience;
        }
    }

    public class WorkExperienceUpdatedEvent : DomainEvent
    {
        public WorkExperience WorkExperience { get; }

        public WorkExperienceUpdatedEvent(WorkExperience workExperience)
        {
            WorkExperience = workExperience;
        }
    }

    public class WorkExperienceSetAsCurrentEvent : DomainEvent
    {
        public WorkExperience WorkExperience { get; }

        public WorkExperienceSetAsCurrentEvent(WorkExperience workExperience)
        {
            WorkExperience = workExperience;
        }
    }

    public class WorkExperienceSetAsFormerEvent : DomainEvent
    {
        public WorkExperience WorkExperience { get; }

        public WorkExperienceSetAsFormerEvent(WorkExperience workExperience)
        {
            WorkExperience = workExperience;
        }
    }

    public class WorkExperienceDeletedEvent : DomainEvent
    {
        public WorkExperience WorkExperience { get; }
        public string DeletionReason { get; }

        public WorkExperienceDeletedEvent(WorkExperience workExperience, string deletionReason)
        {
            WorkExperience = workExperience;
            DeletionReason = deletionReason;
        }
    }

    public class WorkExperienceVerifiedEvent : DomainEvent
    {
        public WorkExperience WorkExperience { get; }
        public string VerifiedBy { get; }
        public DateTime VerificationDate { get; }

        public WorkExperienceVerifiedEvent(WorkExperience workExperience, string verifiedBy, DateTime verificationDate)
        {
            WorkExperience = workExperience;
            VerifiedBy = verifiedBy;
            VerificationDate = verificationDate;
        }
    }

    public class WorkExperienceRejectedEvent : DomainEvent
    {
        public WorkExperience WorkExperience { get; }
        public string RejectedBy { get; }
        public string RejectionReason { get; }
        public DateTime RejectionDate { get; }

        public WorkExperienceRejectedEvent(WorkExperience workExperience, string rejectedBy, string rejectionReason, DateTime rejectionDate)
        {
            WorkExperience = workExperience;
            RejectedBy = rejectedBy;
            RejectionReason = rejectionReason;
            RejectionDate = rejectionDate;
        }
    }
} 