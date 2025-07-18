using System.Collections.Generic;
using UnifiedContract.Domain.Entities.Common;

namespace UnifiedContract.Domain.Entities.Resource.Lookups
{
    /// <summary>
    /// Types of maintenance for equipment (Routine, Repair, Inspection, Emergency)
    /// </summary>
    public class MaintenanceType : Lookup
    {
        /// <summary>
        /// Whether this type of maintenance is scheduled or unplanned
        /// </summary>
        public bool IsScheduled { get; set; }
        
        /// <summary>
        /// Whether this type requires the equipment to be out of service
        /// </summary>
        public bool RequiresOutOfService { get; set; }
        
        /// <summary>
        /// Typical duration in hours for this maintenance type
        /// </summary>
        public int? TypicalDurationHours { get; set; }
        
        /// <summary>
        /// Typical interval in days between maintenance of this type
        /// </summary>
        public int? TypicalIntervalDays { get; set; }
        
        /// <summary>
        /// Color code for UI representation (hex or named color)
        /// </summary>
        public string ColorCode { get; set; }
        
        /// <summary>
        /// Navigation property for maintenance records of this type
        /// </summary>
        public virtual ICollection<EquipmentMaintenance> MaintenanceRecords { get; set; }

        public MaintenanceType()
        {
            MaintenanceRecords = new HashSet<EquipmentMaintenance>();
        }
    }
} 