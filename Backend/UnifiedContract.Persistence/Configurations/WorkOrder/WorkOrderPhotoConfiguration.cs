using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class WorkOrderPhotoConfiguration : IEntityTypeConfiguration<WorkOrderPhoto>
    {
        public void Configure(EntityTypeBuilder<WorkOrderPhoto> builder)
        {
            builder.ToTable("WorkOrderPhotos");

            builder.HasKey(photo => photo.Id);
            
            builder.Property(photo => photo.FileName)
                .IsRequired()
                .HasMaxLength(255);
                
            builder.Property(photo => photo.FileUrl)
                .IsRequired()
                .HasMaxLength(500);
                
            builder.Property(photo => photo.Caption)
                .HasMaxLength(200);
                
            builder.Property(photo => photo.PhotoType)
                .HasMaxLength(50);
                
            // Relationships
            builder.HasOne<Domain.Entities.WorkOrder.WorkOrder>()
                .WithMany(wo => wo.WorkOrderPhotos)
                .HasForeignKey(photo => photo.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Audit properties
            builder.Property(photo => photo.CreatedBy)
                .HasMaxLength(50);
                
            builder.Property(photo => photo.LastModifiedBy)
                .HasMaxLength(50);
        }
    }
} 