using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Client;
using UnifiedContract.Domain.ValueObjects;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Client
{
    public class ClientConfiguration : BaseEntityConfiguration<UnifiedContract.Domain.Entities.Client.Client>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<UnifiedContract.Domain.Entities.Client.Client> builder)
        {
            // Table name
            builder.ToTable("Client", "Client");
            
            // Properties
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(c => c.CompanyName)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(c => c.ContactPerson)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(c => c.AlternatePhone)
                .HasMaxLength(20);
                
            builder.Property(c => c.Website)
                .HasMaxLength(200);
                
            builder.Property(c => c.Industry)
                .HasMaxLength(100);
                
            builder.Property(c => c.VatNumber)
                .HasMaxLength(50);
                
            builder.Property(c => c.LogoUrl)
                .HasMaxLength(500);
                
            builder.Property(c => c.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
                
            builder.Property(c => c.Notes)
                .HasMaxLength(1000);
                
            // Value object mapping for Address
            builder.OwnsOne(c => c.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(200).HasColumnName("Street");
                address.Property(a => a.City).HasMaxLength(100).HasColumnName("City");
                address.Property(a => a.State).HasMaxLength(100).HasColumnName("State");
                address.Property(a => a.Country).HasMaxLength(100).HasColumnName("Country");
                address.Property(a => a.PostalCode).HasMaxLength(20).HasColumnName("PostalCode");
            });
            
            // Indexes
            builder.HasIndex(c => c.Name);
            builder.HasIndex(c => c.CompanyName);
            builder.HasIndex(c => c.Email);
            builder.HasIndex(c => c.Phone);
            builder.HasIndex(c => c.VatNumber);
            builder.HasIndex(c => c.IsActive);
            builder.HasIndex(c => c.AccountManagerId);
            builder.HasIndex(c => new { c.Name, c.IsActive });
            builder.HasIndex(c => new { c.CompanyName, c.IsActive });
            
            // Relationships
            builder.HasOne(c => c.AccountManager)
                .WithMany()
                .HasForeignKey(c => c.AccountManagerId)
                .OnDelete(DeleteBehavior.SetNull);
                
            builder.HasMany(c => c.WorkOrders)
                .WithOne(w => w.Client)
                .HasForeignKey(w => w.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasMany(c => c.Contacts)
                .WithOne(cc => cc.Client)
                .HasForeignKey(cc => cc.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 