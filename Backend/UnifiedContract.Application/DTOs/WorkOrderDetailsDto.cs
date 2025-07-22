using System;
using System.Collections.Generic;

namespace UnifiedContract.Application.DTOs
{
    public class WorkOrderDetailsDto
    {
        public Guid Id { get; set; }
        public string WorkOrderNumber { get; set; }
        public string InternalOrderNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Client { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Class { get; set; }
        public decimal CompletionPercentage { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? TargetEndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public decimal? EstimatedPrice { get; set; }
        public string EngineerInCharge { get; set; }
        public List<PermitDto> Permits { get; set; }
    }
} 