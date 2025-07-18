using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class WorkOrderTaskConfiguration : IEntityTypeConfiguration<WorkOrderTask>
    {
        public void Configure(EntityTypeBuilder<WorkOrderTask> builder)
        {
            builder.ToTable("WorkOrderTasks");

            builder.HasKey(task => task.Id);
            
            builder.Property(task => task.Title)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(task => task.Description)
                .HasMaxLength(1000);
                
            builder.Property(task => task.Status)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(task => task.Priority)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(task => task.Category)
                .HasMaxLength(50);
                
            builder.Property(task => task.EstimatedHours)
                .HasPrecision(10, 2);
                
            builder.Property(task => task.ActualHours)
                .HasPrecision(10, 2);
                
            // Relationships
            builder.HasOne<Domain.Entities.WorkOrder.WorkOrder>()
                .WithMany(wo => wo.WorkOrderTasks)
                .HasForeignKey(task => task.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // User assignment (assuming User is an entity in your system)
            builder.Property(task => task.AssignedToId);
                
            // Audit properties
            builder.Property(task => task.CreatedBy)
                .HasMaxLength(50);
                
            builder.Property(task => task.LastModifiedBy)
                .HasMaxLength(50);
        }
    }
} 