using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Domain.Events.WorkOrder
{
    public class PermitCreatedEvent : DomainEvent
    {
        public Permit Permit { get; }

        public PermitCreatedEvent(Permit permit)
        {
            Permit = permit;
        }
    }
    
    public class PermitUpdatedEvent : DomainEvent
    {
        public Permit Permit { get; }

        public PermitUpdatedEvent(Permit permit)
        {
            Permit = permit;
        }
    }
    
    public class PermitApprovedEvent : DomainEvent
    {
        public Permit Permit { get; }
        public DateTime ApprovalDate { get; }

        public PermitApprovedEvent(Permit permit, DateTime approvalDate)
        {
            Permit = permit;
            ApprovalDate = approvalDate;
        }
    }
    
    public class PermitRejectedEvent : DomainEvent
    {
        public Permit Permit { get; }
        public string RejectionReason { get; }

        public PermitRejectedEvent(Permit permit, string rejectionReason)
        {
            Permit = permit;
            RejectionReason = rejectionReason;
        }
    }
    
    public class PermitExpiredEvent : DomainEvent
    {
        public Permit Permit { get; }

        public PermitExpiredEvent(Permit permit)
        {
            Permit = permit;
        }
    }
} 