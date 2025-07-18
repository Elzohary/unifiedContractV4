using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Common;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Common
{
    public class ActivityLogConfiguration : BaseEntityConfiguration<ActivityLog>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ActivityLog> builder)
        {
            // Table name
            builder.ToTable("ActivityLogs", "Common");
            
            // Properties
            builder.Property(e => e.Action)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(e => e.EntityType)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(e => e.EntityName)
                .HasMaxLength(100);
                
            builder.Property(e => e.Details)
                .HasMaxLength(1000);
                
            builder.Property(e => e.IpAddress)
                .HasMaxLength(50);
                
            builder.Property(e => e.UserAgent)
                .HasMaxLength(500);
                
            builder.Property(e => e.Location)
                .HasMaxLength(100);
            
            // Indexes
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => e.EntityType);
            builder.HasIndex(e => e.EntityId);
            builder.HasIndex(e => e.CreatedAt);
            builder.HasIndex(e => new { e.EntityType, e.EntityId });
        }
    }
} 