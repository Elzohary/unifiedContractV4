using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Analytics;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Analytics
{
    public class DashboardWidgetConfiguration : BaseEntityConfiguration<DashboardWidget>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DashboardWidget> builder)
        {
            // Table name
            builder.ToTable("DashboardWidget", "Analytics");
            
            // Properties
            builder.Property(w => w.Title)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(w => w.Description)
                .HasMaxLength(500);
                
            builder.Property(w => w.Type)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(w => w.ChartType)
                .HasMaxLength(50);
                
            builder.Property(w => w.DataSource)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(w => w.Configuration)
                .HasColumnType("nvarchar(max)");
                
            builder.Property(w => w.Width)
                .IsRequired()
                .HasDefaultValue(4);
                
            builder.Property(w => w.Height)
                .IsRequired()
                .HasDefaultValue(300);
                
            builder.Property(w => w.Position)
                .IsRequired()
                .HasDefaultValue(0);
                
            builder.Property(w => w.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
            
            // Indexes
            builder.HasIndex(w => w.Title);
            builder.HasIndex(w => w.Type);
            builder.HasIndex(w => w.ChartType);
            builder.HasIndex(w => w.DashboardId);
            builder.HasIndex(w => w.IsActive);
            builder.HasIndex(w => w.Position);
            builder.HasIndex(w => new { w.DashboardId, w.Position });
            builder.HasIndex(w => new { w.DashboardId, w.IsActive });
            
            // Relationships
            builder.HasOne(w => w.Dashboard)
                .WithMany(d => d.Widgets)
                .HasForeignKey(w => w.DashboardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 