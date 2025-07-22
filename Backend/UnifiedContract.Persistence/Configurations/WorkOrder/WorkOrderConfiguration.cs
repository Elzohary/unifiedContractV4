using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class WorkOrderConfiguration : IEntityTypeConfiguration<UnifiedContract.Domain.Entities.WorkOrder.WorkOrder>
    {
        public void Configure(EntityTypeBuilder<UnifiedContract.Domain.Entities.WorkOrder.WorkOrder> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.WorkOrderNumber).IsRequired().HasMaxLength(50);
            builder.Property(x => x.InternalOrderNumber).HasMaxLength(50);
            builder.Property(x => x.Title).HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.Location).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Category).HasMaxLength(100);
            builder.Property(x => x.CompletionPercentage).HasColumnType("decimal(5,2)");
            builder.Property(x => x.ReceivedDate).IsRequired();
            builder.Property(x => x.StartDate);
            builder.Property(x => x.DueDate);
            builder.Property(x => x.TargetEndDate);
            builder.Property(x => x.EstimatedCost).HasColumnType("decimal(18,2)");
            builder.Property(x => x.MaterialsExpense).HasColumnType("decimal(18,2)");
            builder.Property(x => x.LaborExpense).HasColumnType("decimal(18,2)");
            builder.Property(x => x.OtherExpense).HasColumnType("decimal(18,2)");

            builder.HasMany(x => x.Items).WithOne().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Remarks).WithOne().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Issues).WithOne().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Tasks).WithOne().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Permits).WithOne().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Actions).WithOne().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Photos).WithOne().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Forms).WithOne().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Expenses).WithOne().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Invoices).WithOne().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
        }
    }
} 