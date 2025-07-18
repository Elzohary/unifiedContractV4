using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Events.HR;

namespace UnifiedContract.Domain.Entities.HR
{
    public enum LeaveType
    {
        Annual,
        Sick,
        Unpaid,
        Emergency,
        Maternity,
        Paternity,
        Bereavement,
        Study,
        Hajj,
        Other
    }
    
    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }
    
    public class Leave : BaseEntity
    {
        // Private setters to protect invariants
        public LeaveType Type { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public int TotalDays { get; private set; }
        public string Reason { get; private set; }
        public LeaveStatus Status { get; private set; } = LeaveStatus.Pending;
        public string Comments { get; private set; }
        public Guid? ApprovedById { get; private set; }
        public DateTime? ApprovedDate { get; private set; }
        public string RejectionReason { get; private set; }
        public string DocumentUrl { get; set; }
        
        // Foreign Keys
        public Guid EmployeeId { get; private set; }
        
        // Navigation Properties
        public virtual Employee Employee { get; set; }
        public virtual Employee ApprovedBy { get; set; }
        
        // For EF Core
        protected Leave() { }
        
        public Leave(Guid employeeId, LeaveType type, DateTime startDate, DateTime endDate, string reason)
        {
            ValidateLeaveRequest(startDate, endDate, reason);
            
            EmployeeId = employeeId;
            Type = type;
            StartDate = startDate;
            EndDate = endDate;
            Reason = reason;
            TotalDays = CalculateTotalDays(startDate, endDate);
            Status = LeaveStatus.Pending;
            
            AddDomainEvent(new LeaveRequestedEvent(this));
        }
        
        public void UpdateLeaveDetails(DateTime startDate, DateTime endDate, string reason)
        {
            if (Status != LeaveStatus.Pending)
                throw new DomainException("Cannot update a leave request that is not in pending status");
                
            ValidateLeaveRequest(startDate, endDate, reason);
            
            StartDate = startDate;
            EndDate = endDate;
            Reason = reason;
            TotalDays = CalculateTotalDays(startDate, endDate);
            
            AddDomainEvent(new LeaveUpdatedEvent(this));
        }
        
        public void Approve(Guid approverId, string comments = null)
        {
            if (Status != LeaveStatus.Pending)
                throw new DomainException("Only pending leave requests can be approved");
                
            if (approverId == Guid.Empty)
                throw new DomainException("Approver ID is required");
                
            Status = LeaveStatus.Approved;
            ApprovedById = approverId;
            ApprovedDate = DateTime.UtcNow;
            Comments = comments;
            
            AddDomainEvent(new LeaveApprovedEvent(this));
        }
        
        public void Reject(Guid approverId, string rejectionReason)
        {
            if (Status != LeaveStatus.Pending)
                throw new DomainException("Only pending leave requests can be rejected");
                
            if (approverId == Guid.Empty)
                throw new DomainException("Approver ID is required");
                
            if (string.IsNullOrWhiteSpace(rejectionReason))
                throw new DomainException("Rejection reason is required");
                
            Status = LeaveStatus.Rejected;
            ApprovedById = approverId;
            ApprovedDate = DateTime.UtcNow;
            RejectionReason = rejectionReason;
            
            AddDomainEvent(new LeaveRejectedEvent(this));
        }
        
        public void Cancel()
        {
            if (Status != LeaveStatus.Pending && Status != LeaveStatus.Approved)
                throw new DomainException("Only pending or approved leave requests can be cancelled");
                
            Status = LeaveStatus.Cancelled;
            
            AddDomainEvent(new LeaveCancelledEvent(this));
        }
        
        public void AddComments(string comments)
        {
            if (string.IsNullOrWhiteSpace(comments))
                return;
                
            Comments = comments;
        }
        
        private static void ValidateLeaveRequest(DateTime startDate, DateTime endDate, string reason)
        {
            if (startDate > endDate)
                throw new DomainException("End date must be after start date");
                
            if (startDate < DateTime.UtcNow.Date)
                throw new DomainException("Start date cannot be in the past");
                
            if (string.IsNullOrWhiteSpace(reason))
                throw new DomainException("Reason is required");
                
            if (reason.Length > 500)
                throw new DomainException("Reason cannot be longer than 500 characters");
        }
        
        private static int CalculateTotalDays(DateTime startDate, DateTime endDate)
        {
            return (endDate.Date - startDate.Date).Days + 1;
        }
    }
} 