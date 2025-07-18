using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class WorkOrderActionConfiguration : IEntityTypeConfiguration<WorkOrderAction>
    {
        public void Configure(EntityTypeBuilder<WorkOrderAction> builder)
        {
            builder.ToTable("WorkOrderActions");

            builder.HasKey(action => action.Id);
            
            builder.Property(action => action.ActionName)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(action => action.Description)
                .HasMaxLength(1000);
                
            builder.Property(action => action.Status)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(action => action.ActionType)
                .HasMaxLength(50);
                
            // Relationships
            builder.HasOne<Domain.Entities.WorkOrder.WorkOrder>()
                .WithMany(wo => wo.WorkOrderActions)
                .HasForeignKey(action => action.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // User assignment (assuming User is an entity in your system)
            builder.Property(action => action.AssignedToId);
                
            // Audit properties
            builder.Property(action => action.CreatedBy)
                .HasMaxLength(50);
                
            builder.Property(action => action.LastModifiedBy)
                .HasMaxLength(50);
        }
    }
} 