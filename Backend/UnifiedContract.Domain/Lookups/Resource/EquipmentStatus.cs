using System;
using System.Collections.Generic;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.Common;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Domain.Events.Resource;
using UnifiedContract.Domain.Events.Common;
using UnifiedContract.Domain.Exceptions;

namespace UnifiedContract.Domain.Entities.Resource.Lookups
{
    /// <summary>
    /// Status options for equipment (Available, InUse, UnderMaintenance, Damaged, Retired)
    /// </summary>
    public class EquipmentStatus : Lookup
    {
        private bool _canBeAssigned;
        private bool _incursCost = true;
        private bool _isOperational = true;
        private string _colorCode;
        private readonly HashSet<Equipment> _equipment = new HashSet<Equipment>();

        /// <summary>
        /// Whether equipment with this status can be assigned to work orders
        /// </summary>
        public bool CanBeAssigned => _canBeAssigned;
        
        /// <summary>
        /// Whether equipment with this status incurs daily costs
        /// </summary>
        public bool IncursCost => _incursCost;
        
        /// <summary>
        /// Whether this is an operational status
        /// </summary>
        public bool IsOperational => _isOperational;
        
        /// <summary>
        /// Color code for UI representation (hex or named color)
        /// </summary>
        public string ColorCode => _colorCode;
        
        /// <summary>
        /// Navigation property for equipment with this status
        /// </summary>
        public virtual IReadOnlyCollection<Equipment> Equipment => _equipment;

        // Required by EF Core
        public EquipmentStatus() { }
        
        public EquipmentStatus(
            string code, 
            string name, 
            string description, 
            bool canBeAssigned, 
            bool incursCost, 
            bool isOperational, 
            string? colorCode = null)
        {
            ValidateCodeAndName(code, name);
            
            Code = code;
            Name = name;
            Description = description;
            _canBeAssigned = canBeAssigned;
            _incursCost = incursCost;
            _isOperational = isOperational;
            _colorCode = colorCode ?? string.Empty;
            
            IsActive = true;
        }
        
        public void UpdateProperties(
            bool? canBeAssigned = null, 
            bool? incursCost = null, 
            bool? isOperational = null, 
            string? colorCode = null)
        {
            if (canBeAssigned.HasValue && canBeAssigned.Value != _canBeAssigned)
            {
                _canBeAssigned = canBeAssigned.Value;
            }
            
            if (incursCost.HasValue && incursCost.Value != _incursCost)
            {
                _incursCost = incursCost.Value;
            }
            
            if (isOperational.HasValue && isOperational.Value != _isOperational)
            {
                _isOperational = isOperational.Value;
            }
            
            if (colorCode != null && colorCode != _colorCode)
            {
                _colorCode = colorCode;
            }
            
            AddDomainEvent(new LookupUpdatedEvent(this));
        }
        
        public void AddEquipment(Equipment equipment)
        {
            if (equipment != null && !_equipment.Contains(equipment))
            {
                _equipment.Add(equipment);
            }
        }
        
        public void RemoveEquipment(Equipment equipment)
        {
            if (equipment != null && _equipment.Contains(equipment))
            {
                _equipment.Remove(equipment);
            }
        }
        
        private void ValidateCodeAndName(string code, string name)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new BusinessRuleException("Equipment status code cannot be empty");
                
            if (string.IsNullOrWhiteSpace(name))
                throw new BusinessRuleException("Equipment status name cannot be empty");
                
            if (code.Length > 50)
                throw new BusinessRuleException("Equipment status code cannot exceed 50 characters");
                
            if (name.Length > 100)
                throw new BusinessRuleException("Equipment status name cannot exceed 100 characters");
        }
    }
} 