using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Analytics;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Analytics
{
    public class DashboardUserConfiguration : BaseEntityConfiguration<DashboardUser>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DashboardUser> builder)
        {
            // Table name
            builder.ToTable("DashboardUser", "Analytics");
            
            // Properties
            builder.Property(du => du.CanEdit)
                .IsRequired()
                .HasDefaultValue(false);
            
            // Indexes
            builder.HasIndex(du => du.DashboardId);
            builder.HasIndex(du => du.UserId);
            builder.HasIndex(du => du.CanEdit);
            builder.HasIndex(du => new { du.DashboardId, du.UserId }).IsUnique();
            
            // Relationships
            builder.HasOne(du => du.Dashboard)
                .WithMany(d => d.DashboardUsers)
                .HasForeignKey(du => du.DashboardId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasOne(du => du.User)
                .WithMany()
                .HasForeignKey(du => du.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 