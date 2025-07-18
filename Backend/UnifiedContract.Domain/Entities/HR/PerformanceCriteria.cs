using System;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.Entities.HR
{
    public class PerformanceCriteria : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; } // 1-5 scale
        public string Comments { get; set; }
        public int Weight { get; set; } // Percentage weight of this criteria in the overall rating
        
        // Foreign Key
        public Guid PerformanceReviewId { get; set; }
        
        // Navigation Property
        public virtual PerformanceReview PerformanceReview { get; set; }
    }
} 