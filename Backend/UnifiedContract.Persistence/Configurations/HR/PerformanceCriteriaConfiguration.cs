using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class PerformanceCriteriaConfiguration : BaseEntityConfiguration<PerformanceCriteria>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PerformanceCriteria> builder)
        {
            // Table name
            builder.ToTable("PerformanceCriteria", "HR");
            
            // Properties
            builder.Property(pc => pc.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(pc => pc.Description)
                .HasMaxLength(500);
                
            builder.Property(pc => pc.Rating)
                .IsRequired();
                
            builder.Property(pc => pc.Comments)
                .HasMaxLength(1000);
                
            builder.Property(pc => pc.Weight)
                .IsRequired()
                .HasDefaultValue(0);
            
            // Indexes
            builder.HasIndex(pc => pc.PerformanceReviewId);
            builder.HasIndex(pc => pc.Name);
            builder.HasIndex(pc => pc.Rating);
            
            // Relationships
            builder.HasOne(pc => pc.PerformanceReview)
                .WithMany(pr => pr.PerformanceCriteria)
                .HasForeignKey(pc => pc.PerformanceReviewId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 