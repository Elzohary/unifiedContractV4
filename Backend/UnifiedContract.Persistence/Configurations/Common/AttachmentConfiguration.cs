using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Common;
using UnifiedContract.Domain.Entities.Auth;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Common
{
    public class AttachmentConfiguration : BaseEntityConfiguration<Attachment>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Attachment> builder)
        {
            // Table name
            builder.ToTable("Attachments", "Common");
            
            // Properties
            builder.Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(255);
                
            builder.Property(e => e.FileType)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(e => e.FileSize)
                .IsRequired();
                
            builder.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(1000);
                
            builder.Property(e => e.Description)
                .HasMaxLength(500);
                
            builder.Property(e => e.UploadDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            
            // Indexes
            builder.HasIndex(e => e.UploadedById);
            builder.HasIndex(e => e.UploadDate);
            builder.HasIndex(e => e.FileType);
            builder.HasIndex(e => new { e.UploadDate, e.UploadedById });
            
            // Relationships
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 