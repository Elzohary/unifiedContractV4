using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnifiedContract.Domain.Entities.Resource;

namespace UnifiedContract.Domain.Interfaces.Services
{
    public interface IMaterialService
    {
        Task<PurchasableMaterial> CreatePurchasableMaterialAsync(PurchasableMaterial material);
        Task<ReceivableMaterial> CreateReceivableMaterialAsync(ReceivableMaterial material);
        Task<MaterialAssignment> AssignMaterialToWorkOrderAsync(Guid materialId, Guid workOrderId, string materialType);
        Task<bool> UnassignMaterialFromWorkOrderAsync(Guid assignmentId);
        Task<IEnumerable<MaterialAssignment>> GetMaterialAssignmentsByWorkOrderAsync(Guid workOrderId);
        Task<IEnumerable<PurchasableMaterial>> GetAllPurchasableMaterialsAsync();
        Task<IEnumerable<ReceivableMaterial>> GetAllReceivableMaterialsAsync();
        Task<decimal> CalculateTotalMaterialCostByWorkOrderAsync(Guid workOrderId);
        Task<PurchasableMaterial> UpdatePurchasableMaterialAsync(PurchasableMaterial material);
        Task<ReceivableMaterial> UpdateReceivableMaterialAsync(ReceivableMaterial material);
        Task<bool> ReceiveMaterialAsync(Guid receivableMaterialId, decimal quantity, Guid receivedById);
        Task<bool> ReturnMaterialAsync(Guid receivableMaterialId, decimal quantity, Guid returnedById);
    }
} 