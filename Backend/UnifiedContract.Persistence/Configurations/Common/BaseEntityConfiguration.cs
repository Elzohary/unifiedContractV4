using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Persistence.Configurations.Common
{
    /// <summary>
    /// Base configuration for all entities that inherit from BaseEntity
    /// </summary>
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            // Primary key
            builder.HasKey(e => e.Id);
            
            // Id property
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWSEQUENTIALID()");
            
            // Audit fields
            builder.Property(e => e.CreatedBy)
                .HasMaxLength(100);
                
            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");
                
            builder.Property(e => e.LastModifiedBy)
                .HasMaxLength(100);
                
            builder.Property(e => e.LastModifiedAt)
                .HasDefaultValueSql("GETUTCDATE()");
                
            // Implement additional entity configuration in derived classes
            ConfigureEntity(builder);
        }
        
        // To be implemented by derived classes
        protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
    }
} 