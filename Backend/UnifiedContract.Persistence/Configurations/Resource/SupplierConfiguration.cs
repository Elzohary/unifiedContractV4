using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Domain.ValueObjects;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Resource
{
    public class SupplierConfiguration : BaseEntityConfiguration<Supplier>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Supplier> builder)
        {
            // Table name
            builder.ToTable("Supplier", "Resource");
            
            // Properties
            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(s => s.ContactPerson)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(s => s.Phone)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(s => s.AlternatePhone)
                .HasMaxLength(20);
                
            builder.Property(s => s.Website)
                .HasMaxLength(200);
                
            builder.Property(s => s.VatNumber)
                .HasMaxLength(50);
                
            builder.Property(s => s.PaymentTerms)
                .HasMaxLength(100);
                
            builder.Property(s => s.BankAccount)
                .HasMaxLength(50);
                
            builder.Property(s => s.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
                
            builder.Property(s => s.Notes)
                .HasMaxLength(1000);
                
            builder.Property(s => s.Rating)
                .HasColumnType("decimal(3,2)")
                .HasDefaultValue(0);
                
            // Value object mapping for Address
            builder.OwnsOne(s => s.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(200).HasColumnName("Street");
                address.Property(a => a.City).HasMaxLength(100).HasColumnName("City");
                address.Property(a => a.State).HasMaxLength(100).HasColumnName("State");
                address.Property(a => a.Country).HasMaxLength(100).HasColumnName("Country");
                address.Property(a => a.PostalCode).HasMaxLength(20).HasColumnName("PostalCode");
            });
            
            // Indexes
            builder.HasIndex(s => s.Name);
            builder.HasIndex(s => s.Email);
            builder.HasIndex(s => s.Phone);
            builder.HasIndex(s => s.VatNumber);
            builder.HasIndex(s => s.IsActive);
            builder.HasIndex(s => s.CategoryId);
            builder.HasIndex(s => new { s.Name, s.IsActive });
            builder.HasIndex(s => new { s.Email, s.IsActive });
            
            // Relationships
            builder.HasOne(s => s.Category)
                .WithMany()
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
                
            builder.HasMany(s => s.Materials)
                .WithOne(m => m.Supplier)
                .HasForeignKey(m => m.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasMany(s => s.Equipment)
                .WithOne(e => e.Supplier)
                .HasForeignKey(e => e.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 