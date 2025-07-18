using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class WorkOrderInvoiceConfiguration : IEntityTypeConfiguration<WorkOrderInvoice>
    {
        public void Configure(EntityTypeBuilder<WorkOrderInvoice> builder)
        {
            builder.ToTable("WorkOrderInvoices");

            builder.HasKey(invoice => invoice.Id);
            
            builder.Property(invoice => invoice.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(invoice => invoice.InvoiceStatus)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(invoice => invoice.Amount)
                .HasPrecision(18, 2);
                
            builder.Property(invoice => invoice.Description)
                .HasMaxLength(1000);
                
            // Relationships
            builder.HasOne<Domain.Entities.WorkOrder.WorkOrder>()
                .WithMany(wo => wo.WorkOrderInvoices)
                .HasForeignKey(invoice => invoice.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Audit properties
            builder.Property(invoice => invoice.CreatedBy)
                .HasMaxLength(50);
                
            builder.Property(invoice => invoice.LastModifiedBy)
                .HasMaxLength(50);
        }
    }
} 