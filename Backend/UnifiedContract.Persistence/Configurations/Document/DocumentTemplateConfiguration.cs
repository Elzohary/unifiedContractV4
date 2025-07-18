using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Document;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Document
{
    public class DocumentTemplateConfiguration : BaseEntityConfiguration<DocumentTemplate>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DocumentTemplate> builder)
        {
            // Table name
            builder.ToTable("DocumentTemplate", "Document");
            
            // Properties
            builder.Property(dt => dt.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(dt => dt.Description)
                .HasMaxLength(500);
                
            builder.Property(dt => dt.Type)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(dt => dt.ContentType)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(dt => dt.TemplateUrl)
                .HasMaxLength(500);
                
            builder.Property(dt => dt.TemplateContent)
                .HasColumnType("nvarchar(max)");
                
            builder.Property(dt => dt.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
                
            builder.Property(dt => dt.Version)
                .IsRequired()
                .HasDefaultValue(1);
            
            // Indexes
            builder.HasIndex(dt => dt.Name);
            builder.HasIndex(dt => dt.Type);
            builder.HasIndex(dt => dt.ContentType);
            builder.HasIndex(dt => dt.IsActive);
            builder.HasIndex(dt => dt.CreatedById);
            builder.HasIndex(dt => dt.LastModifiedById);
            builder.HasIndex(dt => new { dt.Type, dt.IsActive });
            builder.HasIndex(dt => new { dt.Name, dt.Version });
            
            // Relationships
            builder.HasOne(dt => dt.CreatedBy)
                .WithMany()
                .HasForeignKey(dt => dt.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(dt => dt.LastModifiedBy)
                .WithMany()
                .HasForeignKey(dt => dt.LastModifiedById)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 