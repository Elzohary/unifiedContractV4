using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Client;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Client
{
    public class ClientContactConfiguration : BaseEntityConfiguration<ClientContact>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ClientContact> builder)
        {
            // Table name
            builder.ToTable("ClientContact", "Client");
            
            // Properties
            builder.Property(cc => cc.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(cc => cc.Position)
                .HasMaxLength(100);
                
            builder.Property(cc => cc.Email)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(cc => cc.Phone)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(cc => cc.AlternatePhone)
                .HasMaxLength(20);
                
            builder.Property(cc => cc.IsPrimaryContact)
                .IsRequired()
                .HasDefaultValue(false);
                
            builder.Property(cc => cc.Notes)
                .HasMaxLength(1000);
            
            // Indexes
            builder.HasIndex(cc => cc.Name);
            builder.HasIndex(cc => cc.Email);
            builder.HasIndex(cc => cc.Phone);
            builder.HasIndex(cc => cc.ClientId);
            builder.HasIndex(cc => cc.IsPrimaryContact);
            builder.HasIndex(cc => new { cc.ClientId, cc.IsPrimaryContact });
            builder.HasIndex(cc => new { cc.ClientId, cc.Email });
            
            // Relationships
            builder.HasOne(cc => cc.Client)
                .WithMany(c => c.Contacts)
                .HasForeignKey(cc => cc.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 