using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.Resource;

namespace UnifiedContract.Domain.Entities.WorkOrder
{
    public class SiteReportMaterialUsed : BaseEntity
    {
        public Guid SiteReportId { get; set; }
        public Guid MaterialAssignmentId { get; set; }
        public string MaterialName { get; set; }
        public decimal Quantity { get; set; }

        // Navigation
        public virtual SiteReport SiteReport { get; set; }
        public virtual MaterialAssignment MaterialAssignment { get; set; }
    }
} 