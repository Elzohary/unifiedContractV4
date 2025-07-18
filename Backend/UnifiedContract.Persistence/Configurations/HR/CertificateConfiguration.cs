using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class CertificateConfiguration : BaseEntityConfiguration<Certificate>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Certificate> builder)
        {
            // Table name
            builder.ToTable("Certificates", "HR");
            
            // Properties
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(c => c.Issuer)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(c => c.DocumentUrl)
                .HasMaxLength(500);
                
            builder.Property(c => c.Verified)
                .IsRequired()
                .HasDefaultValue(false);
            
            // Indexes
            builder.HasIndex(c => c.EmployeeId);
            builder.HasIndex(c => c.Issuer);
            builder.HasIndex(c => c.IssueDate);
            builder.HasIndex(c => c.ExpiryDate);
            builder.HasIndex(c => c.Verified);
            
            // Relationships
            builder.HasOne(c => c.Employee)
                .WithMany(e => e.Certificates)
                .HasForeignKey(c => c.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 