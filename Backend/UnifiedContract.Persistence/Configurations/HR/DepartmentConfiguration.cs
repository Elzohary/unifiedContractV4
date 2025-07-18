using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class DepartmentConfiguration : BaseEntityConfiguration<Department>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Department> builder)
        {
            // Table name
            builder.ToTable("Departments", "HR");
            
            // Properties
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(d => d.Code)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(d => d.Description)
                .HasMaxLength(500);
            
            builder.Property(d => d.Location)
                .HasMaxLength(100);
            
            builder.Property(d => d.IsActive)
                .HasDefaultValue(true);
            
            builder.Property(d => d.HeadCount)
                .HasDefaultValue(0);
            
            builder.Property(d => d.Budget)
                .HasPrecision(18, 2)
                .HasDefaultValue(0);
            
            // Indexes
            builder.HasIndex(d => d.Name);
            builder.HasIndex(d => d.Code).IsUnique();
            builder.HasIndex(d => d.ManagerId);
            builder.HasIndex(d => d.ParentDepartmentId);
            
            // Relationships
            builder.HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
                
            // Self-referencing relationship for hierarchical departments
            builder.HasOne(d => d.ParentDepartment)
                .WithMany(d => d.ChildDepartments)
                .HasForeignKey(d => d.ParentDepartmentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Add missing collection configuration
            builder.HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Seed initial departments
            SeedDepartments(builder);
        }
        
        private void SeedDepartments(EntityTypeBuilder<Department> builder)
        {
            // Root departments
            builder.HasData(
                new Department
                {
                    Id = new System.Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                    Name = "Executive Office",
                    Code = "EXEC",
                    Description = "Executive management department"
                },
                new Department
                {
                    Id = new System.Guid("248d7ebb-dce9-4d09-b174-12480e9433c9"),
                    Name = "Human Resources",
                    Code = "HR",
                    Description = "Human resources department"
                },
                new Department
                {
                    Id = new System.Guid("3d938468-ed6c-4c28-9787-adace9d9cc25"),
                    Name = "Engineering",
                    Code = "ENG",
                    Description = "Engineering department"
                },
                new Department
                {
                    Id = new System.Guid("1b69b03a-7847-4df2-9e51-c9a27edd2518"),
                    Name = "Finance",
                    Code = "FIN",
                    Description = "Finance department"
                },
                new Department
                {
                    Id = new System.Guid("a23c5bfe-ef35-4c79-8dd9-a2e7a5d7e0ca"),
                    Name = "Operations",
                    Code = "OPS",
                    Description = "Operations department"
                }
            );
            
            // Sub-departments
            builder.HasData(
                new Department
                {
                    Id = new System.Guid("8c4d65a2-24a5-4a60-90b5-5457c69d2ed6"),
                    Name = "Civil Engineering",
                    Code = "ENG-CIV",
                    Description = "Civil engineering team",
                    ParentDepartmentId = new System.Guid("3d938468-ed6c-4c28-9787-adace9d9cc25")
                },
                new Department
                {
                    Id = new System.Guid("b8ae6779-99e4-4a4e-8532-2ec9857461e4"),
                    Name = "Electrical Engineering",
                    Code = "ENG-ELEC",
                    Description = "Electrical engineering team",
                    ParentDepartmentId = new System.Guid("3d938468-ed6c-4c28-9787-adace9d9cc25")
                },
                new Department
                {
                    Id = new System.Guid("bfdee418-d34e-4828-aa30-41c3af7e8749"),
                    Name = "Mechanical Engineering",
                    Code = "ENG-MECH",
                    Description = "Mechanical engineering team",
                    ParentDepartmentId = new System.Guid("3d938468-ed6c-4c28-9787-adace9d9cc25")
                },
                new Department
                {
                    Id = new System.Guid("ffab3150-3dbd-4888-9351-ac0460153ccb"),
                    Name = "Recruitment",
                    Code = "HR-REC",
                    Description = "Recruitment team",
                    ParentDepartmentId = new System.Guid("248d7ebb-dce9-4d09-b174-12480e9433c9")
                },
                new Department
                {
                    Id = new System.Guid("43e4fcc7-a346-4620-b094-08cb4ca03816"),
                    Name = "Training",
                    Code = "HR-TRN",
                    Description = "Training team",
                    ParentDepartmentId = new System.Guid("248d7ebb-dce9-4d09-b174-12480e9433c9")
                }
            );
        }
    }
} 