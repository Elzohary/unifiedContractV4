using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Enums;
using UnifiedContract.Domain.Entities.Auth;
namespace UnifiedContract.Domain.Entities.Common
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public NotificationType Type { get; set; }
        public string EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public string Url { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public Guid UserId { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
    }
} 