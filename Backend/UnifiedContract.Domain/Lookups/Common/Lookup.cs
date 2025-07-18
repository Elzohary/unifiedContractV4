using System;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.Entities.Common
{
    /// <summary>
    /// Base entity for all lookup tables that replace enums
    /// </summary>
    public abstract class Lookup : BaseEntity
    {
        /// <summary>
        /// Unique code identifier for the lookup value (e.g., "COMPLETED", "IN_PROGRESS")
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Display name shown to users (e.g., "Completed", "In Progress")
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Optional description for the lookup value
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Determines if this lookup value is active and available for selection
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Controls the display order in UI dropdowns and lists
        /// </summary>
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// Additional metadata as JSON (can store color codes, icons, etc.)
        /// </summary>
        public string Metadata { get; set; }
    }
} 