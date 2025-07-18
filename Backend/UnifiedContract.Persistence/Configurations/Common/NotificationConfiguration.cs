using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Common;
using UnifiedContract.Domain.Entities.Auth;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Common
{
    public class NotificationConfiguration : BaseEntityConfiguration<Notification>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Notification> builder)
        {
            // Table name
            builder.ToTable("Notifications", "Common");
            
            // Properties
            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(1000);
                
            builder.Property(e => e.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
                
            builder.Property(e => e.EntityType)
                .HasMaxLength(100);
                
            builder.Property(e => e.Url)
                .HasMaxLength(500);
                
            builder.Property(e => e.IsRead)
                .IsRequired()
                .HasDefaultValue(false);
                
            builder.Property(e => e.ReadDate);
            
            // Indexes
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => e.IsRead);
            builder.HasIndex(e => e.CreatedAt);
            builder.HasIndex(e => new { e.UserId, e.IsRead });
            builder.HasIndex(e => new { e.EntityId, e.EntityType });
            
            // Relationships
            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 