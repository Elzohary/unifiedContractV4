using System;
using System.Collections.Generic;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.Entities.Auth
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public string Module { get; set; }
        public string Action { get; set; } // Create, Read, Update, Delete, Export, etc.
        
        // Navigation properties
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        
        public Permission()
        {
            RolePermissions = new HashSet<RolePermission>();
        }
    }
} 