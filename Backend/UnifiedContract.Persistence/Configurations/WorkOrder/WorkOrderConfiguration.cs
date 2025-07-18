using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class WorkOrderConfiguration : IEntityTypeConfiguration<Domain.Entities.WorkOrder.WorkOrder>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.WorkOrder.WorkOrder> builder)
        {
            builder.ToTable("WorkOrders");

            builder.HasKey(wo => wo.Id);
            
            builder.Property(wo => wo.WorkOrderNumber)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(wo => wo.InternalOrderNumber)
                .HasMaxLength(50);
                
            builder.Property(wo => wo.Title)
                .HasMaxLength(200);
                
            builder.Property(wo => wo.Description)
                .HasMaxLength(2000);
                
            builder.Property(wo => wo.Location)
                .HasMaxLength(200);
                
            builder.Property(wo => wo.Category)
                .HasMaxLength(50);
                
            builder.Property(wo => wo.CompletionPercentage)
                .HasPrecision(5, 2);
                
            // Date properties
            builder.Property(wo => wo.ReceivedDate);
            builder.Property(wo => wo.StartDate);
            builder.Property(wo => wo.DueDate);
            builder.Property(wo => wo.TargetEndDate);
                
            builder.Property(wo => wo.EstimatedCost)
                .HasPrecision(18, 2);
                
            builder.Property(wo => wo.MaterialsExpense)
                .HasPrecision(18, 2);
                
            builder.Property(wo => wo.LaborExpense)
                .HasPrecision(18, 2);
                
            builder.Property(wo => wo.OtherExpense)
                .HasPrecision(18, 2);
                
            // Foreign keys
            builder.Property(wo => wo.WorkOrderStatusId);
            builder.Property(wo => wo.PriorityLevelId);
            builder.Property(wo => wo.ClientId);
            builder.Property(wo => wo.EngineerInChargeId);
                
            // Navigation properties
            builder.HasOne(wo => wo.Status)
                .WithMany()
                .HasForeignKey(wo => wo.WorkOrderStatusId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(wo => wo.Priority)
                .WithMany()
                .HasForeignKey(wo => wo.PriorityLevelId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(wo => wo.Client)
                .WithMany()
                .HasForeignKey(wo => wo.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(wo => wo.EngineerInCharge)
                .WithMany()
                .HasForeignKey(wo => wo.EngineerInChargeId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Collection relationships
            builder.HasMany(wo => wo.Items)
                .WithOne()
                .HasForeignKey(woi => woi.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(wo => wo.Photos)
                .WithOne()
                .HasForeignKey(wop => wop.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(wo => wo.Remarks)
                .WithOne()
                .HasForeignKey(wor => wor.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(wo => wo.Tasks)
                .WithOne()
                .HasForeignKey(wot => wot.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(wo => wo.Forms)
                .WithOne()
                .HasForeignKey(wof => wof.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(wo => wo.Invoices)
                .WithOne()
                .HasForeignKey(woi => woi.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(wo => wo.Issues)
                .WithOne()
                .HasForeignKey(woi => woi.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(wo => wo.Actions)
                .WithOne()
                .HasForeignKey(woa => woa.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(wo => wo.Expenses)
                .WithOne()
                .HasForeignKey(woe => woe.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(wo => wo.Permits)
                .WithOne()
                .HasForeignKey(p => p.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(wo => wo.ActionsNeeded)
                .WithOne()
                .HasForeignKey(an => an.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(wo => wo.Materials)
                .WithOne()
                .HasForeignKey(ma => ma.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(wo => wo.Manpower)
                .WithOne()
                .HasForeignKey(ma => ma.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(wo => wo.Equipment)
                .WithOne()
                .HasForeignKey(ea => ea.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Audit properties
            builder.Property(wo => wo.CreatedBy)
                .HasMaxLength(50);
                
            builder.Property(wo => wo.LastModifiedBy)
                .HasMaxLength(50);
        }
    }
} 