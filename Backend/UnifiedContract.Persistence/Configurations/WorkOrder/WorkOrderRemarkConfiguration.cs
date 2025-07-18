using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class WorkOrderRemarkConfiguration : IEntityTypeConfiguration<WorkOrderRemark>
    {
        public void Configure(EntityTypeBuilder<WorkOrderRemark> builder)
        {
            builder.ToTable("WorkOrderRemarks");

            builder.HasKey(remark => remark.Id);
            
            builder.Property(remark => remark.Content)
                .IsRequired()
                .HasMaxLength(2000);
                
            builder.Property(remark => remark.RemarkType)
                .HasMaxLength(50);
                
            // Relationships
            builder.HasOne<Domain.Entities.WorkOrder.WorkOrder>()
                .WithMany(wo => wo.WorkOrderRemarks)
                .HasForeignKey(remark => remark.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Audit properties
            builder.Property(remark => remark.CreatedBy)
                .HasMaxLength(50);
                
            builder.Property(remark => remark.LastModifiedBy)
                .HasMaxLength(50);
        }
    }
} 