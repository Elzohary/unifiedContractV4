using System;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.Entities.WorkOrder
{
    public class WorkOrderItem : BaseEntity
    {
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal EstimatedQuantity { get; set; }
        public decimal EstimatedPrice { get; set; }
        public decimal EstimatedPriceWithVAT { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal ActualPrice { get; set; }
        public decimal ActualPriceWithVAT { get; set; }
        public string ReasonForFinalQuantity { get; set; }
        public string PaymentType { get; set; }
        public string ManagementArea { get; set; }
        public string Currency { get; set; }
        
        // Foreign keys
        public Guid? WorkOrderId { get; set; }
        
        // Navigation properties will be defined in the configurations
    }
} 