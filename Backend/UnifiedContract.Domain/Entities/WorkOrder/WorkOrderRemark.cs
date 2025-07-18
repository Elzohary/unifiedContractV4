using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Enums;

namespace UnifiedContract.Domain.Entities.WorkOrder
{
    public class WorkOrderRemark : BaseEntity
    {
        public string Content { get; set; }
        public RemarkType Type { get; set; }
        public Guid WorkOrderId { get; set; }
        public string[] PeopleInvolved { get; set; } // Stored as JSON array of IDs

        // Navigation properties will be defined in the configurations
    }
} 