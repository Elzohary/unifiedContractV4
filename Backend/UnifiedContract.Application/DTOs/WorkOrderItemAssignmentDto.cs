using System;

namespace UnifiedContract.Application.DTOs
{
    public class WorkOrderItemAssignmentDto
    {
        public Guid Id { get; set; }
        public Guid WorkOrderId { get; set; }
        public Guid ItemId { get; set; }
        public decimal? EstimatedQuantity { get; set; }
        public decimal? EstimatedPrice { get; set; }
        public decimal? EstimatedPriceWithVAT { get; set; }
        public decimal? ActualQuantity { get; set; }
        public decimal? ActualPrice { get; set; }
        public decimal? ActualPriceWithVAT { get; set; }
        public string ReasonForFinalQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
        public ItemDto Item { get; set; }
    }
} 