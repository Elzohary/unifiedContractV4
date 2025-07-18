using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class DeductionConfiguration : BaseEntityConfiguration<Deduction>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Deduction> builder)
        {
            // Table name
            builder.ToTable("Deductions", "HR");
            
            // Properties
            builder.Property(d => d.Type)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(d => d.Description)
                .HasMaxLength(500);
                
            builder.Property(d => d.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
                
            builder.Property(d => d.IsMandatory)
                .IsRequired()
                .HasDefaultValue(false);
                
            builder.Property(d => d.EffectiveDate)
                .IsRequired();
                
            builder.Property(d => d.EndDate);
            
            // Indexes
            builder.HasIndex(d => d.SalaryId);
            builder.HasIndex(d => d.Type);
            builder.HasIndex(d => d.EffectiveDate);
            builder.HasIndex(d => d.EndDate);
            builder.HasIndex(d => d.IsMandatory);
            
            // Relationships
            builder.HasOne(d => d.Salary)
                .WithMany(s => s.Deductions)
                .HasForeignKey(d => d.SalaryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 