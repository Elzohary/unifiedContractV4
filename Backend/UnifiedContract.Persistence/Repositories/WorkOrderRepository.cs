using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnifiedContract.Domain.Entities.WorkOrder;
using UnifiedContract.Domain.Interfaces.Repositories;

namespace UnifiedContract.Persistence.Repositories
{
    public class WorkOrderRepository : Repository<WorkOrder>, IWorkOrderRepository
    {
        public WorkOrderRepository(UnifiedContractDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<WorkOrder> GetWorkOrderWithDetailsAsync(Guid id)
        {
            return await _dbContext.WorkOrders
                .Include(w => w.Status)
                .Include(w => w.Priority)
                .Include(w => w.Client)
                .Include(w => w.Items)
                .Include(w => w.Remarks)
                .Include(w => w.Issues)
                .Include(w => w.Tasks)
                .Include(w => w.Permits)
                .Include(w => w.Actions)
                .Include(w => w.Photos)
                .Include(w => w.Forms)
                .Include(w => w.Expenses)
                .Include(w => w.Invoices)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<WorkOrder>> GetWorkOrdersByClientAsync(string client)
        {
            return await _dbContext.WorkOrders
                .Where(w => w.Client.Name == client)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetWorkOrdersByEngineerAsync(Guid engineerId)
        {
            return await _dbContext.WorkOrders
                .Where(w => w.EngineerInChargeId == engineerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetWorkOrdersWithStatusAsync(string status)
        {
            return await _dbContext.WorkOrders
                .Where(w => w.Status.ToString() == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetWorkOrdersForPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbContext.WorkOrders
                .Where(w => w.StartDate >= startDate && w.DueDate <= endDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalEstimatedCostAsync(Guid id)
        {
            var workOrder = await _dbContext.WorkOrders
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (workOrder == null)
                return 0;

            return workOrder.EstimatedCost ?? 0m;
        }

        public async Task<decimal> GetTotalActualCostAsync(Guid id)
        {
            var workOrder = await _dbContext.WorkOrders
                .Include(w => w.Items)
                .Include(w => w.Expenses)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (workOrder == null)
                return 0;

            decimal materialCost = await GetMaterialCostAsync(id);
            decimal expenseCost = workOrder.Expenses?.Sum(e => e.Amount) ?? 0;
            decimal laborCost = await GetLaborCostAsync(id);

            return materialCost + expenseCost + laborCost;
        }

        public async Task<IEnumerable<WorkOrder>> GetAllAsyncWithStatusPriorityClient()
        {
            return await _dbContext.WorkOrders
                .Include(w => w.Status)
                .Include(w => w.Priority)
                .Include(w => w.Client)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrderItem>> GetItemsByWorkOrderIdAsync(Guid workOrderId)
        {
            return await _dbContext.WorkOrderItems
                .Where(item => item.WorkOrderId == workOrderId)
                .ToListAsync();
        }

        public async Task AddItemAsync(WorkOrderItem item)
        {
            await _dbContext.WorkOrderItems.AddAsync(item);
        }

        public async Task<IEnumerable<WorkOrderItem>> GetAllItemsAsync()
        {
            return await _dbContext.WorkOrderItems.ToListAsync();
        }

        private async Task<decimal> GetMaterialCostAsync(Guid workOrderId)
        {
            var materials = await _dbContext.MaterialAssignments
                .Where(m => m.WorkOrderId == workOrderId && m.MaterialType == Domain.Enums.MaterialType.Purchasable)
                .Join(_dbContext.PurchasableMaterials,
                    ma => ma.PurchasableMaterialId,
                    pm => pm.Id,
                    (ma, pm) => new { Material = pm })
                .ToListAsync();

            return materials.Sum(m => m.Material.TotalCost ?? 0m);
        }

        private async Task<decimal> GetLaborCostAsync(Guid workOrderId)
        {
            // This would be calculated based on manpower assignments and their hourly rates
            // For now, returning a placeholder
            return 0;
        }

        public async Task UpdateWorkOrderPermitsAsync(Guid workOrderId, IEnumerable<Permit> newPermits, string currentUser)
        {
            var existingPermits = await _dbContext.Permits
                .Where(p => p.WorkOrderId == workOrderId)
                .ToListAsync();

            _dbContext.Permits.RemoveRange(existingPermits);

            var now = DateTime.UtcNow;
            var permitsToAdd = newPermits.Select(p => new Permit
            {
                Id = Guid.NewGuid(),
                WorkOrderId = workOrderId,
                Type = p.Type,
                Status = p.Status,
                Title = p.Title ?? p.Type,
                IssueDate = now,
                ExpiryDate = now.AddYears(1),
                Authority = p.Authority ?? "",
                Description = p.Description ?? "",
                Number = p.Number ?? "",
                IssuedBy = p.IssuedBy ?? "",
                DocumentRef = p.DocumentRef ?? "",
                CreatedAt = now,
                CreatedBy = currentUser,
                LastModifiedAt = now,
                LastModifiedBy = currentUser
            }).ToList();

            await _dbContext.Permits.AddRangeAsync(permitsToAdd);

            await _dbContext.SaveChangesAsync();
        }
    }
} 