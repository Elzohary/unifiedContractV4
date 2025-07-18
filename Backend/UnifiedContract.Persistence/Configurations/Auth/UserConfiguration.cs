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
            
            // Relationships
            builder.HasOne<Employee>()
                .WithOne()
                .HasForeignKey<Employee>(e => e.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            
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
        }
    }
} 