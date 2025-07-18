using System.Collections.Generic;
using UnifiedContract.Domain.Entities.Common;

namespace UnifiedContract.Domain.Entities.Resource.Lookups
{
    /// <summary>
    /// Types of materials (Receivable or Purchasable)
    /// </summary>
    public class MaterialType : Lookup
    {
        /// <summary>
        /// Whether this type represents a purchasable material
        /// </summary>
        public bool IsPurchasable { get; set; }
        
        /// <summary>
        /// Whether this type represents a receivable material
        /// </summary>
        public bool IsReceivable { get; set; }
        
        /// <summary>
        /// Whether materials of this type require tracking
        /// </summary>
        public bool RequiresTracking { get; set; } = true;
        
        /// <summary>
        /// Whether materials of this type have a cost
        /// </summary>
        public bool HasCost { get; set; }
        
        /// <summary>
        /// Navigation properties for materials of this type
        /// </summary>
        public virtual ICollection<MaterialAssignment> Materials { get; set; }

        public MaterialType()
        {
            Materials = new HashSet<MaterialAssignment>();
        }
    }
} 