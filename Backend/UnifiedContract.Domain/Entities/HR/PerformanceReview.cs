using System;
using System.Collections.Generic;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.Entities.HR
{
    public enum ReviewStatus
    {
        Draft,
        PendingManagerReview,
        PendingEmployeeAcknowledgment,
        Completed,
        Archived
    }
    
    public class PerformanceReview : BaseEntity
    {
        public string Title { get; set; }
        public DateTime ReviewPeriodStart { get; set; }
        public DateTime ReviewPeriodEnd { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int OverallRating { get; set; } // 1-5 scale
        public string ManagerComments { get; set; }
        public string EmployeeComments { get; set; }
        public string GoalsForNextPeriod { get; set; }
        public string TrainingRecommendations { get; set; }
        public ReviewStatus Status { get; set; } = ReviewStatus.Draft;
        
        // Foreign Keys
        public Guid EmployeeId { get; set; }
        public Guid ReviewerId { get; set; }
        
        // Navigation Properties
        public virtual Employee Employee { get; set; }
        public virtual Employee Reviewer { get; set; }
        public virtual ICollection<PerformanceCriteria> PerformanceCriteria { get; set; } = new List<PerformanceCriteria>();
    }
} 