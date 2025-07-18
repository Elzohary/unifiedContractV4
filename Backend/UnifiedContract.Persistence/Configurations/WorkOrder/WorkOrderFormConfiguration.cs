using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class WorkOrderFormConfiguration : IEntityTypeConfiguration<WorkOrderForm>
    {
        public void Configure(EntityTypeBuilder<WorkOrderForm> builder)
        {
            builder.ToTable("WorkOrderForms");

            builder.HasKey(form => form.Id);
            
            builder.Property(form => form.FormName)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(form => form.FormType)
                .HasMaxLength(50);
                
            builder.Property(form => form.FormData)
                .HasColumnType("nvarchar(max)");
                
            // Relationships
            builder.HasOne<Domain.Entities.WorkOrder.WorkOrder>()
                .WithMany(wo => wo.WorkOrderForms)
                .HasForeignKey(form => form.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Audit properties
            builder.Property(form => form.CreatedBy)
                .HasMaxLength(50);
                
            builder.Property(form => form.LastModifiedBy)
                .HasMaxLength(50);
        }
    }
} 