using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Auth;

namespace UnifiedContract.Persistence.Configurations.Auth
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            // Table name
            builder.ToTable("Roles", "Auth");
            
            // Primary key
            builder.HasKey(x => x.Id);
            
            // Properties
            // Only configure properties that exist in the Role entity
            // Remove or update all references to non-existent properties
            // Ensure navigation property names match the entity class
            
            // Seed data for all roles
            builder.HasData(
                new Role
                {
                    Id = new Guid("1f43eb74-9db6-4128-a3e5-69bd3aff3d67"),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "Administrator role with full access",
                    CreatedBy = "system",
                    LastModifiedBy = "system",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    LastModifiedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Role
                {
                    Id = new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12"),
                    Name = "Engineer",
                    NormalizedName = "ENGINEER",
                    Description = "Engineer role with technical permissions",
                    CreatedBy = "system",
                    LastModifiedBy = "system",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    LastModifiedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Role
                {
                    Id = new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289736c"),
                    Name = "Foreman",
                    NormalizedName = "FOREMAN",
                    Description = "Foreman role with supervision permissions",
                    CreatedBy = "system",
                    LastModifiedBy = "system",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    LastModifiedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Role
                {
                    Id = new Guid("4f511c53-efa7-4f35-a86c-ba3079493f3c"),
                    Name = "Worker",
                    NormalizedName = "WORKER",
                    Description = "Worker role with basic permissions",
                    CreatedBy = "system",
                    LastModifiedBy = "system",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    LastModifiedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Role
                {
                    Id = new Guid("5c77913c-eb6d-4a9f-9b3f-4b17c6d38a2d"),
                    Name = "Client",
                    NormalizedName = "CLIENT",
                    Description = "Client role with limited permissions",
                    CreatedBy = "system",
                    LastModifiedBy = "system",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    LastModifiedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Role
                {
                    Id = new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252"),
                    Name = "Coordinator",
                    NormalizedName = "COORDINATOR",
                    Description = "Coordinator role with management permissions",
                    CreatedBy = "system",
                    LastModifiedBy = "system",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    LastModifiedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
} 