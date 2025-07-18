using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnifiedContract.Domain.Entities.Resource;

namespace UnifiedContract.Domain.Interfaces.Services
{
    public interface IEquipmentService
    {
        Task<EquipmentAssignment> AssignEquipmentToWorkOrderAsync(string companyNumber, Guid workOrderId, DateTime startDate, DateTime? endDate, int hoursAssigned);
        Task<bool> UnassignEquipmentFromWorkOrderAsync(Guid assignmentId);
        Task<IEnumerable<EquipmentAssignment>> GetEquipmentAssignmentsByWorkOrderAsync(Guid workOrderId);
        Task<IEnumerable<EquipmentAssignment>> GetEquipmentAssignmentsByCompanyNumberAsync(string companyNumber);
        Task<decimal> CalculateTotalEquipmentCostByWorkOrderAsync(Guid workOrderId);
        Task<EquipmentAssignment> UpdateEquipmentAssignmentAsync(EquipmentAssignment assignment);
        Task<bool> ExtendEquipmentAssignmentAsync(Guid assignmentId, DateTime newEndDate);
        Task<bool> AssignOperatorToEquipmentAsync(Guid equipmentAssignmentId, string operatorBadgeNumber);
    }
} 