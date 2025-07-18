using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class PermitConfiguration : IEntityTypeConfiguration<Permit>
    {
        public void Configure(EntityTypeBuilder<Permit> builder)
        {
            builder.ToTable("Permits");

            builder.HasKey(permit => permit.Id);
            
            builder.Property(permit => permit.PermitNumber)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(permit => permit.PermitType)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(permit => permit.Status)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(permit => permit.Description)
                .HasMaxLength(1000);
                
            builder.Property(permit => permit.DocumentUrl)
                .HasMaxLength(500);
                
            // Work Order relationship if applicable
            builder.Property(permit => permit.WorkOrderId);
                
            // Issuing authority
            builder.Property(permit => permit.IssuingAuthority)
                .HasMaxLength(200);
                
            // Audit properties
            builder.Property(permit => permit.CreatedBy)
                .HasMaxLength(50);
                
            builder.Property(permit => permit.LastModifiedBy)
                .HasMaxLength(50);
        }
    }
} 