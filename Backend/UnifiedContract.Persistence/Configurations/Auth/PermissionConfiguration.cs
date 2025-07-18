using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Auth;

namespace UnifiedContract.Persistence.Configurations.Auth
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            // Table name
            builder.ToTable("Permissions", "Auth");
            
            // Primary key
            builder.HasKey(p => p.Id);
            
            // Properties
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(p => p.NormalizedName)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(p => p.Description)
                .HasMaxLength(200);
                
            builder.Property(p => p.Module)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(p => p.Action)
                .IsRequired()
                .HasMaxLength(50);
            
            // Indexes
            builder.HasIndex(p => p.NormalizedName).IsUnique();
            builder.HasIndex(p => new { p.Module, p.Action }).IsUnique();
            
            // Seed basic permissions for each module
            SeedPermissions(builder);
        }
        
        private void SeedPermissions(EntityTypeBuilder<Permission> builder)
        {
            // Define modules
            string[] modules = { "WorkOrder", "User", "Role", "HR", "Equipment", "Material", "Client", "Report" };
            
            // Define actions
            string[] actions = { "View", "Create", "Edit", "Delete", "Export", "Approve", "Assign" };
            
            // Create permission list
            var permissions = new List<Permission>();
            int id = 1;
            
            foreach (var module in modules)
            {
                foreach (var action in actions)
                {
                    // Skip irrelevant combinations
                    if ((module == "Report" && action == "Approve") ||
                        (module == "Report" && action == "Assign"))
                    {
                        continue;
                    }
                    
                    var permissionName = $"{module}.{action}";
                    permissions.Add(new Permission
                    {
                        Id = new Guid($"00000000-0000-0000-0000-{id:D12}"),
                        Name = permissionName,
                        NormalizedName = permissionName.ToUpper(),
                        Description = $"Permission to {action.ToLower()} {module.ToLower()}s",
                        Module = module,
                        Action = action
                    });
                    id++;
                }
            }
            
            // Add special permissions
            permissions.Add(new Permission
            {
                Id = new Guid($"00000000-0000-0000-0000-{id:D12}"),
                Name = "Admin.FullAccess",
                NormalizedName = "ADMIN.FULLACCESS",
                Description = "Full access to all system features",
                Module = "Admin",
                Action = "FullAccess"
            });
            
            builder.HasData(permissions);
        }
    }
} 