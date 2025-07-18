using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class ActionNeededConfiguration : IEntityTypeConfiguration<ActionNeeded>
    {
        public void Configure(EntityTypeBuilder<ActionNeeded> builder)
        {
            builder.ToTable("ActionsNeeded");

            builder.HasKey(action => action.Id);
            
            builder.Property(action => action.Title)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(action => action.Description)
                .HasMaxLength(1000);
                
            builder.Property(action => action.Status)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(action => action.Priority)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(action => action.Category)
                .HasMaxLength(50);
                
            // Work Order relationship if applicable
            builder.Property(action => action.WorkOrderId);
                
            // User assignment (assuming User is an entity in your system)
            builder.Property(action => action.AssignedToId);
                
            // Audit properties
            builder.Property(action => action.CreatedBy)
                .HasMaxLength(50);
                
            builder.Property(action => action.LastModifiedBy)
                .HasMaxLength(50);
        }
    }
} 