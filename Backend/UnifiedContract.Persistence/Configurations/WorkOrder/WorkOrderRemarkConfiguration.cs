using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class WorkOrderRemarkConfiguration : IEntityTypeConfiguration<UnifiedContract.Domain.Entities.WorkOrder.WorkOrderRemark>
    {
        public void Configure(EntityTypeBuilder<UnifiedContract.Domain.Entities.WorkOrder.WorkOrderRemark> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Content).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.WorkOrderId).IsRequired();
            builder.Property(x => x.PeopleInvolved);
            // Keep any extra backend fields for future frontend use
            builder.HasOne<UnifiedContract.Domain.Entities.WorkOrder.WorkOrder>()
                .WithMany(x => x.Remarks)
                .HasForeignKey(x => x.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 