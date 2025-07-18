using System.Collections.Generic;
using UnifiedContract.Domain.Entities.Common;

namespace UnifiedContract.Domain.Entities.HR.Lookups
{
    /// <summary>
    /// Status options for leave requests (Pending, Approved, Rejected, Cancelled)
    /// </summary>
    public class LeaveStatus : Lookup
    {
        /// <summary>
        /// Whether this status is a final state
        /// </summary>
        public bool IsFinalState { get; set; }
        
        /// <summary>
        /// Whether this status counts against employee's leave balance
        /// </summary>
        public bool DeductsFromBalance { get; set; }
        
        /// <summary>
        /// Whether leave requests with this status can be edited
        /// </summary>
        public bool AllowsEditing { get; set; }
        
        /// <summary>
        /// Color code for UI representation (hex or named color)
        /// </summary>
        public string ColorCode { get; set; }
        
        /// <summary>
        /// Navigation property for leaves with this status
        /// </summary>
        public virtual ICollection<Leave> Leaves { get; set; }

        public LeaveStatus()
        {
            Leaves = new HashSet<Leave>();
        }
    }
} 