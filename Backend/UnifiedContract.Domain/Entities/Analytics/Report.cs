using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.Auth;

namespace UnifiedContract.Domain.Entities.Analytics
{
    public class Report : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // WorkOrder, Financial, HR, Resource, etc.
        public string Query { get; set; } // SQL query or stored procedure name
        public string Parameters { get; set; } // JSON parameters configuration
        public string ExportFormats { get; set; } // CSV, Excel, PDF, etc. as JSON array
        public bool IsSystem { get; set; }
        public bool IsPublic { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? TemplateId { get; set; }
        
        // Navigation properties
        public virtual User CreatedBy { get; set; }
        public virtual Document.DocumentTemplate Template { get; set; }
    }
} 