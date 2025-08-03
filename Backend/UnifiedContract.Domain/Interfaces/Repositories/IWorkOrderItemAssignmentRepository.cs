using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Domain.Interfaces.Repositories
{
    public interface IWorkOrderItemAssignmentRepository : IRepository<WorkOrderItemAssignment>
    {
        Task<IEnumerable<WorkOrderItemAssignment>> GetAssignmentsByWorkOrderIdAsync(Guid workOrderId);
        Task<WorkOrderItemAssignment> GetAssignmentByIdAsync(Guid assignmentId);
        Task<bool> AssignmentExistsAsync(Guid workOrderId, Guid itemId);
        Task RemoveAssignmentAsync(Guid assignmentId);
    }
} 