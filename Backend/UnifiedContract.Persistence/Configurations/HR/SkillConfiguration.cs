using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class SkillConfiguration : BaseEntityConfiguration<Skill>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Skill> builder)
        {
            // Table name
            builder.ToTable("Skills", "HR");
            
            // Properties
            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(s => s.Category)
                .HasMaxLength(50);
                
            builder.Property(s => s.Description)
                .HasMaxLength(500);
            
            // Indexes
            builder.HasIndex(s => s.Name).IsUnique();
            builder.HasIndex(s => s.Category);
        }
    }
    
    public class EmployeeSkillConfiguration : BaseEntityConfiguration<EmployeeSkill>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<EmployeeSkill> builder)
        {
            // Table name
            builder.ToTable("EmployeeSkills", "HR");
            
            // Properties
            builder.Property(es => es.ProficiencyLevel)
                .IsRequired();
                
            builder.Property(es => es.AcquiredDate)
                .IsRequired();
                
            builder.Property(es => es.Certificate)
                .HasMaxLength(255);
                
            builder.Property(es => es.Notes)
                .HasMaxLength(500);
                
            builder.Property(es => es.IsFeatured)
                .IsRequired()
                .HasDefaultValue(false);
            
            // Indexes
            builder.HasIndex(es => es.EmployeeId);
            builder.HasIndex(es => es.SkillId);
            builder.HasIndex(es => es.ProficiencyLevel);
            builder.HasIndex(es => es.LastUsedDate);
            builder.HasIndex(es => es.IsFeatured);
            builder.HasIndex(es => new { es.EmployeeId, es.SkillId }).IsUnique();
            
            // Relationships
            builder.HasOne(es => es.Employee)
                .WithMany(e => e.Skills)
                .HasForeignKey(es => es.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasOne(es => es.Skill)
                .WithMany(s => s.EmployeeSkills)
                .HasForeignKey(es => es.SkillId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 