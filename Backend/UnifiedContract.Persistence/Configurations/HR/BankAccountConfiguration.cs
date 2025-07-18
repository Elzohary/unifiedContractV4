using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class BankAccountConfiguration : BaseEntityConfiguration<BankAccount>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<BankAccount> builder)
        {
            // Table name
            builder.ToTable("BankAccounts", "HR");
            
            // Properties
            builder.Property(b => b.BankName)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(b => b.AccountNumber)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(b => b.IBAN)
                .HasMaxLength(50);
                
            builder.Property(b => b.SwiftCode)
                .HasMaxLength(20);
                
            builder.Property(b => b.BranchName)
                .HasMaxLength(100);
                
            builder.Property(b => b.BranchCode)
                .HasMaxLength(20);
                
            builder.Property(b => b.IsPrimary)
                .IsRequired()
                .HasDefaultValue(false);
            
            // Indexes
            builder.HasIndex(b => b.EmployeeId);
            builder.HasIndex(b => b.BankName);
            builder.HasIndex(b => b.AccountNumber);
            builder.HasIndex(b => b.IBAN);
            builder.HasIndex(b => b.IsPrimary);
            builder.HasIndex(b => new { b.EmployeeId, b.IsPrimary });
            
            // Relationships
            builder.HasOne(b => b.Employee)
                .WithMany(e => e.BankAccounts)
                .HasForeignKey(b => b.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 