using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence.Configurations.WorkOrder
{
    public class WorkOrderExpenseConfiguration : IEntityTypeConfiguration<WorkOrderExpense>
    {
        public void Configure(EntityTypeBuilder<WorkOrderExpense> builder)
        {
            builder.ToTable("WorkOrderExpenses");

            builder.HasKey(expense => expense.Id);
            
            builder.Property(expense => expense.ExpenseType)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(expense => expense.Amount)
                .HasPrecision(18, 2);
                
            builder.Property(expense => expense.Description)
                .HasMaxLength(1000);
                
            builder.Property(expense => expense.ReceiptUrl)
                .HasMaxLength(500);
                
            builder.Property(expense => expense.Status)
                .IsRequired()
                .HasMaxLength(50);
                
            // Relationships
            builder.HasOne<Domain.Entities.WorkOrder.WorkOrder>()
                .WithMany(wo => wo.WorkOrderExpenses)
                .HasForeignKey(expense => expense.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Audit properties
            builder.Property(expense => expense.CreatedBy)
                .HasMaxLength(50);
                
            builder.Property(expense => expense.LastModifiedBy)
                .HasMaxLength(50);
        }
    }
} 