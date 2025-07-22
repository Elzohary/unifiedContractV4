using System;

namespace UnifiedContract.Application.DTOs
{
    public class WorkOrderListItemDto
    {
        public Guid Id { get; set; }
        public string WorkOrderNumber { get; set; }
        public string InternalOrderNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Client { get; set; }
        public string Location { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string PriorityCode { get; set; }
        public string PriorityName { get; set; }
        public string Category { get; set; }
        public decimal CompletionPercentage { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? TargetEndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public decimal? EstimatedCost { get; set; }
    }
} 