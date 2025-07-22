using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class WorkOrderFormConfiguration : IEntityTypeConfiguration<UnifiedContract.Domain.Entities.WorkOrder.WorkOrderForm>
    {
        public void Configure(EntityTypeBuilder<UnifiedContract.Domain.Entities.WorkOrder.WorkOrderForm> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.SubmittedDate);
            builder.Property(x => x.SubmittedById);
            builder.Property(x => x.Url).HasMaxLength(500);
            builder.Property(x => x.WorkOrderId).IsRequired();
            // Keep any extra backend fields for future frontend use
            builder.HasOne<UnifiedContract.Domain.Entities.WorkOrder.WorkOrder>()
                .WithMany(x => x.Forms)
                .HasForeignKey(x => x.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 