using System;

namespace UnifiedContract.API.Models
{
    public class AssignItemToWorkOrderDto
    {
        public Guid ItemId { get; set; }
        public decimal EstimatedQuantity { get; set; }
        public string ReasonForFinalQuantity { get; set; }
    }
} 