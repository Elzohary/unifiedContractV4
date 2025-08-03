using System;

namespace UnifiedContract.Application.DTOs
{
    public class UpdateMasterItemDto
    {
        public string ItemNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public string PaymentType { get; set; } = string.Empty;
        public string ManagementArea { get; set; } = string.Empty;
        public string Currency { get; set; } = "SAR";
        public bool IsActive { get; set; } = true;
        public Guid ClientId { get; set; }
    }
} 