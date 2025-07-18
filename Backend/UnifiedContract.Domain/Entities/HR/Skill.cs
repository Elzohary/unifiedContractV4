using System;
using System.Collections.Generic;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Events.HR;

namespace UnifiedContract.Domain.Entities.HR
{
    public class Skill : BaseEntity
    {
        private string _name = string.Empty;
        private string _description = string.Empty;
        private string _category = string.Empty;
        private bool _isActive = true;
        
        // Navigation property for the many-to-many relationship
        private readonly List<EmployeeSkill> _employeeSkills = new List<EmployeeSkill>();
        
        // Public properties with private setters
        public string Name => _name;
        public string Description => _description;
        public string Category => _category;
        public bool IsActive => _isActive;
        public IReadOnlyCollection<EmployeeSkill> EmployeeSkills => _employeeSkills.AsReadOnly();
        
        // For EF Core
        private Skill() { }
        
        public Skill(string name, string description, string category)
        {
            ValidateInput(name, nameof(Name));
            ValidateInput(category, nameof(Category));
            
            _name = name;
            _description = description ?? string.Empty;
            _category = category;
            
            AddDomainEvent(new SkillCreatedEvent(this));
        }
        
        public void Update(string name, string description, string category)
        {
            ValidateInput(name, nameof(Name));
            ValidateInput(category, nameof(Category));
            
            _name = name;
            _description = description ?? string.Empty;
            _category = category;
            
            AddDomainEvent(new SkillUpdatedEvent(this));
        }
        
        public void Deactivate()
        {
            _isActive = false;
        }
        
        public void Activate()
        {
            _isActive = true;
        }
        
        private void ValidateInput(string value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{propertyName} cannot be null or empty", propertyName);
            }
        }

        public void AddEmployeeSkill(EmployeeSkill employeeSkill)
        {
            if (!_employeeSkills.Contains(employeeSkill))
            {
                _employeeSkills.Add(employeeSkill);
            }
        }

        public void RemoveEmployeeSkill(EmployeeSkill employeeSkill)
        {
            if (_employeeSkills.Contains(employeeSkill))
            {
                _employeeSkills.Remove(employeeSkill);
            }
        }
    }
} 