using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Domain.Interfaces.Services.HR
{
    public interface IEmployeeSkillService
    {
        /// <summary>
        /// Gets all skills for a specific employee
        /// </summary>
        Task<IEnumerable<EmployeeSkill>> GetEmployeeSkillsAsync(Guid employeeId);
        
        /// <summary>
        /// Gets all featured skills for a specific employee
        /// </summary>
        Task<IEnumerable<EmployeeSkill>> GetFeaturedSkillsAsync(Guid employeeId);
        
        /// <summary>
        /// Adds a new skill to an employee
        /// </summary>
        Task<EmployeeSkill> AddSkillToEmployeeAsync(Guid employeeId, Guid skillId, int proficiencyLevel, DateTime acquiredDate, string certificate = null, string notes = null);
        
        /// <summary>
        /// Updates an existing employee skill
        /// </summary>
        Task<EmployeeSkill> UpdateEmployeeSkillAsync(Guid employeeSkillId, int proficiencyLevel, DateTime acquiredDate, string certificate = null, string notes = null, DateTime? lastUsedDate = null);
        
        /// <summary>
        /// Sets a skill as featured or not featured
        /// </summary>
        Task<EmployeeSkill> SetSkillFeaturedStatusAsync(Guid employeeSkillId, bool isFeatured);
        
        /// <summary>
        /// Removes a skill from an employee
        /// </summary>
        Task RemoveSkillFromEmployeeAsync(Guid employeeSkillId);
        
        /// <summary>
        /// Updates the last used date for a skill
        /// </summary>
        Task<EmployeeSkill> UpdateSkillLastUsedDateAsync(Guid employeeSkillId, DateTime lastUsedDate);
        
        /// <summary>
        /// Gets all employees with a specific skill
        /// </summary>
        Task<IEnumerable<Employee>> GetEmployeesWithSkillAsync(Guid skillId, int? minimumProficiency = null);
        
        /// <summary>
        /// Gets skill statistics for all employees
        /// </summary>
        Task<IDictionary<string, int>> GetSkillStatisticsAsync();
    }
} 