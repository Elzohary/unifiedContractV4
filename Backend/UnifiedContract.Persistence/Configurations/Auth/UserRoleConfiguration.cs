using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Auth;

namespace UnifiedContract.Persistence.Configurations.Auth
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            // Table name is already defined in UserConfiguration
            
            // Configure if needed beyond what's in UserConfiguration
            
            // Seed a default admin user with admin role
            var adminUserId = new Guid("7eb08e14-4e4c-4801-93e5-5d821bba7fd2");
            var adminRoleId = new Guid("1f43eb74-9db6-4128-a3e5-69bd3aff3d67");
            
            builder.HasData(new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = adminUserId,
                RoleId = adminRoleId
            });
        }
    }
} 