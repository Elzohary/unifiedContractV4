namespace UnifiedContract.API.Models
{
    public class CreateWorkOrderItemDto
    {
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public string PaymentType { get; set; }
        public string ManagementArea { get; set; }
        public string Currency { get; set; }
    }
} 