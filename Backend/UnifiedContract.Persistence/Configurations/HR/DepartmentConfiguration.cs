using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            builder.HasKey(x => x.Id);
            
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.Code)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(d => d.Description)
                .HasMaxLength(500);
                
            builder.Property(d => d.Location)
                .HasMaxLength(200);

            builder.Property(b => b.Budget)
                .HasPrecision(18, 2);

            builder.HasOne(d => d.ParentDepartment)
                .WithMany(d => d.ChildDepartments)
                .HasForeignKey(d => d.ParentDepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 