using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Domain.Interfaces.Services.HR
{
    public interface ISkillService
    {
        /// <summary>
        /// Gets all skills
        /// </summary>
        Task<IEnumerable<Skill>> GetAllSkillsAsync();
        
        /// <summary>
        /// Gets all active skills
        /// </summary>
        Task<IEnumerable<Skill>> GetActiveSkillsAsync();
        
        /// <summary>
        /// Gets skills by category
        /// </summary>
        Task<IEnumerable<Skill>> GetSkillsByCategoryAsync(string category);
        
        /// <summary>
        /// Gets a skill by id
        /// </summary>
        Task<Skill> GetSkillByIdAsync(Guid id);
        
        /// <summary>
        /// Creates a new skill
        /// </summary>
        Task<Skill> CreateSkillAsync(string name, string description, string category);
        
        /// <summary>
        /// Updates an existing skill
        /// </summary>
        Task<Skill> UpdateSkillAsync(Guid id, string name, string description, string category);
        
        /// <summary>
        /// Activates a skill
        /// </summary>
        Task<Skill> ActivateSkillAsync(Guid id);
        
        /// <summary>
        /// Deactivates a skill
        /// </summary>
        Task<Skill> DeactivateSkillAsync(Guid id);
        
        /// <summary>
        /// Gets all skill categories
        /// </summary>
        Task<IEnumerable<string>> GetAllCategoriesAsync();
        
        /// <summary>
        /// Checks if a skill with the same name already exists
        /// </summary>
        Task<bool> SkillExistsAsync(string name);
    }
} 