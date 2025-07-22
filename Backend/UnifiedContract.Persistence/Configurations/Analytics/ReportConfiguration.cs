using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Analytics;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Analytics
{
    public class ReportConfiguration : BaseEntityConfiguration<Report>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Report> builder)
        {
            // Table name
            builder.ToTable("Report", "Analytics");
            
            // Properties
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(r => r.Description)
                .HasMaxLength(500);
                
            builder.Property(r => r.Type)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(r => r.Query)
                .IsRequired()
                .HasColumnType("nvarchar(max)");
                
            builder.Property(r => r.Parameters)
                .HasColumnType("nvarchar(max)");
                
            builder.Property(r => r.ExportFormats)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(r => r.IsSystem)
                .IsRequired()
                .HasDefaultValue(false);
                
            builder.Property(r => r.IsPublic)
                .IsRequired()
                .HasDefaultValue(false);
            
            // Indexes
            builder.HasIndex(r => r.Name);
            builder.HasIndex(r => r.Type);
            builder.HasIndex(r => r.CreatedByUserId);
            builder.HasIndex(r => r.TemplateId);
            builder.HasIndex(r => r.IsSystem);
            builder.HasIndex(r => r.IsPublic);
            builder.HasIndex(r => new { r.Type, r.IsPublic });
            builder.HasIndex(r => new { r.CreatedByUserId, r.IsPublic });
            
            // Relationships
            builder.HasOne(r => r.CreatedByUser)
                .WithMany()
                .HasForeignKey(r => r.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);
                
            builder.HasOne(r => r.Template)
                .WithMany()
                .HasForeignKey(r => r.TemplateId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 