using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class WorkOrderIssueConfiguration : IEntityTypeConfiguration<UnifiedContract.Domain.Entities.WorkOrder.WorkOrderIssue>
    {
        public void Configure(EntityTypeBuilder<UnifiedContract.Domain.Entities.WorkOrder.WorkOrderIssue> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.Priority).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Severity).IsRequired();
            builder.Property(x => x.ReportedById).IsRequired();
            builder.Property(x => x.ReportedDate).IsRequired();
            builder.Property(x => x.WorkOrderId).IsRequired();
            builder.Property(x => x.ResolutionNotes);
            // Keep any extra backend fields for future frontend use
            builder.HasOne<UnifiedContract.Domain.Entities.WorkOrder.WorkOrder>()
                .WithMany(x => x.Issues)
                .HasForeignKey(x => x.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 