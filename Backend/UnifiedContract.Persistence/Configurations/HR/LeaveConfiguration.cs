using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class LeaveConfiguration : BaseEntityConfiguration<Leave>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Leave> builder)
        {
            // Table name
            builder.ToTable("Leaves", "HR");
            
            // Properties
            builder.Property(l => l.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
                
            builder.Property(l => l.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(LeaveStatus.Pending);
                
            builder.Property(l => l.Reason)
                .IsRequired()
                .HasMaxLength(500);
                
            builder.Property(l => l.Comments)
                .HasMaxLength(1000);
                
            builder.Property(l => l.RejectionReason)
                .HasMaxLength(500);
                
            builder.Property(l => l.DocumentUrl)
                .HasMaxLength(500);
                
            builder.Property(l => l.TotalDays)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(l => l.StartDate)
                .IsRequired();

            builder.Property(l => l.EndDate)
                .IsRequired();

            builder.Property(l => l.ApprovedDate);

            // Indexes
            builder.HasIndex(l => l.EmployeeId);
            builder.HasIndex(l => l.Type);
            builder.HasIndex(l => l.Status);
            builder.HasIndex(l => l.StartDate);
            builder.HasIndex(l => l.EndDate);
            builder.HasIndex(l => l.ApprovedById);
            builder.HasIndex(l => new { l.StartDate, l.EndDate });
            builder.HasIndex(l => new { l.EmployeeId, l.Status });
            
            // Relationships
            builder.HasOne(l => l.Employee)
                .WithMany(e => e.Leaves)
                .HasForeignKey(l => l.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(l => l.ApprovedBy)
                .WithMany()
                .HasForeignKey(l => l.ApprovedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 