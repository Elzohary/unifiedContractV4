using System;
using System.Collections.Generic;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Enums;

namespace UnifiedContract.Domain.Entities.WorkOrder
{
    public class Permit : BaseEntity
    {
        public string Type { get; set; } // e.g., 'Municipality', 'Electrical', 'Plumbing', etc.
        public string Title { get; set; }
        public string Description { get; set; }
        public string Number { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public PermitStatus Status { get; set; }
        public string IssuedBy { get; set; }
        public string Authority { get; set; }
        public string DocumentRef { get; set; }
        public Guid WorkOrderId { get; set; }

        // Navigation properties will be defined in the configurations
    }
} 