using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Resource;

namespace UnifiedContract.Persistence.Configurations.Resource
{
    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            builder.HasKey(x => x.Id);
            // Only configure properties that exist in the Equipment entity
            // Remove or update all references to non-existent properties
            // Ensure navigation property names match the entity class
        }
    }
} 