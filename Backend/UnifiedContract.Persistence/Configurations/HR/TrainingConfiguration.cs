using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Domain.Enums;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class TrainingConfiguration : BaseEntityConfiguration<Training>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Training> builder)
        {
            // Table name
            builder.ToTable("Trainings", "HR");
            
            // Properties
            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(t => t.Description)
                .HasMaxLength(1000);
                
            builder.Property(t => t.Provider)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(t => t.Location)
                .HasMaxLength(100);
                
            builder.Property(t => t.Currency)
                .HasMaxLength(3)
                .HasDefaultValue("SAR");
                
            builder.Property(t => t.Cost)
                .HasColumnType("decimal(18,2)");
                
            builder.Property(t => t.CertificateUrl)
                .HasMaxLength(500);
                
            builder.Property(t => t.Feedback)
                .HasMaxLength(1000);
            
            // Add missing property configurations
            builder.Property(t => t.StartDate)
                .IsRequired();

            builder.Property(t => t.EndDate)
                .IsRequired();

            builder.Property(t => t.DurationHours)
                .IsRequired();

            builder.Property(t => t.Score);
            
            // Indexes
            builder.HasIndex(t => t.EmployeeId);
            builder.HasIndex(t => t.Title);
            builder.HasIndex(t => t.Provider);
            builder.HasIndex(t => t.StartDate);
            builder.HasIndex(t => t.EndDate);
            builder.HasIndex(t => t.Status);
            builder.HasIndex(t => new { t.StartDate, t.EndDate });
            builder.HasIndex(t => new { t.EmployeeId, t.Status });
            builder.HasIndex(t => t.Score);
            
            // Relationships
            builder.HasOne(t => t.Employee)
                .WithMany(e => e.Trainings)
                .HasForeignKey(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Enum conversions
            builder.Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();
        }
    }
} 