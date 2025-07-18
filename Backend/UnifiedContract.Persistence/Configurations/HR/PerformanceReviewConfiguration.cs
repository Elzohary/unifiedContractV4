using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Persistence.Configurations.Common;

namespace UnifiedContract.Persistence.Configurations.HR
{
    public class PerformanceReviewConfiguration : BaseEntityConfiguration<PerformanceReview>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PerformanceReview> builder)
        {
            // Table name
            builder.ToTable("PerformanceReviews", "HR");
            
            // Properties
            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(p => p.ManagerComments)
                .HasMaxLength(1000);
                
            builder.Property(p => p.EmployeeComments)
                .HasMaxLength(1000);
                
            builder.Property(p => p.GoalsForNextPeriod)
                .HasMaxLength(1000);
                
            builder.Property(p => p.TrainingRecommendations)
                .HasMaxLength(1000);
                
            builder.Property(p => p.OverallRating)
                .IsRequired();
                
            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasDefaultValue(ReviewStatus.Draft);
            
            // Indexes
            builder.HasIndex(p => p.EmployeeId);
            builder.HasIndex(p => p.ReviewerId);
            builder.HasIndex(p => p.ReviewPeriodStart);
            builder.HasIndex(p => p.ReviewPeriodEnd);
            builder.HasIndex(p => p.DueDate);
            builder.HasIndex(p => p.CompletionDate);
            builder.HasIndex(p => p.OverallRating);
            builder.HasIndex(p => p.Status);
            
            // Relationships
            builder.HasOne(p => p.Employee)
                .WithMany(e => e.PerformanceReviews)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasOne(p => p.Reviewer)
                .WithMany(e => e.ReviewsGiven)
                .HasForeignKey(p => p.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 