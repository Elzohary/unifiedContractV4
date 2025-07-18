using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Analytics;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Analytics
{
    public class DashboardConfiguration : BaseEntityConfiguration<Dashboard>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Dashboard> builder)
        {
            // Table name
            builder.ToTable("Dashboard", "Analytics");
            
            // Properties
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(d => d.Description)
                .HasMaxLength(500);
                
            builder.Property(d => d.Module)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(d => d.IsDefault)
                .IsRequired()
                .HasDefaultValue(false);
                
            builder.Property(d => d.IsSystem)
                .IsRequired()
                .HasDefaultValue(false);
                
            builder.Property(d => d.IsPublic)
                .IsRequired()
                .HasDefaultValue(false);
            
            // Indexes
            builder.HasIndex(d => d.Name);
            builder.HasIndex(d => d.Module);
            builder.HasIndex(d => d.OwnerId);
            builder.HasIndex(d => d.IsDefault);
            builder.HasIndex(d => d.IsSystem);
            builder.HasIndex(d => d.IsPublic);
            builder.HasIndex(d => new { d.Module, d.IsDefault });
            builder.HasIndex(d => new { d.OwnerId, d.IsPublic });
            
            // Relationships
            builder.HasOne(d => d.Owner)
                .WithMany()
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);
                
            builder.HasMany(d => d.Widgets)
                .WithOne(w => w.Dashboard)
                .HasForeignKey(w => w.DashboardId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(d => d.DashboardUsers)
                .WithOne(du => du.Dashboard)
                .HasForeignKey(du => du.DashboardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 