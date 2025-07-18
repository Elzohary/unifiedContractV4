using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class WorkExperienceConfiguration : BaseEntityConfiguration<WorkExperience>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WorkExperience> builder)
        {
            // Table name
            builder.ToTable("WorkExperiences", "HR");
            
            // Properties
            builder.Property(w => w.CompanyName)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(w => w.Position)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(w => w.JobDescription)
                .HasMaxLength(1000);
                
            builder.Property(w => w.Location)
                .HasMaxLength(100);
                
            builder.Property(w => w.IsCurrentEmployer)
                .IsRequired()
                .HasDefaultValue(false);
                
            builder.Property(w => w.ContactReference)
                .HasMaxLength(100);
                
            builder.Property(w => w.ContactReferencePhone)
                .HasMaxLength(20);
                
            builder.Property(w => w.ContactReferenceEmail)
                .HasMaxLength(100);
                
            builder.Property(w => w.Achievements)
                .HasMaxLength(1000);
                
            builder.Property(w => w.ReasonForLeaving)
                .HasMaxLength(500);

            builder.Property(w => w.StartDate)
                .IsRequired();

            builder.Property(w => w.EndDate);
            
            // Indexes
            builder.HasIndex(w => w.EmployeeId);
            builder.HasIndex(w => w.CompanyName);
            builder.HasIndex(w => w.Position);
            builder.HasIndex(w => new { w.StartDate, w.EndDate });
            builder.HasIndex(w => w.IsCurrentEmployer);
            builder.HasIndex(w => new { w.EmployeeId, w.IsCurrentEmployer });
            
            // Relationships
            builder.HasOne(w => w.Employee)
                .WithMany(e => e.WorkExperiences)
                .HasForeignKey(w => w.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 