using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Auth;

namespace UnifiedContract.Persistence.Configurations.Auth
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // Table name
            builder.ToTable("Roles", "Auth");
            
            // Primary key
            builder.HasKey(r => r.Id);
            
            // Properties
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(r => r.NormalizedName)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(r => r.Description)
                .HasMaxLength(200);
            
            // Indexes
            builder.HasIndex(r => r.NormalizedName).IsUnique();
            
            // Relationships
            // Many-to-many relationship with permissions through RolePermission
            builder.HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<RolePermission>(
                    j => j
                        .HasOne(rp => rp.Permission)
                        .WithMany(p => p.RolePermissions)
                        .HasForeignKey(rp => rp.PermissionId)
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne(rp => rp.Role)
                        .WithMany(r => r.RolePermissions)
                        .HasForeignKey(rp => rp.RoleId)
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey(rp => new { rp.RoleId, rp.PermissionId });
                        j.ToTable("RolePermissions", "Auth");
                        j.HasIndex(rp => new { rp.RoleId, rp.PermissionId });
                    });
            
            // Seed Data for default roles
            var administratorRole = new Role
            {
                Id = new System.Guid("1f43eb74-9db6-4128-a3e5-69bd3aff3d67"),
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                Description = "Full system administrator with all permissions"
            };
            
            var engineerRole = new Role
            {
                Id = new System.Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12"),
                Name = "Engineer",
                NormalizedName = "ENGINEER",
                Description = "Site engineer responsible for work orders"
            };
            
            var foremanRole = new Role
            {
                Id = new System.Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289736c"),
                Name = "Foreman",
                NormalizedName = "FOREMAN",
                Description = "Foreman supervising site work"
            };
            
            var workerRole = new Role
            {
                Id = new System.Guid("4f511c53-efa7-4f35-a86c-ba3079493f3c"),
                Name = "Worker",
                NormalizedName = "WORKER",
                Description = "Site worker carrying out tasks"
            };
            
            var clientRole = new Role
            {
                Id = new System.Guid("5c77913c-eb6d-4a9f-9b3f-4b17c6d38a2d"),
                Name = "Client",
                NormalizedName = "CLIENT",
                Description = "Client with limited access to their work orders"
            };
            
            var coordinatorRole = new Role
            {
                Id = new System.Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252"),
                Name = "Coordinator",
                NormalizedName = "COORDINATOR",
                Description = "Project coordinator managing work orders and resources"
            };
            
            builder.HasData(
                administratorRole,
                engineerRole,
                foremanRole,
                workerRole,
                clientRole,
                coordinatorRole
            );
        }
    }
} 