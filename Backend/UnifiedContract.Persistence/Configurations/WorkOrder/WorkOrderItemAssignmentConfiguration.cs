using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class WorkOrderItemAssignmentConfiguration : IEntityTypeConfiguration<WorkOrderItemAssignment>
    {
        public void Configure(EntityTypeBuilder<WorkOrderItemAssignment> builder)
        {
            builder.ToTable("WorkOrderItemAssignments");

            builder.HasKey(assignment => assignment.Id);
            
            builder.Property(assignment => assignment.EstimatedQuantity)
                .HasPrecision(18, 2);
                
            builder.Property(assignment => assignment.EstimatedPrice)
                .HasPrecision(18, 2);
                
            builder.Property(assignment => assignment.EstimatedPriceWithVAT)
                .HasPrecision(18, 2);
                
            builder.Property(assignment => assignment.ActualQuantity)
                .HasPrecision(18, 2);
                
            builder.Property(assignment => assignment.ActualPrice)
                .HasPrecision(18, 2);
                
            builder.Property(assignment => assignment.ActualPriceWithVAT)
                .HasPrecision(18, 2);
                
            builder.Property(assignment => assignment.ReasonForFinalQuantity)
                .HasMaxLength(500);
                
            // Relationships
            builder.HasOne(assignment => assignment.WorkOrder)
                .WithMany(wo => wo.ItemAssignments)
                .HasForeignKey(assignment => assignment.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasOne(assignment => assignment.Item)
                .WithMany(item => item.WorkOrderAssignments)
                .HasForeignKey(assignment => assignment.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Audit properties
            builder.Property(assignment => assignment.CreatedBy)
                .HasMaxLength(50);
                
            builder.Property(assignment => assignment.LastModifiedBy)
                .HasMaxLength(50);
        }
    }
} 