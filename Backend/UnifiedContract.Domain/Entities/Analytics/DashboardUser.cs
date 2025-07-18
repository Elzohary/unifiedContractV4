using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.Auth;

namespace UnifiedContract.Domain.Entities.Analytics
{
    public class DashboardUser : BaseEntity
    {
        public Guid DashboardId { get; set; }
        public Guid UserId { get; set; }
        public bool CanEdit { get; set; }
        
        // Navigation properties
        public virtual Dashboard Dashboard { get; set; }
        public virtual User User { get; set; }
    }
} 