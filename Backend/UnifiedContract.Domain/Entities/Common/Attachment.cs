using System;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.Entities.Common
{
    public class Attachment : BaseEntity
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public string FilePath { get; set; }
        public Guid UploadedById { get; set; }
        public DateTime UploadDate { get; set; }
        
        // For polymorphic association
        public Guid EntityId { get; set; }
        public string EntityType { get; set; }

        // Navigation properties will be defined in the configurations
    }
} 