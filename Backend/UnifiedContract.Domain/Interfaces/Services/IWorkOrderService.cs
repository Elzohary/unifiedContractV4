using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Domain.Interfaces.Services
{
    public interface IWorkOrderService
    {
        Task<WorkOrder> CreateWorkOrderAsync(WorkOrder workOrder);
        Task<WorkOrder> UpdateWorkOrderAsync(WorkOrder workOrder);
        Task<bool> DeleteWorkOrderAsync(Guid id);
        Task<WorkOrder> GetWorkOrderByIdAsync(Guid id);
        Task<IEnumerable<WorkOrder>> GetAllWorkOrdersAsync();
        Task<IEnumerable<WorkOrder>> GetWorkOrdersByStatusAsync(string status);
        Task<IEnumerable<WorkOrder>> GetWorkOrdersByClientAsync(string client);
        Task<decimal> CalculateWorkOrderEstimatedCostAsync(Guid workOrderId);
        Task<decimal> CalculateWorkOrderActualCostAsync(Guid workOrderId);
        Task<bool> AssignEngineerToWorkOrderAsync(Guid workOrderId, Guid engineerId);
        Task<bool> UpdateWorkOrderStatusAsync(Guid workOrderId, string status);
        Task<bool> UpdateWorkOrderCompletionPercentageAsync(Guid workOrderId, decimal completionPercentage);
    }
} 