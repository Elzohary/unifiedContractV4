using System;
using System.Collections.Generic;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.Entities.WorkOrder
{
    public class SiteReport : BaseEntity
    {
        public Guid WorkOrderId { get; set; }
        public Guid ForemanId { get; set; }
        public string ForemanName { get; set; }
        public SiteReportStatus Status { get; set; }
        public string WorkDone { get; set; }
        public decimal? ActualQuantity { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public virtual WorkOrder WorkOrder { get; set; }
        public virtual ICollection<SiteReportMaterialUsed> MaterialsUsed { get; set; } = new List<SiteReportMaterialUsed>();
        public virtual ICollection<SiteReportPhoto> Photos { get; set; } = new List<SiteReportPhoto>();
    }

    public enum SiteReportStatus
    {
        Open = 0,
        Closed = 1
    }
} 