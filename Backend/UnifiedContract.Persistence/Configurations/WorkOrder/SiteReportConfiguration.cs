using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class SiteReportConfiguration : IEntityTypeConfiguration<SiteReport>
    {
        public void Configure(EntityTypeBuilder<SiteReport> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ForemanName).HasMaxLength(100);
            builder.Property(x => x.WorkDone).HasMaxLength(500);
            builder.Property(x => x.Notes).HasMaxLength(1000);
            
            builder.Property(x => x.ActualQuantity)
                .HasPrecision(18, 2);

            builder.HasOne(x => x.WorkOrder)
                .WithMany(x => x.SiteReports)
                .HasForeignKey(x => x.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.MaterialsUsed)
                .WithOne(x => x.SiteReport)
                .HasForeignKey(x => x.SiteReportId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Photos)
                .WithOne(x => x.SiteReport)
                .HasForeignKey(x => x.SiteReportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 