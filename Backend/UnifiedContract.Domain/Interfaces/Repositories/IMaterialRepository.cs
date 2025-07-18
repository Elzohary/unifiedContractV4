using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnifiedContract.Domain.Entities.Resource;

namespace UnifiedContract.Domain.Interfaces.Repositories
{
    public interface IMaterialRepository : IRepository<MaterialAssignment>
    {
        Task<IEnumerable<PurchasableMaterial>> GetAllPurchasableMaterialsAsync();
        Task<IEnumerable<ReceivableMaterial>> GetAllReceivableMaterialsAsync();
        Task<IEnumerable<MaterialAssignment>> GetMaterialAssignmentsByWorkOrderAsync(Guid workOrderId);
        Task<IEnumerable<MaterialAssignment>> GetUnassignedMaterialsAsync();
        Task<decimal> GetTotalMaterialCostByWorkOrderAsync(Guid workOrderId);
    }
} 