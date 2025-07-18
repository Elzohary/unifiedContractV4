using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.Auth;
using UnifiedContract.Domain.Entities.Common;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Persistence
{
    public class UnifiedContractDbContext : DbContext
    {
        public UnifiedContractDbContext(DbContextOptions<UnifiedContractDbContext> options)
            : base(options)
        {
        }

        // Auth
        public DbSet<User> Users { get; set; }

        // Common
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        // WorkOrder
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkOrderItem> WorkOrderItems { get; set; }
        public DbSet<WorkOrderRemark> WorkOrderRemarks { get; set; }
        public DbSet<WorkOrderIssue> WorkOrderIssues { get; set; }
        public DbSet<WorkOrderTask> WorkOrderTasks { get; set; }
        public DbSet<Permit> Permits { get; set; }
        public DbSet<WorkOrderAction> WorkOrderActions { get; set; }
        public DbSet<WorkOrderPhoto> WorkOrderPhotos { get; set; }
        public DbSet<WorkOrderForm> WorkOrderForms { get; set; }
        public DbSet<WorkOrderExpense> WorkOrderExpenses { get; set; }
        public DbSet<WorkOrderInvoice> WorkOrderInvoices { get; set; }

        // Resource
        public DbSet<ManpowerAssignment> ManpowerAssignments { get; set; }
        public DbSet<EquipmentAssignment> EquipmentAssignments { get; set; }
        public DbSet<MaterialAssignment> MaterialAssignments { get; set; }
        public DbSet<PurchasableMaterial> PurchasableMaterials { get; set; }
        public DbSet<ReceivableMaterial> ReceivableMaterials { get; set; }
        public DbSet<ClientMaterial> ClientMaterials { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Id = entry.Entity.Id == Guid.Empty ? Guid.NewGuid() : entry.Entity.Id;
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Apply configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UnifiedContractDbContext).Assembly);
        }
    }
} 