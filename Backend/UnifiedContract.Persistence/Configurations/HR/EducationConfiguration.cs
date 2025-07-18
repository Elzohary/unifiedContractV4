using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class EducationConfiguration : BaseEntityConfiguration<Education>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Education> builder)
        {
            // Table name
            builder.ToTable("Educations", "HR");
            
            // Properties
            builder.Property(e => e.Institution)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(e => e.Degree)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(e => e.FieldOfStudy)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(e => e.StartDate)
                .IsRequired();
                
            builder.Property(e => e.EndDate);
                
            builder.Property(e => e.Description)
                .HasMaxLength(1000);
                
            builder.Property(e => e.DocumentUrl)
                .HasMaxLength(500);
                
            builder.Property(e => e.IsVerified)
                .IsRequired()
                .HasDefaultValue(false);
            
            // Indexes
            builder.HasIndex(e => e.EmployeeId);
            builder.HasIndex(e => e.Institution);
            builder.HasIndex(e => e.Degree);
            builder.HasIndex(e => e.FieldOfStudy);
            builder.HasIndex(e => new { e.StartDate, e.EndDate });
            builder.HasIndex(e => new { e.EmployeeId, e.IsVerified });
            
            // Relationships
            builder.HasOne(e => e.Employee)
                .WithMany(e => e.EducationHistory)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 