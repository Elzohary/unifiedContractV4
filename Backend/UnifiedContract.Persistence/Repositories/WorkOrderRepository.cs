using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnifiedContract.Domain.Entities.WorkOrder;
using UnifiedContract.Domain.Entities.Resource;
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
                .Include(w => w.ItemAssignments)
                    .ThenInclude(ia => ia.Item)
                        .ThenInclude(item => item.Client)
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
                .Include(w => w.ItemAssignments)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (workOrder == null)
                return 0;

            return workOrder.EstimatedCost ?? 0m;
        }

        public async Task<decimal> GetTotalActualCostAsync(Guid id)
        {
            var workOrder = await _dbContext.WorkOrders
                .Include(w => w.ItemAssignments)
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

        public async Task<WorkOrderItem> GetItemByIdAsync(Guid itemId)
        {
            return await _dbContext.WorkOrderItems.FirstOrDefaultAsync(item => item.Id == itemId);
        }

        public async Task UpdateItemAsync(WorkOrderItem item)
        {
            _dbContext.WorkOrderItems.Update(item);
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

        // Material assignment methods
        public async Task AddMaterialAssignmentAsync(MaterialAssignment materialAssignment)
        {
            await _dbContext.MaterialAssignments.AddAsync(materialAssignment);
        }

        public async Task<MaterialAssignment> GetMaterialAssignmentByIdAsync(Guid assignmentId)
        {
            return await _dbContext.MaterialAssignments
                .Include(ma => ma.PurchasableMaterial)
                .Include(ma => ma.ReceivableMaterial)
                .FirstOrDefaultAsync(ma => ma.Id == assignmentId);
        }

        public async Task<IEnumerable<MaterialAssignment>> GetMaterialAssignmentsByWorkOrderIdAsync(Guid workOrderId)
        {
            return await _dbContext.MaterialAssignments
                .Include(ma => ma.PurchasableMaterial)
                .Include(ma => ma.ReceivableMaterial)
                .Where(ma => ma.WorkOrderId == workOrderId)
                .ToListAsync();
        }

        public async Task UpdateMaterialAssignmentAsync(MaterialAssignment materialAssignment)
        {
            _dbContext.MaterialAssignments.Update(materialAssignment);
        }

        public async Task RemoveMaterialAssignmentAsync(Guid assignmentId)
        {
            var materialAssignment = await _dbContext.MaterialAssignments.FindAsync(assignmentId);
            if (materialAssignment != null)
            {
                _dbContext.MaterialAssignments.Remove(materialAssignment);
            }
        }
    }
} 