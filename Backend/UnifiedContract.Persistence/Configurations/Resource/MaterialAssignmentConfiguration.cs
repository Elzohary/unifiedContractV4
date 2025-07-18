using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Resource
{
    public class MaterialAssignmentConfiguration : BaseEntityConfiguration<MaterialAssignment>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<MaterialAssignment> builder)
        {
            // Table name
            builder.ToTable("MaterialAssignment", "Resource");
            
            // Properties
            builder.Property(m => m.AssignDate)
                .IsRequired();
                
            builder.Property(m => m.StoringLocation)
                .HasMaxLength(200);
                
            builder.Property(m => m.WorkOrderNumber)
                .HasMaxLength(50);
                
            builder.Property(m => m.Quantity)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
                
            builder.Property(m => m.Unit)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Units");
                
            builder.Property(m => m.Notes)
                .HasMaxLength(1000);
                
            builder.Property(m => m.MaterialType)
                .IsRequired()
                .HasConversion<string>();
            
            // Indexes
            builder.HasIndex(m => m.AssignDate);
            builder.HasIndex(m => m.WorkOrderId);
            builder.HasIndex(m => m.PurchasableMaterialId);
            builder.HasIndex(m => m.ReceivableMaterialId);
            builder.HasIndex(m => m.AssignedById);
            builder.HasIndex(m => m.MaterialType);
            builder.HasIndex(m => new { m.WorkOrderId, m.AssignDate });
            builder.HasIndex(m => new { m.MaterialType, m.AssignDate });
            
            // Relationships
            builder.HasOne(m => m.WorkOrder)
                .WithMany()
                .HasForeignKey(m => m.WorkOrderId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(m => m.PurchasableMaterial)
                .WithMany()
                .HasForeignKey(m => m.PurchasableMaterialId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(m => m.ReceivableMaterial)
                .WithMany()
                .HasForeignKey(m => m.ReceivableMaterialId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(m => m.AssignedBy)
                .WithMany()
                .HasForeignKey(m => m.AssignedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 