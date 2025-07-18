using System;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.Entities.Common
{
    public class Attachment : BaseEntity
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public string Url { get; set; }
        public Guid UploadedById { get; set; }
        public DateTime UploadDate { get; set; }
        public string Description { get; set; }

        // Navigation properties will be defined in the configurations
    }
} 