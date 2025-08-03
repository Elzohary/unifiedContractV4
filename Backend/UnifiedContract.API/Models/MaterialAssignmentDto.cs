using UnifiedContract.Domain.Enums;

namespace UnifiedContract.API.Models
{
    public class MaterialAssignmentDto
    {
        public MaterialType MaterialType { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public string? StoringLocation { get; set; }
        public string? Notes { get; set; }
        public Guid? PurchasableMaterialId { get; set; }
        public Guid? ReceivableMaterialId { get; set; }
    }

    public class UpdateMaterialAssignmentDto
    {
        public decimal? Quantity { get; set; }
        public string? StoringLocation { get; set; }
        public string? Notes { get; set; }
    }
} 