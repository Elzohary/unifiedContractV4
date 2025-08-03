using System;
using System.ComponentModel.DataAnnotations;

namespace UnifiedContract.Application.DTOs
{
    public class CreateMasterItemDto
    {
        [Required(ErrorMessage = "Item number is required")]
        [StringLength(50, ErrorMessage = "Item number cannot exceed 50 characters")]
        public string ItemNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Unit cannot exceed 50 characters")]
        public string Unit { get; set; } = "Piece";

        [Range(0, double.MaxValue, ErrorMessage = "Unit price must be a positive number")]
        public decimal UnitPrice { get; set; }

        [StringLength(100, ErrorMessage = "Payment type cannot exceed 100 characters")]
        public string PaymentType { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Management area cannot exceed 200 characters")]
        public string ManagementArea { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "Currency cannot exceed 10 characters")]
        public string Currency { get; set; } = "SAR";

        [Required(ErrorMessage = "Client ID is required")]
        public Guid ClientId { get; set; }
    }
} 