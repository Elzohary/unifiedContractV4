using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Domain.Events.WorkOrder
{
    public class WorkOrderIssueCreatedEvent : DomainEvent
    {
        public WorkOrderIssue Issue { get; }

        public WorkOrderIssueCreatedEvent(WorkOrderIssue issue)
        {
            Issue = issue;
        }
    }
    
    public class WorkOrderIssueUpdatedEvent : DomainEvent
    {
        public WorkOrderIssue Issue { get; }

        public WorkOrderIssueUpdatedEvent(WorkOrderIssue issue)
        {
            Issue = issue;
        }
    }
    
    public class WorkOrderIssueResolvedEvent : DomainEvent
    {
        public WorkOrderIssue Issue { get; }
        public DateTime ResolutionDate { get; }
        public string ResolutionNotes { get; }

        public WorkOrderIssueResolvedEvent(WorkOrderIssue issue, DateTime resolutionDate, string resolutionNotes)
        {
            Issue = issue;
            ResolutionDate = resolutionDate;
            ResolutionNotes = resolutionNotes;
        }
    }
    
    public class WorkOrderIssuePriorityChangedEvent : DomainEvent
    {
        public WorkOrderIssue Issue { get; }
        public string PreviousPriority { get; }
        public string NewPriority { get; }

        public WorkOrderIssuePriorityChangedEvent(WorkOrderIssue issue, string previousPriority, string newPriority)
        {
            Issue = issue;
            PreviousPriority = previousPriority;
            NewPriority = newPriority;
        }
    }
} 