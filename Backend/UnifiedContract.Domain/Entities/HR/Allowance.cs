using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Events.HR;
using UnifiedContract.Domain.Exceptions;

namespace UnifiedContract.Domain.Entities.HR
{
    public class Allowance : BaseEntity
    {
        private string _type;
        private string _description;
        private decimal _amount;
        private bool _isTaxable;
        private DateTime _effectiveDate;
        private DateTime? _endDate;

        // Foreign Key
        public Guid SalaryId { get; private set; }
        
        // Navigation properties
        public virtual Salary Salary { get; private set; }
        
        // Public properties with private setters
        public string Type => _type;
        public string Description => _description;
        public decimal Amount => _amount;
        public bool IsTaxable => _isTaxable;
        public DateTime EffectiveDate => _effectiveDate;
        public DateTime? EndDate => _endDate;
        
        // Required by EF Core
        private Allowance() { }
        
        public Allowance(
            Guid salaryId,
            string type,
            decimal amount,
            DateTime effectiveDate,
            string description = null,
            bool isTaxable = false,
            DateTime? endDate = null)
        {
            ValidateAllowance(type, amount, effectiveDate, endDate);
            
            SalaryId = salaryId;
            _type = type;
            _amount = amount;
            _effectiveDate = effectiveDate;
            _description = description ?? string.Empty;
            _isTaxable = isTaxable;
            _endDate = endDate;
            
            AddDomainEvent(new AllowanceAddedEvent(this));
        }
        
        public void UpdateDetails(
            string type = null,
            decimal? amount = null,
            string description = null,
            bool? isTaxable = null,
            DateTime? effectiveDate = null,
            DateTime? endDate = default)
        {
            bool changed = false;
            
            if (type != null && type != _type)
            {
                ValidateType(type);
                _type = type;
                changed = true;
            }
            
            if (amount.HasValue && amount != _amount)
            {
                ValidateAmount(amount.Value);
                _amount = amount.Value;
                changed = true;
            }
            
            if (description != null && description != _description)
            {
                _description = description;
                changed = true;
            }
            
            if (isTaxable.HasValue && isTaxable != _isTaxable)
            {
                _isTaxable = isTaxable.Value;
                changed = true;
            }
            
            if (effectiveDate.HasValue && effectiveDate != _effectiveDate)
            {
                ValidateEffectiveDate(effectiveDate.Value, _endDate);
                _effectiveDate = effectiveDate.Value;
                changed = true;
            }
            
            // Special handling for endDate because it's nullable
            if (endDate != default)
            {
                if (endDate != _endDate)
                {
                    if (endDate.HasValue)
                    {
                        ValidateEndDate(_effectiveDate, endDate.Value);
                    }
                    _endDate = endDate;
                    changed = true;
                }
            }
            
            if (changed)
            {
                AddDomainEvent(new AllowanceUpdatedEvent(this));
            }
        }
        
        public void Terminate(DateTime endDate)
        {
            if (endDate < _effectiveDate)
            {
                throw new BusinessRuleException("End date cannot be before effective date.");
            }
            
            if (_endDate.HasValue && endDate > _endDate.Value)
            {
                throw new BusinessRuleException("End date cannot be after existing end date.");
            }
            
            _endDate = endDate;
            AddDomainEvent(new AllowanceTerminatedEvent(this));
        }
        
        public bool IsActive(DateTime checkDate)
        {
            return _effectiveDate <= checkDate && (!_endDate.HasValue || _endDate >= checkDate);
        }
        
        private void ValidateAllowance(string type, decimal amount, DateTime effectiveDate, DateTime? endDate)
        {
            ValidateType(type);
            ValidateAmount(amount);
            ValidateEffectiveDate(effectiveDate, endDate);
            
            if (endDate.HasValue)
            {
                ValidateEndDate(effectiveDate, endDate.Value);
            }
        }
        
        private void ValidateType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new BusinessRuleException("Allowance type is required.");
            }
            
            if (type.Length > 50)
            {
                throw new BusinessRuleException("Allowance type cannot exceed 50 characters.");
            }
        }
        
        private void ValidateAmount(decimal amount)
        {
            if (amount < 0)
            {
                throw new BusinessRuleException("Allowance amount cannot be negative.");
            }
        }
        
        private void ValidateEffectiveDate(DateTime effectiveDate, DateTime? endDate)
        {
            if (effectiveDate == default)
            {
                throw new BusinessRuleException("Effective date must be set.");
            }
            
            if (endDate.HasValue && effectiveDate > endDate.Value)
            {
                throw new BusinessRuleException("Effective date cannot be after end date.");
            }
        }
        
        private void ValidateEndDate(DateTime effectiveDate, DateTime endDate)
        {
            if (endDate < effectiveDate)
            {
                throw new BusinessRuleException("End date cannot be before effective date.");
            }
        }
    }
} 