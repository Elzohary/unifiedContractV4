using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Auth;

namespace UnifiedContract.Persistence.Configurations.Auth
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            // Table name is already defined in RoleConfiguration
            
            // Configure if needed beyond what's in RoleConfiguration
            
            // Seed default permissions for administrator role
            SeedAdministratorPermissions(builder);
            
            // Seed default permissions for other roles
            SeedEngineerPermissions(builder);
            SeedForemanPermissions(builder);
            SeedWorkerPermissions(builder);
            SeedClientPermissions(builder);
            SeedCoordinatorPermissions(builder);
        }
        
        private void SeedAdministratorPermissions(EntityTypeBuilder<RolePermission> builder)
        {
            // Admin gets the full access permission
            builder.HasData(new RolePermission
            {
                Id = new Guid("11111111-1111-1111-1111-000000000053"), // RoleId + PermissionId based
                RoleId = new Guid("1f43eb74-9db6-4128-a3e5-69bd3aff3d67"), // Administrator role
                PermissionId = new Guid("00000000-0000-0000-0000-000000000053"), // Admin.FullAccess permission
                CreatedBy = "system",
                LastModifiedBy = "system",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                LastModifiedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
        }
        
        private void SeedEngineerPermissions(EntityTypeBuilder<RolePermission> builder)
        {
            var roleId = new Guid("2a5db2e7-9170-4a5d-9065-c2b1f20d2c12"); // Engineer role
            var permissionAssignments = new List<RolePermission>();
            
            // Work Order permissions
            permissionAssignments.Add(CreateRolePermission(roleId, "WorkOrder", "View"));
            permissionAssignments.Add(CreateRolePermission(roleId, "WorkOrder", "Edit"));
            permissionAssignments.Add(CreateRolePermission(roleId, "WorkOrder", "Approve"));
            permissionAssignments.Add(CreateRolePermission(roleId, "WorkOrder", "Export"));
            
            // HR permissions (limited)
            permissionAssignments.Add(CreateRolePermission(roleId, "HR", "View"));
            
            // Equipment permissions
            permissionAssignments.Add(CreateRolePermission(roleId, "Equipment", "View"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Equipment", "Assign"));
            
            // Material permissions
            permissionAssignments.Add(CreateRolePermission(roleId, "Material", "View"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Material", "Create"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Material", "Edit"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Material", "Assign"));
            
            // Client permissions
            permissionAssignments.Add(CreateRolePermission(roleId, "Client", "View"));
            
            // Report permissions
            permissionAssignments.Add(CreateRolePermission(roleId, "Report", "View"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Report", "Export"));
            
            builder.HasData(permissionAssignments);
        }
        
        private void SeedForemanPermissions(EntityTypeBuilder<RolePermission> builder)
        {
            var roleId = new Guid("3c3d5e29-43b9-4f0e-8d5a-6d5e7289736c"); // Foreman role
            var permissionAssignments = new List<RolePermission>();
            
            // Work Order permissions
            permissionAssignments.Add(CreateRolePermission(roleId, "WorkOrder", "View"));
            permissionAssignments.Add(CreateRolePermission(roleId, "WorkOrder", "Edit"));
            
            // Equipment permissions
            permissionAssignments.Add(CreateRolePermission(roleId, "Equipment", "View"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Equipment", "Assign"));
            
            // Material permissions
            permissionAssignments.Add(CreateRolePermission(roleId, "Material", "View"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Material", "Create"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Material", "Assign"));
            
            builder.HasData(permissionAssignments);
        }
        
        private void SeedWorkerPermissions(EntityTypeBuilder<RolePermission> builder)
        {
            var roleId = new Guid("4f511c53-efa7-4f35-a86c-ba3079493f3c"); // Worker role
            var permissionAssignments = new List<RolePermission>();
            
            // Work Order permissions (limited)
            permissionAssignments.Add(CreateRolePermission(roleId, "WorkOrder", "View"));
            
            // Material permissions (limited)
            permissionAssignments.Add(CreateRolePermission(roleId, "Material", "View"));
            
            builder.HasData(permissionAssignments);
        }
        
        private void SeedClientPermissions(EntityTypeBuilder<RolePermission> builder)
        {
            var roleId = new Guid("5c77913c-eb6d-4a9f-9b3f-4b17c6d38a2d"); // Client role
            var permissionAssignments = new List<RolePermission>();
            
            // Work Order permissions (very limited)
            permissionAssignments.Add(CreateRolePermission(roleId, "WorkOrder", "View"));
            
            // Report permissions (limited)
            permissionAssignments.Add(CreateRolePermission(roleId, "Report", "View"));
            
            builder.HasData(permissionAssignments);
        }
        
        private void SeedCoordinatorPermissions(EntityTypeBuilder<RolePermission> builder)
        {
            var roleId = new Guid("6d4c2c7c-cf19-4a33-8c5d-e9b68a598252"); // Coordinator role
            var permissionAssignments = new List<RolePermission>();
            
            // Work Order permissions
            permissionAssignments.Add(CreateRolePermission(roleId, "WorkOrder", "View"));
            permissionAssignments.Add(CreateRolePermission(roleId, "WorkOrder", "Create"));
            permissionAssignments.Add(CreateRolePermission(roleId, "WorkOrder", "Edit"));
            permissionAssignments.Add(CreateRolePermission(roleId, "WorkOrder", "Assign"));
            permissionAssignments.Add(CreateRolePermission(roleId, "WorkOrder", "Export"));
            
            // HR permissions (limited)
            permissionAssignments.Add(CreateRolePermission(roleId, "HR", "View"));
            permissionAssignments.Add(CreateRolePermission(roleId, "HR", "Assign"));
            
            // Equipment permissions
            permissionAssignments.Add(CreateRolePermission(roleId, "Equipment", "View"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Equipment", "Create"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Equipment", "Edit"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Equipment", "Assign"));
            
            // Material permissions
            permissionAssignments.Add(CreateRolePermission(roleId, "Material", "View"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Material", "Create"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Material", "Edit"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Material", "Assign"));
            
            // Client permissions
            permissionAssignments.Add(CreateRolePermission(roleId, "Client", "View"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Client", "Create"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Client", "Edit"));
            
            // Report permissions
            permissionAssignments.Add(CreateRolePermission(roleId, "Report", "View"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Report", "Create"));
            permissionAssignments.Add(CreateRolePermission(roleId, "Report", "Export"));
            
            builder.HasData(permissionAssignments);
        }
        
        private RolePermission CreateRolePermission(Guid roleId, string module, string action)
        {
            // Calculate permission ID based on module and action
            // This method assumes the permissions were created in the same order as in PermissionConfiguration

            string[] modules = { "WorkOrder", "User", "Role", "HR", "Equipment", "Material", "Client", "Report" };
            string[] actions = { "View", "Create", "Edit", "Delete", "Export", "Approve", "Assign" };

            int moduleIndex = Array.IndexOf(modules, module);
            int actionIndex = Array.IndexOf(actions, action);

            if (moduleIndex == -1 || actionIndex == -1)
            {
                throw new ArgumentException($"Invalid module ({module}) or action ({action})");
            }

            int permissionId = moduleIndex * actions.Length + actionIndex + 1;
            // Use a deterministic Guid for Id based on RoleId and PermissionId
            var idBytes = roleId.ToByteArray();
            var permBytes = new Guid($"00000000-0000-0000-0000-{permissionId:D12}").ToByteArray();
            for (int i = 0; i < 16; i++) idBytes[i] ^= permBytes[i];
            var deterministicId = new Guid(idBytes);

            return new RolePermission
            {
                Id = deterministicId,
                RoleId = roleId,
                PermissionId = new Guid($"00000000-0000-0000-0000-{permissionId:D12}"),
                CreatedBy = "system",
                LastModifiedBy = "system",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                LastModifiedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };
        }
    }
} 