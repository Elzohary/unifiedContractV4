using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Resource
{
    public class ManpowerAssignmentConfiguration : BaseEntityConfiguration<ManpowerAssignment>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ManpowerAssignment> builder)
        {
            // Table name
            builder.ToTable("ManpowerAssignment", "Resource");
            
            // Properties
            builder.Property(m => m.BadgeNumber)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(m => m.Role)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(m => m.HoursAssigned)
                .IsRequired();
                
            builder.Property(m => m.StartDate)
                .IsRequired();
                
            builder.Property(m => m.EndDate);
                
            builder.Property(m => m.Notes)
                .HasMaxLength(1000);
                
            builder.Property(m => m.WorkOrderNumber)
                .IsRequired()
                .HasMaxLength(50);
            
            // Indexes
            builder.HasIndex(m => m.BadgeNumber);
            builder.HasIndex(m => m.Name);
            builder.HasIndex(m => m.Role);
            builder.HasIndex(m => m.WorkOrderId);
            builder.HasIndex(m => m.UserId);
            builder.HasIndex(m => m.EmployeeId);
            builder.HasIndex(m => m.StartDate);
            builder.HasIndex(m => m.EndDate);
            builder.HasIndex(m => new { m.WorkOrderId, m.StartDate });
            builder.HasIndex(m => new { m.EmployeeId, m.StartDate });
            
            // Relationships
            builder.HasOne(m => m.WorkOrder)
                .WithMany()
                .HasForeignKey(m => m.WorkOrderId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.SetNull);
                
            builder.HasOne(m => m.Employee)
                .WithMany()
                .HasForeignKey(m => m.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 