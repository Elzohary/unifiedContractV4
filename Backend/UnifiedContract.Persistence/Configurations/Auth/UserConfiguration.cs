using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Auth;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Persistence.Configurations.Auth
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table name
            builder.ToTable("Users", "Auth");
            
            // Primary key
            builder.HasKey(u => u.Id);
            
            // Properties
            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(u => u.Avatar)
                .IsRequired(false)
                .HasMaxLength(500);
                
            builder.Property(u => u.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
                
            builder.Property(u => u.IsEmployee)
                .IsRequired()
                .HasDefaultValue(false);
                
            builder.Property(u => u.EmployeeId);
            
            // Indexes
            builder.HasIndex(u => u.UserName).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.EmployeeId);
            builder.HasIndex(u => u.IsEmployee);
            
            // Remove relationship to Employee.UserId (property no longer exists)
            // builder.HasOne<Employee>()
            //     .WithOne()
            //     .HasForeignKey<Employee>(e => e.UserId)
            //     .IsRequired(false)
            //     .OnDelete(DeleteBehavior.SetNull);
            
            // Many-to-many relationship with roles through UserRole
            builder.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<UserRole>(
                    j => j
                        .HasOne(ur => ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId)
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne(ur => ur.User)
                        .WithMany(u => u.UserRoles)
                        .HasForeignKey(ur => ur.UserId)
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey(ur => new { ur.UserId, ur.RoleId });
                        j.ToTable("UserRoles", "Auth");
                        j.HasIndex(ur => new { ur.UserId, ur.RoleId });
                    });
                    
            // Seed data for admin user
            var adminUserId = new Guid("7eb08e14-4e4c-4801-93e5-5d821bba7fd2");
            var staticAdminCreated = new DateTime(2024, 7, 20, 12, 0, 0, DateTimeKind.Utc);
            builder.HasData(new User
            {
                Id = adminUserId,
                UserName = "admin",
                Email = "admin@unifiedcontract.com",
                PasswordHash = "AQAAAAIAAYagAAAAELbXp1J2NwQxX8K8QxX8K8QxX8K8QxX8K8QxX8K8QxX8K8QxX8QxX8K8Q==", // This should be properly hashed
                FullName = "System Administrator",
                Avatar = null,
                IsActive = true,
                IsEmployee = false,
                CreatedAt = staticAdminCreated,
                LastModifiedAt = staticAdminCreated,
                CreatedBy = "system",
                LastModifiedBy = "system"
            });
        }
    }
} 