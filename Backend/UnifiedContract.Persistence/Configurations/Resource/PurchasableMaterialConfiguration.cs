using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Resource
{
    public class PurchasableMaterialConfiguration : BaseEntityConfiguration<PurchasableMaterial>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PurchasableMaterial> builder)
        {
            // Table name
            builder.ToTable("PurchasableMaterial", "Resource");
            
            // Properties
            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(m => m.Description)
                .HasMaxLength(500);
                
            builder.Property(m => m.Quantity)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
                
            builder.Property(m => m.Unit)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Units");
                
            builder.Property(m => m.UnitCost)
                .HasColumnType("decimal(18,2)");
                
            builder.Property(m => m.TotalCost)
                .HasColumnType("decimal(18,2)");
                
            builder.Property(m => m.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
                
            builder.Property(m => m.SupplierName)
                .HasMaxLength(100);
                
            builder.Property(m => m.OrderDate);
                
            builder.Property(m => m.DeliveryDate);
            
            // Indexes
            builder.HasIndex(m => m.Name);
            builder.HasIndex(m => m.Status);
            builder.HasIndex(m => m.SupplierId);
            builder.HasIndex(m => m.MaterialTypeId);
            builder.HasIndex(m => m.OrderDate);
            builder.HasIndex(m => m.DeliveryDate);
            builder.HasIndex(m => new { m.Status, m.MaterialTypeId });
            builder.HasIndex(m => new { m.SupplierId, m.Status });
            
            // Relationships
            builder.HasOne(m => m.Supplier)
                .WithMany(s => s.Materials)
                .HasForeignKey(m => m.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);
                
            builder.HasOne(m => m.MaterialType)
                .WithMany()
                .HasForeignKey(m => m.MaterialTypeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 