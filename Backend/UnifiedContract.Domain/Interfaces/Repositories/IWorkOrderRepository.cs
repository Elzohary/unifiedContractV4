using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnifiedContract.Domain.Entities.WorkOrder;

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
    }
} 