using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Items");

            builder.HasKey(item => item.Id);
            
            builder.Property(item => item.ItemNumber)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(item => item.Description)
                .IsRequired()
                .HasMaxLength(1000);
                
            builder.Property(item => item.Unit)
                .HasMaxLength(50)
                .HasDefaultValue("Piece");
                
            builder.Property(item => item.UnitPrice)
                .HasPrecision(18, 2);
                
            builder.Property(item => item.PaymentType)
                .HasMaxLength(100);
                
            builder.Property(item => item.ManagementArea)
                .HasMaxLength(200);
                
            builder.Property(item => item.Currency)
                .HasMaxLength(10);
                
            builder.Property(item => item.IsActive)
                .HasDefaultValue(true);
                
            // Relationships
            builder.HasOne(item => item.Client)
                .WithMany()
                .HasForeignKey(item => item.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasMany(item => item.WorkOrderAssignments)
                .WithOne(assignment => assignment.Item)
                .HasForeignKey(assignment => assignment.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Audit properties
            builder.Property(item => item.CreatedBy)
                .HasMaxLength(50);
                
            builder.Property(item => item.LastModifiedBy)
                .HasMaxLength(50);
        }
    }
} 