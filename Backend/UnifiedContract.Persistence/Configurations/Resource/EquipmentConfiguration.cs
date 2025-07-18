using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Domain.ValueObjects;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Resource
{
    public class EquipmentConfiguration : BaseEntityConfiguration<Equipment>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Equipment> builder)
        {
            // Table name
            builder.ToTable("Equipment", "Resource");
            
            // Properties
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.CompanyNumber)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(e => e.Model)
                .HasMaxLength(100);
            builder.Property(e => e.SerialNumber)
                .HasMaxLength(100);
            builder.Property(e => e.Manufacturer)
                .HasMaxLength(100);
            builder.Property(e => e.DailyCost)
                .HasColumnType("decimal(18,2)");
            builder.Property(e => e.Currency)
                .HasMaxLength(3)
                .HasDefaultValue("SAR");
            builder.Property(e => e.PurchaseDate)
                .IsRequired();
            builder.Property(e => e.CurrentLocation)
                .HasMaxLength(200);
            builder.Property(e => e.Description)
                .HasMaxLength(1000);
            builder.Property(e => e.ImageUrl)
                .HasMaxLength(500);
            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
            builder.Property(e => e.Condition)
                .HasMaxLength(50);
            builder.Property(e => e.Notes)
                .HasMaxLength(1000);
            
            // Value object mapping for PurchaseInfo
            builder.OwnsOne(e => e.PurchaseInfo, purchase =>
            {
                purchase.Property(p => p.PurchaseDate).HasColumnName("PurchaseDate");
                purchase.Property(p => p.PurchasePrice).HasColumnType("decimal(18,2)").HasColumnName("PurchasePrice");
                purchase.Property(p => p.PurchaseCurrency).HasMaxLength(3).HasColumnName("PurchaseCurrency");
                purchase.Property(p => p.PurchaseFrom).HasMaxLength(100).HasColumnName("PurchaseFrom");
                purchase.Property(p => p.PurchaseDocument).HasMaxLength(500).HasColumnName("PurchaseDocument");
            });
            
            // Value object mapping for MaintenanceInfo
            builder.OwnsOne(e => e.MaintenanceInfo, maintenance =>
            {
                maintenance.Property(m => m.LastMaintenanceDate).HasColumnName("LastMaintenanceDate");
                maintenance.Property(m => m.NextMaintenanceDate).HasColumnName("NextMaintenanceDate");
                maintenance.Property(m => m.MaintenanceFrequencyDays).HasColumnName("MaintenanceFrequencyDays");
                maintenance.Property(m => m.MaintenanceNotes).HasMaxLength(1000).HasColumnName("MaintenanceNotes");
            });
            
            // Indexes
            builder.HasIndex(e => e.CompanyNumber).IsUnique();
            builder.HasIndex(e => e.Name);
            builder.HasIndex(e => e.Type);
            builder.HasIndex(e => e.SerialNumber);
            builder.HasIndex(e => e.Status);
            builder.HasIndex(e => new { e.Status, e.Type });
            
            // Relationships
            builder.HasMany(e => e.MaintenanceRecords)
                .WithOne(m => m.Equipment)
                .HasForeignKey(m => m.EquipmentId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(e => e.Assignments)
                .WithOne(a => a.Equipment)
                .HasForeignKey(a => a.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 