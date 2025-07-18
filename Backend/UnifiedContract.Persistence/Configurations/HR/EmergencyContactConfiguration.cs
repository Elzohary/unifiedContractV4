using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Domain.ValueObjects;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class EmergencyContactConfiguration : BaseEntityConfiguration<EmergencyContact>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<EmergencyContact> builder)
        {
            // Table name
            builder.ToTable("EmergencyContacts", "HR");
            
            // Properties
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(e => e.Relationship)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(e => e.PrimaryPhone)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(e => e.SecondaryPhone)
                .HasMaxLength(20);
                
            builder.Property(e => e.Email)
                .HasMaxLength(100);
                
            builder.Property(e => e.IsPrimaryContact)
                .IsRequired()
                .HasDefaultValue(false);
                
            // Value object mapping for Address
            builder.OwnsOne(e => e.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(200).HasColumnName("AddressStreet");
                address.Property(a => a.City).HasMaxLength(100).HasColumnName("AddressCity");
                address.Property(a => a.State).HasMaxLength(100).HasColumnName("AddressState");
                address.Property(a => a.PostalCode).HasMaxLength(20).HasColumnName("AddressPostalCode");
                address.Property(a => a.Country).HasMaxLength(100).HasColumnName("AddressCountry");
                address.Property(a => a.FormattedAddress).HasMaxLength(500).HasColumnName("AddressFormattedAddress");
                address.Property(a => a.Latitude).HasColumnName("AddressLatitude");
                address.Property(a => a.Longitude).HasColumnName("AddressLongitude");
            });
            
            // Indexes
            builder.HasIndex(e => e.EmployeeId);
            builder.HasIndex(e => e.Name);
            builder.HasIndex(e => e.Relationship);
            builder.HasIndex(e => e.IsPrimaryContact);
            builder.HasIndex(e => new { e.EmployeeId, e.IsPrimaryContact });
            builder.HasIndex(e => new { e.EmployeeId, e.Name }).IsUnique();
            
            // Relationships
            builder.HasOne(e => e.Employee)
                .WithMany(e => e.EmergencyContacts)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 