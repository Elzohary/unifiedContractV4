using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.Resource
{
    public class EquipmentAssignmentConfiguration : BaseEntityConfiguration<EquipmentAssignment>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<EquipmentAssignment> builder)
        {
            // Table name
            builder.ToTable("EquipmentAssignment", "Resource");
            
            // Properties
            builder.Property(a => a.StartDate)
                .IsRequired();
            builder.Property(a => a.EndDate);
            builder.Property(a => a.Notes)
                .HasMaxLength(1000);
            
            // Indexes
            builder.HasIndex(a => a.EquipmentId);
            builder.HasIndex(a => a.StartDate);
            builder.HasIndex(a => a.EndDate);
            
            // Relationships
            builder.HasOne(a => a.Equipment)
                .WithMany(e => e.Assignments)
                .HasForeignKey(a => a.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(a => a.WorkOrder)
                .WithMany()
                .HasForeignKey(a => a.WorkOrderId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(a => a.Operator)
                .WithMany()
                .HasForeignKey(a => a.OperatorId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 