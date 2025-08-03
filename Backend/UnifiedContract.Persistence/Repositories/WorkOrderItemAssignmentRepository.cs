using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnifiedContract.Domain.Entities.WorkOrder;
using UnifiedContract.Domain.Interfaces.Repositories;
using UnifiedContract.Persistence;

namespace UnifiedContract.Persistence.Repositories
{
    public class WorkOrderItemAssignmentRepository : Repository<WorkOrderItemAssignment>, IWorkOrderItemAssignmentRepository
    {
        public WorkOrderItemAssignmentRepository(UnifiedContractDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<WorkOrderItemAssignment>> GetAssignmentsByWorkOrderIdAsync(Guid workOrderId)
        {
            return await _dbContext.WorkOrderItemAssignments
                .Where(assignment => assignment.WorkOrderId == workOrderId)
                .Include(assignment => assignment.Item)
                .ThenInclude(item => item.Client)
                .ToListAsync();
        }

        public async Task<WorkOrderItemAssignment> GetAssignmentByIdAsync(Guid assignmentId)
        {
            return await _dbContext.WorkOrderItemAssignments
                .Include(assignment => assignment.Item)
                .ThenInclude(item => item.Client)
                .FirstOrDefaultAsync(assignment => assignment.Id == assignmentId);
        }

        public async Task<bool> AssignmentExistsAsync(Guid workOrderId, Guid itemId)
        {
            return await _dbContext.WorkOrderItemAssignments
                .AnyAsync(assignment => assignment.WorkOrderId == workOrderId && assignment.ItemId == itemId);
        }

        public async Task RemoveAssignmentAsync(Guid assignmentId)
        {
            var assignment = await _dbContext.WorkOrderItemAssignments
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment != null)
            {
                _dbContext.WorkOrderItemAssignments.Remove(assignment);
            }
        }
    }
} 