using System;
using System.Collections.Generic;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Events.HR;

namespace UnifiedContract.Domain.Entities.HR
{
    public class Salary : BaseEntity
    {
        private decimal _baseSalary;
        private string _currency = string.Empty;
        private DateTime _effectiveDate;
        private DateTime? _endDate;
        private bool _isActive;
        private string _notes = string.Empty;
        private readonly List<Allowance> _allowances = new List<Allowance>();
        private readonly List<Deduction> _deductions = new List<Deduction>();

        // Foreign Keys
        public Guid EmployeeId { get; private set; }
        
        // Navigation Properties
        public Employee? Employee { get; private set; }
        
        // Collection Navigation Properties
        public IReadOnlyCollection<Allowance> Allowances => _allowances.AsReadOnly();
        public IReadOnlyCollection<Deduction> Deductions => _deductions.AsReadOnly();
        
        // Public properties with private setters
        public decimal BaseSalary => _baseSalary;
        public string Currency => _currency;
        public DateTime EffectiveDate => _effectiveDate;
        public DateTime? EndDate => _endDate;
        public bool IsActive => _isActive;
        public string Notes => _notes;
        
        // Required by EF Core
        private Salary() { }
        
        public Salary(
            Guid employeeId,
            decimal baseSalary,
            string currency,
            DateTime effectiveDate,
            string? notes = null,
            DateTime? endDate = null)
        {
            ValidateSalary(baseSalary, currency, effectiveDate, endDate);
            
            EmployeeId = employeeId;
            _baseSalary = baseSalary;
            _currency = currency;
            _effectiveDate = effectiveDate;
            _endDate = endDate;
            _isActive = true;
            _notes = notes ?? string.Empty;
            
            AddDomainEvent(new SalaryAddedEvent(this));
        }
        
        public void UpdateDetails(
            decimal? baseSalary = null,
            string? currency = null,
            DateTime? effectiveDate = null,
            string? notes = null,
            DateTime? endDate = default)
        {
            bool changed = false;
            
            if (baseSalary.HasValue && baseSalary != _baseSalary)
            {
                ValidateBaseSalary(baseSalary.Value);
                _baseSalary = baseSalary.Value;
                changed = true;
            }
            
            if (currency != null && currency != _currency)
            {
                ValidateCurrency(currency);
                _currency = currency;
                changed = true;
            }
            
            if (effectiveDate.HasValue && effectiveDate != _effectiveDate)
            {
                ValidateEffectiveDate(effectiveDate.Value, _endDate);
                _effectiveDate = effectiveDate.Value;
                changed = true;
            }
            
            if (notes != null && notes != _notes)
            {
                _notes = notes;
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
                AddDomainEvent(new SalaryUpdatedEvent(this));
            }
        }
        
        public void Activate()
        {
            if (!_isActive)
            {
                _isActive = true;
                AddDomainEvent(new SalaryActivatedEvent(this));
            }
        }
        
        public void Deactivate()
        {
            if (_isActive)
            {
                _isActive = false;
                AddDomainEvent(new SalaryDeactivatedEvent(this));
            }
        }
        
        public void Terminate(DateTime endDate)
        {
            if (endDate < _effectiveDate)
            {
                throw new ArgumentException("End date cannot be before effective date.", nameof(endDate));
            }
            
            if (_endDate.HasValue && endDate > _endDate.Value)
            {
                throw new ArgumentException("End date cannot be after existing end date.", nameof(endDate));
            }
            
            _endDate = endDate;
            _isActive = false;
            AddDomainEvent(new SalaryTerminatedEvent(this));
        }
        
        public void AddAllowance(Allowance allowance)
        {
            if (allowance == null)
            {
                throw new ArgumentNullException(nameof(allowance), "Allowance cannot be null.");
            }
            
            _allowances.Add(allowance);
        }
        
        public void AddDeduction(Deduction deduction)
        {
            if (deduction == null)
            {
                throw new ArgumentNullException(nameof(deduction), "Deduction cannot be null.");
            }
            
            _deductions.Add(deduction);
        }
        
        public decimal CalculateTotalAllowances(DateTime? calculationDate = null)
        {
            var dateToCheck = calculationDate ?? DateTime.UtcNow;
            decimal total = 0;
            
            foreach (var allowance in _allowances)
            {
                if (allowance.EffectiveDate <= dateToCheck && 
                    (!allowance.EndDate.HasValue || allowance.EndDate >= dateToCheck))
                {
                    total += allowance.Amount;
                }
            }
            
            return total;
        }
        
        public decimal CalculateTotalDeductions(DateTime? calculationDate = null)
        {
            var dateToCheck = calculationDate ?? DateTime.UtcNow;
            decimal total = 0;
            
            foreach (var deduction in _deductions)
            {
                if (deduction.EffectiveDate <= dateToCheck && 
                    (!deduction.EndDate.HasValue || deduction.EndDate >= dateToCheck))
                {
                    total += deduction.Amount;
                }
            }
            
            return total;
        }
        
        public decimal CalculateNetSalary(DateTime? calculationDate = null)
        {
            var totalAllowances = CalculateTotalAllowances(calculationDate);
            var totalDeductions = CalculateTotalDeductions(calculationDate);
            
            return _baseSalary + totalAllowances - totalDeductions;
        }
        
        public bool IsActiveOn(DateTime date)
        {
            return _isActive && _effectiveDate <= date && (!_endDate.HasValue || _endDate >= date);
        }
        
        private void ValidateSalary(decimal baseSalary, string currency, DateTime effectiveDate, DateTime? endDate)
        {
            ValidateBaseSalary(baseSalary);
            ValidateCurrency(currency);
            ValidateEffectiveDate(effectiveDate, endDate);
            
            if (endDate.HasValue)
            {
                ValidateEndDate(effectiveDate, endDate.Value);
            }
        }
        
        private void ValidateBaseSalary(decimal baseSalary)
        {
            if (baseSalary < 0)
            {
                throw new ArgumentException("Base salary cannot be negative.", nameof(baseSalary));
            }
        }
        
        private void ValidateCurrency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException("Currency is required.", nameof(currency));
            }
            
            if (currency.Length != 3)
            {
                throw new ArgumentException("Currency should be a 3-letter ISO currency code.", nameof(currency));
            }
        }
        
        private void ValidateEffectiveDate(DateTime effectiveDate, DateTime? endDate)
        {
            if (effectiveDate == default)
            {
                throw new ArgumentException("Effective date must be set.", nameof(effectiveDate));
            }
            
            if (endDate.HasValue && effectiveDate > endDate.Value)
            {
                throw new ArgumentException("Effective date cannot be after end date.", nameof(effectiveDate));
            }
        }
        
        private void ValidateEndDate(DateTime effectiveDate, DateTime endDate)
        {
            if (endDate < effectiveDate)
            {
                throw new ArgumentException("End date cannot be before effective date.", nameof(endDate));
            }
        }
    }
} 