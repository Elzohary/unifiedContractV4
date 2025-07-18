using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class AttendanceConfiguration : BaseEntityConfiguration<Attendance>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Attendance> builder)
        {
            // Table name
            builder.ToTable("Attendances", "HR");
            
            // Properties
            builder.Property(a => a.Date)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
                
            builder.Property(a => a.CheckInTime);
            
            builder.Property(a => a.CheckOutTime);
            
            builder.Property(a => a.WorkingHours)
                .HasPrecision(5, 2);
            
            builder.Property(a => a.LateMinutes)
                .HasPrecision(5, 2);
            
            builder.Property(a => a.EarlyDepartureMinutes)
                .HasPrecision(5, 2);
            
            builder.Property(a => a.OvertimeHours)
                .HasPrecision(5, 2);
            
            builder.Property(a => a.Notes)
                .HasMaxLength(500);
                
            builder.Property(a => a.IsAbsent)
                .IsRequired()
                .HasDefaultValue(false);
                
            builder.Property(a => a.IsHalfDay)
                .IsRequired()
                .HasDefaultValue(false);
                
            builder.Property(a => a.IsOnLeave)
                .IsRequired()
                .HasDefaultValue(false);
                
            builder.Property(a => a.CheckInLocation)
                .HasMaxLength(200);
                
            builder.Property(a => a.CheckOutLocation)
                .HasMaxLength(200);
            
            // Indexes
            builder.HasIndex(a => a.EmployeeId);
            builder.HasIndex(a => a.Date);
            builder.HasIndex(a => a.LeaveId);
            builder.HasIndex(a => a.IsAbsent);
            builder.HasIndex(a => a.IsOnLeave);
            builder.HasIndex(a => new { a.EmployeeId, a.Date }).IsUnique();
            builder.HasIndex(a => a.IsHalfDay);
            builder.HasIndex(a => new { a.Date, a.IsAbsent });
            
            // Relationships
            builder.HasOne(a => a.Employee)
                .WithMany(e => e.Attendances)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasOne(a => a.Leave)
                .WithMany()
                .HasForeignKey(a => a.LeaveId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 