using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Enums;

namespace UnifiedContract.Domain.Entities.WorkOrder
{
    public class WorkOrderForm : BaseEntity
    {
        public string Title { get; set; }
        public FormType Type { get; set; }
        public FormStatus Status { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public Guid? SubmittedById { get; set; }
        public string Url { get; set; }
        public Guid WorkOrderId { get; set; }

        // Navigation properties will be defined in the configurations
    }
} 