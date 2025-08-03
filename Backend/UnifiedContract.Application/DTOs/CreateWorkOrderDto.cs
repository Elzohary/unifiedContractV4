using System;
using System.ComponentModel.DataAnnotations;

namespace UnifiedContract.Application.DTOs
{
    public class CreateWorkOrderDto
    {
        [Required]
        public string WorkOrderNumber { get; set; } = string.Empty;
        
        [Required]
        public string InternalOrderNumber { get; set; } = string.Empty;
        
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public string Location { get; set; } = string.Empty;
        
        [Required]
        public string Category { get; set; } = string.Empty;
        
        public string Type { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string ProjectType { get; set; } = string.Empty;
        public string PO { get; set; } = string.Empty;
        public string D1 { get; set; } = string.Empty;
        
        [Range(0, 100)]
        public decimal CompletionPercentage { get; set; }
        
        [Required]
        public DateTime ReceivedDate { get; set; }
        
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? TargetEndDate { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal? EstimatedCost { get; set; }
        
        [Required]
        public Guid WorkOrderStatusId { get; set; }
        
        [Required]
        public Guid PriorityLevelId { get; set; }
        
        public Guid? ClientId { get; set; }
        public Guid? EngineerInChargeId { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
} 