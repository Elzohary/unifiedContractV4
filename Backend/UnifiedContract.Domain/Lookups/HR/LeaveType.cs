using System.Collections.Generic;
using UnifiedContract.Domain.Entities.Common;

namespace UnifiedContract.Domain.Entities.HR.Lookups
{
    /// <summary>
    /// Types of leave that employees can request (Annual, Sick, Maternity, etc.)
    /// </summary>
    public class LeaveType : Lookup
    {
        /// <summary>
        /// Default number of days allocated per year
        /// </summary>
        public int DefaultDaysPerYear { get; set; }
        
        /// <summary>
        /// Whether this type of leave requires documentation
        /// </summary>
        public bool RequiresDocumentation { get; set; }
        
        /// <summary>
        /// Whether this type of leave is paid
        /// </summary>
        public bool IsPaid { get; set; } = true;
        
        /// <summary>
        /// Maximum number of consecutive days allowed
        /// </summary>
        public int? MaxConsecutiveDays { get; set; }
        
        /// <summary>
        /// Minimum notice period in days
        /// </summary>
        public int? MinimumNoticeDays { get; set; }
        
        /// <summary>
        /// Navigation property for leaves of this type
        /// </summary>
        public virtual ICollection<Leave> Leaves { get; set; }

        public LeaveType()
        {
            Leaves = new HashSet<Leave>();
        }
    }
} 