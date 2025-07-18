using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnifiedContract.Domain.Entities.Resource;

namespace UnifiedContract.Domain.Interfaces.Services
{
    public interface IManpowerService
    {
        Task<ManpowerAssignment> AssignManpowerToWorkOrderAsync(string badgeNumber, Guid workOrderId, DateTime startDate, DateTime? endDate, int hoursAssigned);
        Task<bool> UnassignManpowerFromWorkOrderAsync(Guid assignmentId);
        Task<IEnumerable<ManpowerAssignment>> GetManpowerAssignmentsByWorkOrderAsync(Guid workOrderId);
        Task<IEnumerable<ManpowerAssignment>> GetManpowerAssignmentsByBadgeNumberAsync(string badgeNumber);
        Task<decimal> CalculateTotalManpowerCostByWorkOrderAsync(Guid workOrderId);
        Task<ManpowerAssignment> UpdateManpowerAssignmentAsync(ManpowerAssignment assignment);
        Task<bool> ExtendManpowerAssignmentAsync(Guid assignmentId, DateTime newEndDate);
        Task<bool> UpdateManpowerHoursAsync(Guid assignmentId, int newHoursAssigned);
    }
} 