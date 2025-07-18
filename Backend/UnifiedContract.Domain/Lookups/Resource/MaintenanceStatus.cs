using System.Collections.Generic;
using UnifiedContract.Domain.Entities.Common;

namespace UnifiedContract.Domain.Entities.Resource.Lookups
{
    /// <summary>
    /// Status options for equipment maintenance (Scheduled, InProgress, Completed, Cancelled)
    /// </summary>
    public class MaintenanceStatus : Lookup
    {
        /// <summary>
        /// Whether this status is a final state
        /// </summary>
        public bool IsFinalState { get; set; }
        
        /// <summary>
        /// Whether maintenance records with this status can be edited
        /// </summary>
        public bool AllowsEditing { get; set; } = true;
        
        /// <summary>
        /// Color code for UI representation (hex or named color)
        /// </summary>
        public string ColorCode { get; set; }
        
        /// <summary>
        /// Navigation property for maintenance records with this status
        /// </summary>
        public virtual ICollection<EquipmentMaintenance> MaintenanceRecords { get; set; }

        public MaintenanceStatus()
        {
            MaintenanceRecords = new HashSet<EquipmentMaintenance>();
        }
    }
} 