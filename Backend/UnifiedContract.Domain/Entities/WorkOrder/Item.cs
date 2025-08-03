using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.Client;

namespace UnifiedContract.Domain.Entities.WorkOrder
{
    public class Item : BaseEntity
    {
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public string PaymentType { get; set; }
        public string ManagementArea { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Foreign keys
        public Guid ClientId { get; set; }
        
        // Navigation properties
        public virtual Client.Client Client { get; set; }
        public virtual ICollection<WorkOrderItemAssignment> WorkOrderAssignments { get; set; }
        
        public Item()
        {
            WorkOrderAssignments = new HashSet<WorkOrderItemAssignment>();
        }
    }
} 