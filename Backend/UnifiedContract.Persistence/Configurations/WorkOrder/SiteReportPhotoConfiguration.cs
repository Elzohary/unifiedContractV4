using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class SiteReportPhotoConfiguration : IEntityTypeConfiguration<SiteReportPhoto>
    {
        public void Configure(EntityTypeBuilder<SiteReportPhoto> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Url).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Caption).HasMaxLength(200);
            builder.Property(x => x.Category).HasMaxLength(50);
            builder.Property(x => x.UploadedDate).IsRequired();

            builder.HasOne(x => x.SiteReport)
                .WithMany(x => x.Photos)
                .HasForeignKey(x => x.SiteReportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 