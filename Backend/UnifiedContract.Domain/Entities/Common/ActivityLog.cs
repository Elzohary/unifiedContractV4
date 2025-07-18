using System;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.Entities.Common
{
    public class ActivityLog : BaseEntity
    {
        public string Action { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public string EntityName { get; set; }
        public string Details { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string Location { get; set; }
        public Guid UserId { get; set; }
    }
} 