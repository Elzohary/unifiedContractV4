using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;
using System;

namespace UnifiedContract.Domain.Events.HR
{
    public class SalaryAddedEvent : DomainEvent
    {
        public Salary Salary { get; }

        public SalaryAddedEvent(Salary salary)
        {
            Salary = salary;
        }
    }

    public class SalaryUpdatedEvent : DomainEvent
    {
        public Salary Salary { get; }

        public SalaryUpdatedEvent(Salary salary)
        {
            Salary = salary;
        }
    }

    public class SalaryActivatedEvent : DomainEvent
    {
        public Salary Salary { get; }

        public SalaryActivatedEvent(Salary salary)
        {
            Salary = salary;
        }
    }

    public class SalaryDeactivatedEvent : DomainEvent
    {
        public Salary Salary { get; }

        public SalaryDeactivatedEvent(Salary salary)
        {
            Salary = salary;
        }
    }

    public class SalaryTerminatedEvent : DomainEvent
    {
        public Salary Salary { get; }

        public SalaryTerminatedEvent(Salary salary)
        {
            Salary = salary;
        }
    }

    public class SalaryDeletedEvent : DomainEvent
    {
        public Salary Salary { get; }
        public string DeletionReason { get; }

        public SalaryDeletedEvent(Salary salary, string deletionReason)
        {
            Salary = salary;
            DeletionReason = deletionReason;
        }
    }

    public class SalaryApprovedEvent : DomainEvent
    {
        public Salary Salary { get; }
        public string ApprovedBy { get; }
        public DateTime ApprovalDate { get; }

        public SalaryApprovedEvent(Salary salary, string approvedBy, DateTime approvalDate)
        {
            Salary = salary;
            ApprovedBy = approvedBy;
            ApprovalDate = approvalDate;
        }
    }

    public class SalaryRejectedEvent : DomainEvent
    {
        public Salary Salary { get; }
        public string RejectedBy { get; }
        public string RejectionReason { get; }
        public DateTime RejectionDate { get; }

        public SalaryRejectedEvent(Salary salary, string rejectedBy, string rejectionReason, DateTime rejectionDate)
        {
            Salary = salary;
            RejectedBy = rejectedBy;
            RejectionReason = rejectionReason;
            RejectionDate = rejectionDate;
        }
    }

    public class SalaryEffectiveDateChangedEvent : DomainEvent
    {
        public Salary Salary { get; }
        public DateTime PreviousEffectiveDate { get; }
        public DateTime NewEffectiveDate { get; }

        public SalaryEffectiveDateChangedEvent(Salary salary, DateTime previousEffectiveDate, DateTime newEffectiveDate)
        {
            Salary = salary;
            PreviousEffectiveDate = previousEffectiveDate;
            NewEffectiveDate = newEffectiveDate;
        }
    }
} 