using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.Auth;

namespace UnifiedContract.Domain.Entities.Document
{
    public class DocumentTemplate : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // WorkOrder, Invoice, Report, Form, etc.
        public string ContentType { get; set; } // HTML, PDF, DOCX, etc.
        public string TemplateUrl { get; set; }
        public string TemplateContent { get; set; }
        public bool IsActive { get; set; }
        public int Version { get; set; }
        public Guid CreatedById { get; set; }
        public Guid? LastModifiedById { get; set; }
        
        // Navigation properties
        public virtual User CreatedByUser { get; set; }
        public virtual User LastModifiedByUser { get; set; }
    }
} 