using System;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.Entities.Client
{
    public class ClientContact : BaseEntity
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AlternatePhone { get; set; }
        public bool IsPrimaryContact { get; set; }
        public string Notes { get; set; }
        public Guid ClientId { get; set; }
        
        // Navigation properties
        public virtual Client Client { get; set; }
    }
} 