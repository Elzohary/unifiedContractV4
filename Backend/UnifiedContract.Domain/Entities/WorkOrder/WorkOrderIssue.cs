using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Enums;

namespace UnifiedContract.Domain.Entities.WorkOrder
{
    public class WorkOrderIssue : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public WorkOrderPriority Priority { get; set; }
        public IssueStatus Status { get; set; }
        public IssueSeverity Severity { get; set; }
        public Guid ReportedById { get; set; }
        public DateTime ReportedDate { get; set; }
        public Guid? AssignedToId { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public string ResolutionNotes { get; set; }
        public Guid WorkOrderId { get; set; }

        // Navigation properties will be defined in the configurations
    }
} 