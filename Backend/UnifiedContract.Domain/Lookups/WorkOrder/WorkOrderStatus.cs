using System.Collections.Generic;
using UnifiedContract.Domain.Entities.Common;

namespace UnifiedContract.Domain.Entities.WorkOrder.Lookups
{
    /// <summary>
    /// Status options for a work order (Pending, In Progress, Completed, etc.)
    /// </summary>
    public class WorkOrderStatus : Lookup
    {
        /// <summary>
        /// Whether this status represents completion of a work order
        /// </summary>
        public bool IsCompleted { get; set; }
        
        /// <summary>
        /// Whether this status allows further changes to the work order
        /// </summary>
        public bool AllowsEditing { get; set; } = true;
        
        /// <summary>
        /// Whether this status requires approval to progress
        /// </summary>
        public bool RequiresApproval { get; set; }
        
        /// <summary>
        /// Color code for UI representation (hex or named color)
        /// </summary>
        public string ColorCode { get; set; }
        
        /// <summary>
        /// Navigation property for work orders with this status
        /// </summary>
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }

        public WorkOrderStatus()
        {
            WorkOrders = new HashSet<WorkOrder>();
        }
    }
} 