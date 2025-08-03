using System;

namespace UnifiedContract.Application.DTOs
{
    public class WorkOrderItemDto
    {
        public Guid Id { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal? UnitPrice { get; set; }
        public string PaymentType { get; set; }
        public string ManagementArea { get; set; }
        public string Currency { get; set; }
        public decimal? EstimatedQuantity { get; set; }
        public decimal? EstimatedPrice { get; set; }
        public decimal? ActualQuantity { get; set; }
        public decimal? ActualPrice { get; set; }
        public string ReasonForFinalQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
    }
} 