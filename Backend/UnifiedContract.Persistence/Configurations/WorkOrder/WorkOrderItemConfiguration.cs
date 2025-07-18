using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class WorkOrderItemConfiguration : IEntityTypeConfiguration<WorkOrderItem>
    {
        public void Configure(EntityTypeBuilder<WorkOrderItem> builder)
        {
            builder.ToTable("WorkOrderItems");

            builder.HasKey(item => item.Id);
            
            builder.Property(item => item.ItemNumber)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(item => item.Description)
                .HasMaxLength(1000);
                
            builder.Property(item => item.Unit)
                .HasMaxLength(50);
                
            builder.Property(item => item.UnitPrice)
                .HasPrecision(18, 2);
                
            builder.Property(item => item.EstimatedQuantity)
                .HasPrecision(18, 2);
                
            builder.Property(item => item.EstimatedPrice)
                .HasPrecision(18, 2);
                
            builder.Property(item => item.EstimatedPriceWithVAT)
                .HasPrecision(18, 2);
                
            builder.Property(item => item.ActualQuantity)
                .HasPrecision(18, 2);
                
            builder.Property(item => item.ActualPrice)
                .HasPrecision(18, 2);
                
            builder.Property(item => item.ActualPriceWithVAT)
                .HasPrecision(18, 2);
                
            builder.Property(item => item.ReasonForFinalQuantity)
                .HasMaxLength(500);
                
            // Relationships
            builder.HasOne<Domain.Entities.WorkOrder.WorkOrder>()
                .WithMany(wo => wo.Items)
                .HasForeignKey(item => item.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Audit properties
            builder.Property(item => item.CreatedBy)
                .HasMaxLength(50);
                
            builder.Property(item => item.LastModifiedBy)
                .HasMaxLength(50);
        }
    }
} 