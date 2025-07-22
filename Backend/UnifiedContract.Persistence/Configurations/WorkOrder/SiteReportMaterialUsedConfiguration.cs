using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class SiteReportMaterialUsedConfiguration : IEntityTypeConfiguration<SiteReportMaterialUsed>
    {
        public void Configure(EntityTypeBuilder<SiteReportMaterialUsed> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.MaterialName).HasMaxLength(200);
            
            builder.Property(x => x.Quantity)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.HasOne(x => x.SiteReport)
                .WithMany(x => x.MaterialsUsed)
                .HasForeignKey(x => x.SiteReportId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.MaterialAssignment)
                .WithMany()
                .HasForeignKey(x => x.MaterialAssignmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 