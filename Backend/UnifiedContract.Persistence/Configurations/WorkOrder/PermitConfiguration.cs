using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class PermitConfiguration : IEntityTypeConfiguration<UnifiedContract.Domain.Entities.WorkOrder.Permit>
    {
        public void Configure(EntityTypeBuilder<UnifiedContract.Domain.Entities.WorkOrder.Permit> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Type).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Number).HasMaxLength(50);
            builder.Property(x => x.IssueDate).IsRequired();
            builder.Property(x => x.ExpiryDate).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.IssuedBy).HasMaxLength(100);
            builder.Property(x => x.Authority).HasMaxLength(100);
            builder.Property(x => x.DocumentRef).HasMaxLength(200);
            builder.Property(x => x.WorkOrderId).IsRequired();
            // Keep any extra backend fields for future frontend use
            builder.HasOne<UnifiedContract.Domain.Entities.WorkOrder.WorkOrder>()
                .WithMany(x => x.Permits)
                .HasForeignKey(x => x.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 