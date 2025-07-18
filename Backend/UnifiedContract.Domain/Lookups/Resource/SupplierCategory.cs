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
    /// Categories for suppliers (Materials, Equipment, Services, etc.)
    /// </summary>
    public class SupplierCategory : Lookup
    {
        private bool _requiresVatNumber;
        private decimal _defaultCreditDays = 30;
        private bool _requiresContractReview;
        private string _colorCode;
        private readonly HashSet<Supplier> _suppliers = new HashSet<Supplier>();

        /// <summary>
        /// Whether suppliers in this category require a VAT number
        /// </summary>
        public bool RequiresVatNumber => _requiresVatNumber;
        
        /// <summary>
        /// Default credit days for suppliers in this category
        /// </summary>
        public decimal DefaultCreditDays => _defaultCreditDays;
        
        /// <summary>
        /// Whether suppliers in this category require contract review
        /// </summary>
        public bool RequiresContractReview => _requiresContractReview;
        
        /// <summary>
        /// Color code for UI representation (hex or named color)
        /// </summary>
        public string ColorCode => _colorCode;
        
        /// <summary>
        /// Navigation property for suppliers in this category
        /// </summary>
        public virtual IReadOnlyCollection<Supplier> Suppliers => _suppliers;

        // Required by EF Core
        public SupplierCategory() { }
        
        public SupplierCategory(
            string code, 
            string name, 
            string description, 
            bool requiresVatNumber, 
            decimal defaultCreditDays, 
            bool requiresContractReview,
            string? colorCode = null)
        {
            ValidateCodeAndName(code, name);
            ValidateDefaultCreditDays(defaultCreditDays);
            
            Code = code;
            Name = name;
            Description = description;
            _requiresVatNumber = requiresVatNumber;
            _defaultCreditDays = defaultCreditDays;
            _requiresContractReview = requiresContractReview;
            _colorCode = colorCode ?? string.Empty;
            
            IsActive = true;
            
            AddDomainEvent(new LookupCreatedEvent(this));
        }
        
        public void UpdateProperties(
            bool? requiresVatNumber = null, 
            decimal? defaultCreditDays = null, 
            bool? requiresContractReview = null, 
            string? colorCode = null)
        {
            bool changed = false;
            
            if (requiresVatNumber.HasValue && requiresVatNumber.Value != _requiresVatNumber)
            {
                _requiresVatNumber = requiresVatNumber.Value;
                changed = true;
            }
            
            if (defaultCreditDays.HasValue && defaultCreditDays.Value != _defaultCreditDays)
            {
                ValidateDefaultCreditDays(defaultCreditDays.Value);
                _defaultCreditDays = defaultCreditDays.Value;
                changed = true;
            }
            
            if (requiresContractReview.HasValue && requiresContractReview.Value != _requiresContractReview)
            {
                _requiresContractReview = requiresContractReview.Value;
                changed = true;
            }
            
            if (colorCode != null && colorCode != _colorCode)
            {
                _colorCode = colorCode;
                changed = true;
            }
            
            if (changed)
            {
                AddDomainEvent(new LookupUpdatedEvent(this));
            }
        }
        
        public void SetActiveStatus(bool active)
        {
            if (active != IsActive)
            {
                IsActive = active;
                
                if (active)
                {
                    AddDomainEvent(new LookupReactivatedEvent(this));
                }
                else
                {
                    AddDomainEvent(new LookupDeactivatedEvent(this));
                }
            }
        }
        
        public void AddSupplier(Supplier supplier)
        {
            if (supplier != null && !_suppliers.Contains(supplier))
            {
                _suppliers.Add(supplier);
            }
        }
        
        public void RemoveSupplier(Supplier supplier)
        {
            if (supplier != null && _suppliers.Contains(supplier))
            {
                _suppliers.Remove(supplier);
            }
        }
        
        private void ValidateCodeAndName(string code, string name)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new BusinessRuleException("Supplier category code cannot be empty");
                
            if (string.IsNullOrWhiteSpace(name))
                throw new BusinessRuleException("Supplier category name cannot be empty");
                
            if (code.Length > 50)
                throw new BusinessRuleException("Supplier category code cannot exceed 50 characters");
                
            if (name.Length > 100)
                throw new BusinessRuleException("Supplier category name cannot exceed 100 characters");
        }
        
        private void ValidateDefaultCreditDays(decimal creditDays)
        {
            if (creditDays < 0)
                throw new BusinessRuleException("Default credit days cannot be negative");
                
            if (creditDays > 180)
                throw new BusinessRuleException("Default credit days cannot exceed 180 days");
        }
    }
} 