using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Common;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Auth
{
    /// <summary>
    /// Base configuration for all lookup entities
    /// </summary>
    public abstract class LookupConfiguration<T> : BaseEntityConfiguration<T> where T : Lookup
    {
        protected override void ConfigureEntity(EntityTypeBuilder<T> builder)
        {
            // Table name will be defined in derived classes
            
            // Common properties for all lookup entities
            builder.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(e => e.Description)
                .HasMaxLength(500);
                
            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
                
            builder.Property(e => e.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);
                
            builder.Property(e => e.Metadata)
                .HasMaxLength(1000);
            
            // Common indexes for all lookup entities
            builder.HasIndex(e => e.Code).IsUnique();
            builder.HasIndex(e => e.Name);
            builder.HasIndex(e => e.IsActive);
            builder.HasIndex(e => e.DisplayOrder);
            
            // Call specific configuration for derived lookup entities
            ConfigureLookupEntity(builder);
        }
        
        // To be implemented by specific lookup entity configurations
        protected abstract void ConfigureLookupEntity(EntityTypeBuilder<T> builder);
    }
} 