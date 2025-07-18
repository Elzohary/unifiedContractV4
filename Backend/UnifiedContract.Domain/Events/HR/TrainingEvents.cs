using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;
using System;

namespace UnifiedContract.Domain.Events.HR
{
    public class TrainingAddedEvent : DomainEvent
    {
        public Training Training { get; }

        public TrainingAddedEvent(Training training)
        {
            Training = training;
        }
    }

    public class TrainingUpdatedEvent : DomainEvent
    {
        public Training Training { get; }

        public TrainingUpdatedEvent(Training training)
        {
            Training = training;
        }
    }

    public class TrainingStartedEvent : DomainEvent
    {
        public Training Training { get; }

        public TrainingStartedEvent(Training training)
        {
            Training = training;
        }
    }

    public class TrainingCompletedEvent : DomainEvent
    {
        public Training Training { get; }

        public TrainingCompletedEvent(Training training)
        {
            Training = training;
        }
    }

    public class TrainingCancelledEvent : DomainEvent
    {
        public Training Training { get; }

        public TrainingCancelledEvent(Training training)
        {
            Training = training;
        }
    }

    public class TrainingFailedEvent : DomainEvent
    {
        public Training Training { get; }

        public TrainingFailedEvent(Training training)
        {
            Training = training;
        }
    }

    public class TrainingFeedbackAddedEvent : DomainEvent
    {
        public Training Training { get; }

        public TrainingFeedbackAddedEvent(Training training)
        {
            Training = training;
        }
    }

    public class TrainingCertificateAddedEvent : DomainEvent
    {
        public Training Training { get; }

        public TrainingCertificateAddedEvent(Training training)
        {
            Training = training;
        }
    }

    public class TrainingScoreUpdatedEvent : DomainEvent
    {
        public Training Training { get; }

        public TrainingScoreUpdatedEvent(Training training)
        {
            Training = training;
        }
    }

    public class TrainingDeletedEvent : DomainEvent
    {
        public Training Training { get; }
        public string DeletionReason { get; }

        public TrainingDeletedEvent(Training training, string deletionReason)
        {
            Training = training;
            DeletionReason = deletionReason;
        }
    }

    public class TrainingRescheduledEvent : DomainEvent
    {
        public Training Training { get; }
        public DateTime PreviousStartDate { get; }
        public DateTime PreviousEndDate { get; }
        public string RescheduleReason { get; }

        public TrainingRescheduledEvent(Training training, DateTime previousStartDate, DateTime previousEndDate, string rescheduleReason)
        {
            Training = training;
            PreviousStartDate = previousStartDate;
            PreviousEndDate = previousEndDate;
            RescheduleReason = rescheduleReason;
        }
    }

    public class TrainingLocationChangedEvent : DomainEvent
    {
        public Training Training { get; }
        public string PreviousLocation { get; }
        public string NewLocation { get; }

        public TrainingLocationChangedEvent(Training training, string previousLocation, string newLocation)
        {
            Training = training;
            PreviousLocation = previousLocation;
            NewLocation = newLocation;
        }
    }

    public class TrainingInstructorChangedEvent : DomainEvent
    {
        public Training Training { get; }
        public string PreviousInstructor { get; }
        public string NewInstructor { get; }

        public TrainingInstructorChangedEvent(Training training, string previousInstructor, string newInstructor)
        {
            Training = training;
            PreviousInstructor = previousInstructor;
            NewInstructor = newInstructor;
        }
    }
} 