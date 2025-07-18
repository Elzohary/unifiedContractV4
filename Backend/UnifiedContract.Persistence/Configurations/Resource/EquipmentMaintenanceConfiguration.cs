using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Resource
{
    public class EquipmentMaintenanceConfiguration : BaseEntityConfiguration<EquipmentMaintenance>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<EquipmentMaintenance> builder)
        {
            // Table name
            builder.ToTable("EquipmentMaintenance", "Resource");
            
            // Properties
            builder.Property(m => m.ScheduledDate)
                .IsRequired();
            builder.Property(m => m.CompletedDate);
            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(1000);
            builder.Property(m => m.Cost)
                .HasColumnType("decimal(18,2)");
            builder.Property(m => m.Currency)
                .HasMaxLength(3)
                .HasDefaultValue("SAR");
            builder.Property(m => m.Notes)
                .HasMaxLength(1000);
            builder.Property(m => m.ServiceProvider)
                .HasMaxLength(100);
            builder.Property(m => m.InvoiceNumber)
                .HasMaxLength(100);
            builder.Property(m => m.DocumentUrl)
                .HasMaxLength(500);
            
            // Indexes
            builder.HasIndex(m => m.EquipmentId);
            builder.HasIndex(m => m.ScheduledDate);
            builder.HasIndex(m => m.CompletedDate);
            builder.HasIndex(m => new { m.EquipmentId, m.ScheduledDate });
            
            // Relationships
            builder.HasOne(m => m.Equipment)
                .WithMany(e => e.MaintenanceRecords)
                .HasForeignKey(m => m.EquipmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 