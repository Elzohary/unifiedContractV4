using System;

namespace UnifiedContract.API.Models
{
    public class UpdateWorkOrderItemAssignmentDto
    {
        public decimal? EstimatedQuantity { get; set; }
        public decimal? ActualQuantity { get; set; }
        public string ReasonForFinalQuantity { get; set; }
    }
} 