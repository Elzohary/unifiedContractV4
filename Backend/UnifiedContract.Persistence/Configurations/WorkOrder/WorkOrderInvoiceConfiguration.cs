using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class WorkOrderInvoiceConfiguration : IEntityTypeConfiguration<UnifiedContract.Domain.Entities.WorkOrder.WorkOrderInvoice>
    {
        public void Configure(EntityTypeBuilder<UnifiedContract.Domain.Entities.WorkOrder.WorkOrderInvoice> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Number).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.Currency).IsRequired().HasMaxLength(10);
            builder.Property(x => x.IssueDate).IsRequired();
            builder.Property(x => x.DueDate).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.PaidDate);
            builder.Property(x => x.PaidById);
            builder.Property(x => x.Url).HasMaxLength(500);
            builder.Property(x => x.WorkOrderId).IsRequired();
            // Keep any extra backend fields for future frontend use
            builder.HasOne<UnifiedContract.Domain.Entities.WorkOrder.WorkOrder>()
                .WithMany(x => x.Invoices)
                .HasForeignKey(x => x.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 