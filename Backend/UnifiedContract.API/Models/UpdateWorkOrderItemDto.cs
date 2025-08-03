namespace UnifiedContract.API.Models
{
    public class UpdateWorkOrderItemDto
    {
        public UpdateWorkOrderItemRequest Item { get; set; }
    }

    public class UpdateWorkOrderItemRequest
    {
        public string Id { get; set; }
        public string ItemNumber { get; set; }
        public string LineType { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string UOM { get; set; }
        public string Currency { get; set; }
        public decimal UnitPrice { get; set; }
        public string PaymentType { get; set; }
        public string ManagementArea { get; set; }
        public decimal EstimatedQuantity { get; set; }
    }
} 