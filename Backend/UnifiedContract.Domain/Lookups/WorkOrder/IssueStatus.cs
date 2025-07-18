using System.Collections.Generic;
using UnifiedContract.Domain.Entities.Common;

namespace UnifiedContract.Domain.Entities.WorkOrder.Lookups
{
    /// <summary>
    /// Status options for work order issues (Open, In Progress, Resolved, Closed)
    /// </summary>
    public class IssueStatus : Lookup
    {
        /// <summary>
        /// Whether this status represents resolution of an issue
        /// </summary>
        public bool IsResolved { get; set; }
        
        /// <summary>
        /// Whether issues with this status can be edited
        /// </summary>
        public bool AllowsEditing { get; set; } = true;
        
        /// <summary>
        /// Color code for UI representation (hex or named color)
        /// </summary>
        public string ColorCode { get; set; }
        
        /// <summary>
        /// Navigation property for issues with this status
        /// </summary>
        public virtual ICollection<WorkOrderIssue> Issues { get; set; }

        public IssueStatus()
        {
            Issues = new HashSet<WorkOrderIssue>();
        }
    }
} 