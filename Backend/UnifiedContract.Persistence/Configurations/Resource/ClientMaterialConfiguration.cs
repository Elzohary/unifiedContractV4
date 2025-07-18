using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Resource
{
    public class ClientMaterialConfiguration : BaseEntityConfiguration<ClientMaterial>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ClientMaterial> builder)
        {
            // Table name
            builder.ToTable("ClientMaterials", "Resource");
            
            // Properties
            builder.Property(m => m.GroupCode)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(m => m.SEQ)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(m => m.MaterialMasterCode)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(500);
                
            builder.Property(m => m.Unit)
                .IsRequired()
                .HasMaxLength(50);
                
            // Relationship with Client
            builder.HasOne(m => m.Client)
                .WithMany()
                .HasForeignKey(m => m.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Indexes for fast lookup
            builder.HasIndex(m => m.ClientId);
            builder.HasIndex(m => m.MaterialMasterCode);
            builder.HasIndex(m => new { m.ClientId, m.MaterialMasterCode }).IsUnique();
        }
    }
} 