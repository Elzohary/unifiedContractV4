using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class SalaryConfiguration : BaseEntityConfiguration<Salary>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Salary> builder)
        {
            // Table name
            builder.ToTable("Salaries", "HR");
            
            // Properties
            builder.Property(s => s.BaseSalary)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
                
            builder.Property(s => s.Currency)
                .IsRequired()
                .HasMaxLength(3);
                
            builder.Property(s => s.EffectiveDate)
                .IsRequired();
                
            builder.Property(s => s.EndDate);
                
            builder.Property(s => s.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
                
            builder.Property(s => s.Notes)
                .HasMaxLength(500);
            
            // Indexes
            builder.HasIndex(s => s.EmployeeId);
            builder.HasIndex(s => s.EffectiveDate);
            builder.HasIndex(s => s.EndDate);
            builder.HasIndex(s => s.IsActive);
            builder.HasIndex(s => new { s.EmployeeId, s.EffectiveDate });
            
            // Relationships
            builder.HasOne(s => s.Employee)
                .WithMany(e => e.SalaryHistory)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add missing indexes
            builder.HasIndex(s => new { s.EffectiveDate, s.EndDate });
            builder.HasIndex(s => new { s.EmployeeId, s.IsActive });
        }
    }
} 