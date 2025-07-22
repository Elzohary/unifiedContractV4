using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class IdentificationConfiguration : BaseEntityConfiguration<Identification>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Identification> builder)
        {
            // Table name
            builder.ToTable("Identifications", "HR");
            
            // Properties
            builder.Property(i => i.Type)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(i => i.Number)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(i => i.IssuingCountry)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(i => i.DocumentUrl)
                .HasMaxLength(500);
            
            // Indexes
            builder.HasIndex(i => i.EmployeeId);
            builder.HasIndex(i => i.Type);
            builder.HasIndex(i => i.Number);
            builder.HasIndex(i => i.IssuingCountry);
            builder.HasIndex(i => i.IssueDate);
            builder.HasIndex(i => i.ExpiryDate);
            builder.HasIndex(i => new { i.Type, i.Number }).IsUnique();
            
            // Relationships - configured in EmployeeConfiguration to avoid duplicate foreign keys
        }
    }
} 