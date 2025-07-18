using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class AllowanceConfiguration : BaseEntityConfiguration<Allowance>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Allowance> builder)
        {
            // Table name
            builder.ToTable("Allowances", "HR");
            
            // Properties
            builder.Property(a => a.Type)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(a => a.Description)
                .HasMaxLength(500);
                
            builder.Property(a => a.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
                
            builder.Property(a => a.IsTaxable)
                .IsRequired()
                .HasDefaultValue(false);
                
            builder.Property(a => a.EffectiveDate)
                .IsRequired();
                
            builder.Property(a => a.EndDate);
            
            // Indexes
            builder.HasIndex(a => a.SalaryId);
            builder.HasIndex(a => a.Type);
            builder.HasIndex(a => a.EffectiveDate);
            builder.HasIndex(a => a.EndDate);
            builder.HasIndex(a => a.IsTaxable);
            
            // Relationships
            builder.HasOne(a => a.Salary)
                .WithMany(s => s.Allowances)
                .HasForeignKey(a => a.SalaryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 