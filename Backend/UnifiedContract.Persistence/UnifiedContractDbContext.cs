using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.Auth;
using UnifiedContract.Domain.Entities.Common;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Domain.Entities.WorkOrder;
#if DEBUG
using Microsoft.EntityFrameworkCore.Design;

namespace UnifiedContract.Persistence
{
    public class UnifiedContractDbContextFactory : IDesignTimeDbContextFactory<UnifiedContractDbContext>
    {
        public UnifiedContractDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UnifiedContractDbContext>();
            // Use your actual connection string here or read from a config file
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=UnifiedContractDb;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new UnifiedContractDbContext(optionsBuilder.Options);
        }
    }
}
#endif

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

        public DbSet<SiteReport> SiteReports { get; set; }
        public DbSet<SiteReportMaterialUsed> SiteReportMaterialsUsed { get; set; }
        public DbSet<SiteReportPhoto> SiteReportPhotos { get; set; }

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
            
            // Ignore DomainEvent and DomainEvents property
            modelBuilder.Ignore<UnifiedContract.Domain.Common.DomainEvent>();
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var domainEventsProp = entityType.ClrType.GetProperty("DomainEvents");
                if (domainEventsProp != null)
                {
                    modelBuilder.Entity(entityType.ClrType).Ignore("DomainEvents");
                }
            }
            // Apply configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UnifiedContractDbContext).Assembly);

            // Seed a test user
            var testUserId = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc");
            var password = "Test@1234";
            var staticUserCreated = new DateTime(2024, 7, 20, 12, 0, 0, DateTimeKind.Utc);
            
            // The dynamic password hashing was causing the model to change on each build.
            // It has been replaced with a static hash to ensure a stable model.
            var staticPasswordHash = "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4"; // Represents "Test@1234"

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = testUserId,
                UserName = "testuser",
                Email = "testuser@example.com",
                PasswordHash = staticPasswordHash, 
                FullName = "Test User",
                IsActive = true,
                IsEmployee = false,
                CreatedBy = "seed",
                CreatedAt = staticUserCreated,
                IsDeleted = false,
                LastModifiedBy = "seed"
            });
            // --- DEMO DATA SEEDING ---
            // Fixed GUIDs for lookup values
            var statusPendingId = new Guid("11111111-1111-1111-1111-111111111111");
            var statusCompletedId = new Guid("22222222-2222-2222-2222-222222222222");
            var priorityNormalId = new Guid("33333333-3333-3333-3333-333333333333");
            var priorityHighId = new Guid("44444444-4444-4444-4444-444444444444");

            // Use static DateTime values for seeding
            var staticNow = new DateTime(2024, 7, 20, 12, 0, 0, DateTimeKind.Utc);
            var staticStart1 = new DateTime(2024, 7, 16, 8, 0, 0, DateTimeKind.Utc);
            var staticDue1 = new DateTime(2024, 7, 25, 17, 0, 0, DateTimeKind.Utc);
            var staticTargetEnd1 = new DateTime(2024, 7, 27, 17, 0, 0, DateTimeKind.Utc);
            var staticStart2 = new DateTime(2024, 7, 11, 8, 0, 0, DateTimeKind.Utc);
            var staticDue2 = new DateTime(2024, 7, 18, 17, 0, 0, DateTimeKind.Utc);
            var staticTargetEnd2 = new DateTime(2024, 7, 19, 17, 0, 0, DateTimeKind.Utc);

            // Seed WorkOrderStatus
            modelBuilder.Entity<UnifiedContract.Domain.Entities.WorkOrder.Lookups.WorkOrderStatus>().HasData(
                new UnifiedContract.Domain.Entities.WorkOrder.Lookups.WorkOrderStatus
                {
                    Id = statusPendingId,
                    Name = "Pending",
                    Code = "PENDING",
                    Description = "Work order is pending.",
                    IsCompleted = false,
                    AllowsEditing = true,
                    RequiresApproval = false,
                    ColorCode = "#FFA500",
                    CreatedBy = "seed",
                    CreatedAt = staticNow,
                    LastModifiedBy = "seed",
                    LastModifiedAt = staticNow,
                    Metadata = ""
                },
                new UnifiedContract.Domain.Entities.WorkOrder.Lookups.WorkOrderStatus
                {
                    Id = statusCompletedId,
                    Name = "Completed",
                    Code = "COMPLETED",
                    Description = "Work order is completed.",
                    IsCompleted = true,
                    AllowsEditing = false,
                    RequiresApproval = false,
                    ColorCode = "#4CAF50",
                    CreatedBy = "seed",
                    CreatedAt = staticNow,
                    LastModifiedBy = "seed",
                    LastModifiedAt = staticNow,
                    Metadata = ""
                }
            );

            // Seed PriorityLevel
            modelBuilder.Entity<UnifiedContract.Domain.Entities.WorkOrder.Lookups.PriorityLevel>().HasData(
                new UnifiedContract.Domain.Entities.WorkOrder.Lookups.PriorityLevel
                {
                    Id = priorityNormalId,
                    Name = "Normal",
                    Code = "NORMAL",
                    Description = "Normal priority.",
                    SeverityValue = 2,
                    ColorCode = "#2196F3",
                    TargetResponseHours = 48,
                    RequiresImmediateNotification = false,
                    CreatedBy = "seed",
                    CreatedAt = staticNow,
                    LastModifiedBy = "seed",
                    LastModifiedAt = staticNow,
                    Metadata = ""
                },
                new UnifiedContract.Domain.Entities.WorkOrder.Lookups.PriorityLevel
                {
                    Id = priorityHighId,
                    Name = "High",
                    Code = "HIGH",
                    Description = "High priority.",
                    SeverityValue = 4,
                    ColorCode = "#F44336",
                    TargetResponseHours = 8,
                    RequiresImmediateNotification = true,
                    CreatedBy = "seed",
                    CreatedAt = staticNow,
                    LastModifiedBy = "seed",
                    LastModifiedAt = staticNow,
                    Metadata = ""
                }
            );

            // Seed demo WorkOrders
            modelBuilder.Entity<UnifiedContract.Domain.Entities.WorkOrder.WorkOrder>().HasData(
                new UnifiedContract.Domain.Entities.WorkOrder.WorkOrder
                {
                    Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    WorkOrderNumber = "WO-1001",
                    InternalOrderNumber = "INT-1001",
                    Title = "Demo Work Order 1",
                    Description = "Demo work order for testing.",
                    Location = "Riyadh",
                    Category = "Electrical",
                    Type = "Project",
                    Class = "A",
                    CompletionPercentage = 0,
                    ReceivedDate = new DateTime(2024, 7, 15, 12, 0, 0, DateTimeKind.Utc),
                    StartDate = staticStart1,
                    DueDate = staticDue1,
                    TargetEndDate = staticTargetEnd1,
                    EstimatedCost = 1000,
                    WorkOrderStatusId = statusPendingId,
                    PriorityLevelId = priorityNormalId,
                    ClientId = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    MaterialsExpense = 200,
                    LaborExpense = 300,
                    OtherExpense = 50,
                    CreatedBy = "seed",
                    CreatedAt = staticNow,
                    LastModifiedBy = "seed",
                    LastModifiedAt = staticNow
                },
                new UnifiedContract.Domain.Entities.WorkOrder.WorkOrder
                {
                    Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    WorkOrderNumber = "WO-1002",
                    InternalOrderNumber = "INT-1002",
                    Title = "Demo Work Order 2",
                    Description = "Second demo work order.",
                    Location = "Jeddah",
                    Category = "Mechanical",
                    Type = "Maintenance",
                    Class = "B",
                    CompletionPercentage = 100,
                    ReceivedDate = new DateTime(2024, 7, 10, 12, 0, 0, DateTimeKind.Utc),
                    StartDate = staticStart2,
                    DueDate = staticDue2,
                    TargetEndDate = staticTargetEnd2,
                    EstimatedCost = 2500,
                    WorkOrderStatusId = statusCompletedId,
                    PriorityLevelId = priorityHighId,
                    ClientId = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    MaterialsExpense = 500,
                    LaborExpense = 800,
                    OtherExpense = 100,
                    CreatedBy = "seed",
                    CreatedAt = staticNow,
                    LastModifiedBy = "seed",
                    LastModifiedAt = staticNow
                }
            );

            // Seed a demo client for the work orders
            modelBuilder.Entity<UnifiedContract.Domain.Entities.Client.Client>().HasData(
                new {
                    Id = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Name = "Demo Client",
                    CompanyName = "Demo Company",
                    ContactPerson = "John Doe",
                    Email = "demo.client@example.com",
                    Phone = "1234567890",
                    AlternatePhone = "0987654321",
                    Website = "www.demo.com",
                    Industry = "Construction",
                    VatNumber = "VAT123456",
                    LogoUrl = "",
                    IsActive = true,
                    Notes = "Seeded client for demo work orders",
                    AccountManagerId = (Guid?)null,
                    CreatedBy = "seed",
                    CreatedAt = staticNow,
                    LastModifiedBy = "seed",
                    LastModifiedAt = staticNow,
                    IsDeleted = false
                }
            );
            modelBuilder.Entity<UnifiedContract.Domain.Entities.Client.Client>().OwnsOne(c => c.Address).HasData(
                new {
                    ClientId = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Street = "Demo Street",
                    City = "Riyadh",
                    State = "Riyadh",
                    PostalCode = "12345",
                    Country = "Saudi Arabia",
                    FormattedAddress = "Demo Street, Riyadh, Riyadh 12345, Saudi Arabia"
                }
            );
            // Seed demo WorkOrderItems for the demo work orders
            modelBuilder.Entity<UnifiedContract.Domain.Entities.WorkOrder.WorkOrderItem>().HasData(
                new UnifiedContract.Domain.Entities.WorkOrder.WorkOrderItem
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000001"),
                    ItemNumber = "WOI-1001-001",
                    Description = "Concrete Mix - Grade 30 for foundation work",
                    Unit = "m³",
                    UnitPrice = 100m,
                    EstimatedQuantity = 10m,
                    EstimatedPrice = 1000m,
                    EstimatedPriceWithVAT = 1150m,
                    ActualQuantity = 10m,
                    ActualPrice = 1000m,
                    ActualPriceWithVAT = 1150m,
                    ReasonForFinalQuantity = "Used as planned",
                    WorkOrderId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    CreatedBy = "seed",
                    CreatedAt = staticNow,
                    LastModifiedBy = "seed",
                    LastModifiedAt = staticNow,
                    PaymentType = "Fixed Price",
                    ManagementArea = "Default Area",
                    Currency = "SAR"
                },
                new UnifiedContract.Domain.Entities.WorkOrder.WorkOrderItem
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000002"),
                    ItemNumber = "WOI-1001-002",
                    Description = "Steel Bars - 12mm for structural support",
                    Unit = "ton",
                    UnitPrice = 120m,
                    EstimatedQuantity = 5m,
                    EstimatedPrice = 600m,
                    EstimatedPriceWithVAT = 690m,
                    ActualQuantity = 5m,
                    ActualPrice = 600m,
                    ActualPriceWithVAT = 690m,
                    ReasonForFinalQuantity = "Used as planned",
                    WorkOrderId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    CreatedBy = "seed",
                    CreatedAt = staticNow,
                    LastModifiedBy = "seed",
                    LastModifiedAt = staticNow,
                    PaymentType = "Fixed Price",
                    ManagementArea = "Default Area",
                    Currency = "SAR"
                },
                new UnifiedContract.Domain.Entities.WorkOrder.WorkOrderItem
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000003"),
                    ItemNumber = "WOI-1002-001",
                    Description = "Electrical Wiring - 2.5mm² for power distribution",
                    Unit = "m",
                    UnitPrice = 1250m,
                    EstimatedQuantity = 2m,
                    EstimatedPrice = 2500m,
                    EstimatedPriceWithVAT = 2875m,
                    ActualQuantity = 2m,
                    ActualPrice = 2500m,
                    ActualPriceWithVAT = 2875m,
                    ReasonForFinalQuantity = "Used as planned",
                    WorkOrderId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    CreatedBy = "seed",
                    CreatedAt = staticNow,
                    LastModifiedBy = "seed",
                    LastModifiedAt = staticNow,
                    PaymentType = "Fixed Price",
                    ManagementArea = "Default Area",
                    Currency = "SAR"
                }
            );
            // --- END DEMO DATA SEEDING ---
        }
    }
} 