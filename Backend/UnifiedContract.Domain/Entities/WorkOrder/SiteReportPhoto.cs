using System;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.Entities.WorkOrder
{
    public class SiteReportPhoto : BaseEntity
    {
        public Guid SiteReportId { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }
        public string Category { get; set; } // safety, progress, housekeeping
        public DateTime UploadedDate { get; set; }

        // Navigation
        public virtual SiteReport SiteReport { get; set; }
    }
} 