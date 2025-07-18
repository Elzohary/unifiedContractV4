using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Resource
{
    public class ReceivableMaterialConfiguration : BaseEntityConfiguration<ReceivableMaterial>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ReceivableMaterial> builder)
        {
            // Table name
            builder.ToTable("ReceivableMaterials", "Resource");
            
            // Properties
            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(500);
                
            builder.Property(m => m.Unit)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(m => m.EstimatedQuantity)
                .IsRequired()
                .HasPrecision(18, 2);
                
            builder.Property(m => m.ReceivedQuantity)
                .HasPrecision(18, 2);
                
            builder.Property(m => m.ActualQuantity)
                .HasPrecision(18, 2);
                
            builder.Property(m => m.RemainingQuantity)
                .HasPrecision(18, 2);
                
            builder.Property(m => m.ReturnedQuantity)
                .HasPrecision(18, 2);
                
            builder.Property(m => m.SourceLocation)
                .HasMaxLength(200);
                
            builder.Property(m => m.Notes)
                .HasMaxLength(1000);
                
            builder.Property(m => m.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);
                
            builder.Property(m => m.ReceivedDate);
            builder.Property(m => m.ReturnedDate);
            
            // Relationships
            builder.HasOne(m => m.ReceivedBy)
                .WithMany()
                .HasForeignKey(m => m.ReceivedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(m => m.ReturnedBy)
                .WithMany()
                .HasForeignKey(m => m.ReturnedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(m => m.WorkOrder)
                .WithMany()
                .HasForeignKey(m => m.WorkOrderId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(m => m.MaterialType)
                .WithMany()
                .HasForeignKey(m => m.MaterialTypeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Relationship with ClientMaterial
            builder.HasOne(m => m.ClientMaterial)
                .WithMany(cm => cm.ReceivableMaterials)
                .HasForeignKey(m => m.ClientMaterialId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Indexes
            builder.HasIndex(m => m.Status);
            builder.HasIndex(m => m.WorkOrderId);
            builder.HasIndex(m => m.ClientMaterialId);
        }
    }
} 