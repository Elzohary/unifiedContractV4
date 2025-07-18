using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Domain.ValueObjects;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class EmployeeConfiguration : BaseEntityConfiguration<Employee>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Employee> builder)
        {
            // Table name
            builder.ToTable("Employees", "HR");
            
            // Properties
            builder.Property(e => e.BadgeNumber)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(e => e.Photo)
                .HasMaxLength(500);
                
            builder.Property(e => e.JobTitle)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(e => e.WorkLocation)
                .HasMaxLength(100);
                
            // Value object mapping for Address
            builder.OwnsOne(e => e.HomeAddress, address =>
            {
                address.Property(a => a.Street).HasMaxLength(200).HasColumnName("HomeStreet");
                address.Property(a => a.City).HasMaxLength(100).HasColumnName("HomeCity");
                address.Property(a => a.State).HasMaxLength(100).HasColumnName("HomeState");
                address.Property(a => a.PostalCode).HasMaxLength(20).HasColumnName("HomePostalCode");
                address.Property(a => a.Country).HasMaxLength(100).HasColumnName("HomeCountry");
                address.Property(a => a.FormattedAddress).HasMaxLength(500).HasColumnName("HomeFormattedAddress");
                address.Property(a => a.Latitude).HasColumnName("HomeLatitude");
                address.Property(a => a.Longitude).HasColumnName("HomeLongitude");
            });
                
            builder.Property(e => e.HomeType)
                .HasMaxLength(20);
                
            builder.Property(e => e.CompanyPhone)
                .HasMaxLength(20);
                
            builder.Property(e => e.PersonalPhone)
                .HasMaxLength(20);
                
            builder.Property(e => e.IqamaNumber)
                .HasMaxLength(20);
                
            builder.Property(e => e.Nationality)
                .HasMaxLength(50);
                
            builder.Property(e => e.CurrentProject)
                .HasMaxLength(100);
            
            // Value object mapping for Salary (Money)
            builder.OwnsOne(e => e.Salary, money =>
            {
                money.Property(m => m.Amount).HasColumnType("decimal(18,2)").HasColumnName("SalaryAmount");
                money.Property(m => m.Currency).HasMaxLength(3).HasColumnName("SalaryCurrency").HasDefaultValue("SAR");
            });
            
            // Add missing property configurations
            builder.Property(e => e.Age);
            builder.Property(e => e.WorkTimeRatio)
                .HasPrecision(5, 2);
            builder.Property(e => e.MonthlyHours);
            builder.Property(e => e.AvgLateMinutes)
                .HasPrecision(5, 2);
            builder.Property(e => e.SickLeaveCounter);
            builder.Property(e => e.OffDays);
            
            // Indexes
            builder.HasIndex(e => e.BadgeNumber).IsUnique();
            builder.HasIndex(e => e.Name);
            builder.HasIndex(e => e.JobTitle);
            builder.HasIndex(e => e.WorkLocation);
            builder.HasIndex(e => e.IqamaNumber);
            builder.HasIndex(e => e.Nationality);
            builder.HasIndex(e => e.JoinDate);
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => e.DirectManagerId);
            
            // Relationships
            builder.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
                
            builder.HasOne(e => e.User)
                .WithOne()
                .HasForeignKey<Employee>(e => e.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
                
            builder.HasOne(e => e.DirectManager)
                .WithMany(e => e.ManagedEmployees)
                .HasForeignKey(e => e.DirectManagerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
                
            // Collections
            builder.HasMany(e => e.Certificates)
                .WithOne(c => c.Employee)
                .HasForeignKey(c => c.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(e => e.WorkExperiences)
                .WithOne(w => w.Employee)
                .HasForeignKey(w => w.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(e => e.Identifications)
                .WithOne(i => i.Employee)
                .HasForeignKey(i => i.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(e => e.EmergencyContacts)
                .WithOne(ec => ec.Employee)
                .HasForeignKey(ec => ec.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(e => e.Leaves)
                .WithOne(l => l.Employee)
                .HasForeignKey(l => l.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(e => e.BankAccounts)
                .WithOne(ba => ba.Employee)
                .HasForeignKey(ba => ba.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(e => e.Skills)
                .WithOne(es => es.Employee)
                .HasForeignKey(es => es.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Trainings)
                .WithOne(t => t.Employee)
                .HasForeignKey(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(e => e.Attendances)
                .WithOne(a => a.Employee)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add missing collection configurations
            builder.HasMany(e => e.EducationHistory)
                .WithOne(ed => ed.Employee)
                .HasForeignKey(ed => ed.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.PerformanceReviews)
                .WithOne(pr => pr.Employee)
                .HasForeignKey(pr => pr.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.ReviewsGiven)
                .WithOne(pr => pr.Reviewer)
                .HasForeignKey(pr => pr.ReviewerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.ManagedDepartments)
                .WithOne(d => d.Manager)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.SalaryHistory)
                .WithOne(s => s.Employee)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 