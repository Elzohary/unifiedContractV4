using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class WorkOrderIssueConfiguration : IEntityTypeConfiguration<WorkOrderIssue>
    {
        public void Configure(EntityTypeBuilder<WorkOrderIssue> builder)
        {
            builder.ToTable("WorkOrderIssues");

            builder.HasKey(issue => issue.Id);
            
            builder.Property(issue => issue.Title)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(issue => issue.Description)
                .HasMaxLength(1000);
                
            builder.Property(issue => issue.Status)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(issue => issue.Priority)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(issue => issue.IssueType)
                .HasMaxLength(50);
                
            // Relationships
            builder.HasOne<Domain.Entities.WorkOrder.WorkOrder>()
                .WithMany(wo => wo.WorkOrderIssues)
                .HasForeignKey(issue => issue.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // User assignment (assuming User is an entity in your system)
            builder.Property(issue => issue.AssignedToId);
                
            // Audit properties
            builder.Property(issue => issue.CreatedBy)
                .HasMaxLength(50);
                
            builder.Property(issue => issue.LastModifiedBy)
                .HasMaxLength(50);
        }
    }
} 