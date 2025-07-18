using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Enums;

namespace UnifiedContract.Domain.Entities.WorkOrder
{
    public class WorkOrderPhoto : BaseEntity
    {
        public string Url { get; set; }
        public string Caption { get; set; }
        public DateTime UploadedDate { get; set; }
        public Guid UploadedById { get; set; }
        public PhotoType Type { get; set; }
        public Guid WorkOrderId { get; set; }

        // Navigation properties will be defined in the configurations
    }
} 