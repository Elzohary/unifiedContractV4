using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnifiedContract.Domain.Entities.WorkOrder;
using UnifiedContract.Domain.Entities.Resource;

namespace UnifiedContract.Domain.Interfaces.Repositories
{
    public interface IWorkOrderRepository : IRepository<WorkOrder>
    {
        Task<WorkOrder> GetWorkOrderWithDetailsAsync(Guid id);
        Task<IEnumerable<WorkOrder>> GetWorkOrdersByClientAsync(string client);
        Task<IEnumerable<WorkOrder>> GetWorkOrdersByEngineerAsync(Guid engineerId);
        Task<IEnumerable<WorkOrder>> GetWorkOrdersWithStatusAsync(string status);
        Task<IEnumerable<WorkOrder>> GetWorkOrdersForPeriodAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalEstimatedCostAsync(Guid id);
        Task<decimal> GetTotalActualCostAsync(Guid id);
        Task<IEnumerable<WorkOrder>> GetAllAsyncWithStatusPriorityClient();
        Task<IEnumerable<WorkOrderItem>> GetItemsByWorkOrderIdAsync(Guid workOrderId);
        Task AddItemAsync(WorkOrderItem item);
        Task<WorkOrderItem> GetItemByIdAsync(Guid itemId);
        Task UpdateItemAsync(WorkOrderItem item);
        Task<IEnumerable<WorkOrderItem>> GetAllItemsAsync();
        Task UpdateWorkOrderPermitsAsync(Guid workOrderId, IEnumerable<Permit> permits, string currentUser);
        
        // Material assignment methods
        Task AddMaterialAssignmentAsync(MaterialAssignment materialAssignment);
        Task<MaterialAssignment> GetMaterialAssignmentByIdAsync(Guid assignmentId);
        Task<IEnumerable<MaterialAssignment>> GetMaterialAssignmentsByWorkOrderIdAsync(Guid workOrderId);
        Task UpdateMaterialAssignmentAsync(MaterialAssignment materialAssignment);
        Task RemoveMaterialAssignmentAsync(Guid assignmentId);
    }
} 