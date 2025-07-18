using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Domain.Events.HR
{
    public class AttendanceCreatedEvent : DomainEvent
    {
        public Attendance Attendance { get; }

        public AttendanceCreatedEvent(Attendance attendance)
        {
            Attendance = attendance;
        }
    }

    public class AttendanceCheckedInEvent : DomainEvent
    {
        public Attendance Attendance { get; }

        public AttendanceCheckedInEvent(Attendance attendance)
        {
            Attendance = attendance;
        }
    }

    public class AttendanceCheckedOutEvent : DomainEvent
    {
        public Attendance Attendance { get; }

        public AttendanceCheckedOutEvent(Attendance attendance)
        {
            Attendance = attendance;
        }
    }

    public class AttendanceMarkedAsAbsentEvent : DomainEvent
    {
        public Attendance Attendance { get; }

        public AttendanceMarkedAsAbsentEvent(Attendance attendance)
        {
            Attendance = attendance;
        }
    }

    public class AttendanceMarkedAsHalfDayEvent : DomainEvent
    {
        public Attendance Attendance { get; }

        public AttendanceMarkedAsHalfDayEvent(Attendance attendance)
        {
            Attendance = attendance;
        }
    }

    public class AttendanceMarkedAsOnLeaveEvent : DomainEvent
    {
        public Attendance Attendance { get; }

        public AttendanceMarkedAsOnLeaveEvent(Attendance attendance)
        {
            Attendance = attendance;
        }
    }

    public class AttendanceNotesUpdatedEvent : DomainEvent
    {
        public Attendance Attendance { get; }

        public AttendanceNotesUpdatedEvent(Attendance attendance)
        {
            Attendance = attendance;
        }
    }

    public class AttendanceDeletedEvent : DomainEvent
    {
        public Attendance Attendance { get; }
        public string DeletionReason { get; }

        public AttendanceDeletedEvent(Attendance attendance, string deletionReason)
        {
            Attendance = attendance;
            DeletionReason = deletionReason;
        }
    }

    public class AttendanceStatusChangedEvent : DomainEvent
    {
        public Attendance Attendance { get; }
        public string PreviousStatus { get; }
        public string NewStatus { get; }

        public AttendanceStatusChangedEvent(Attendance attendance, string previousStatus, string newStatus)
        {
            Attendance = attendance;
            PreviousStatus = previousStatus;
            NewStatus = newStatus;
        }
    }

    public class AttendanceRegularizedEvent : DomainEvent
    {
        public Attendance Attendance { get; }
        public string RegularizationReason { get; }
        public string ApprovedBy { get; }

        public AttendanceRegularizedEvent(Attendance attendance, string regularizationReason, string approvedBy)
        {
            Attendance = attendance;
            RegularizationReason = regularizationReason;
            ApprovedBy = approvedBy;
        }
    }
} 