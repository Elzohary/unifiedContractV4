using System;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.Entities.WorkOrder
{
    public class WorkOrderItemAssignment : BaseEntity
    {
        public decimal EstimatedQuantity { get; set; }
        public decimal EstimatedPrice { get; set; }
        public decimal EstimatedPriceWithVAT { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal ActualPrice { get; set; }
        public decimal ActualPriceWithVAT { get; set; }
        public string ReasonForFinalQuantity { get; set; }
        
        // Foreign keys
        public Guid WorkOrderId { get; set; }
        public Guid ItemId { get; set; }
        
        // Navigation properties
        public virtual WorkOrder WorkOrder { get; set; }
        public virtual Item Item { get; set; }
    }
} 