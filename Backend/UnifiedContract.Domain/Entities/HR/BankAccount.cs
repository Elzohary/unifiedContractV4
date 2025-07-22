using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Events.HR;
using UnifiedContract.Domain.Exceptions;

namespace UnifiedContract.Domain.Entities.HR
{
    public class BankAccount : BaseEntity
    {
        private string _bankName;
        private string _accountNumber;
        private string _iban;
        private string _swiftCode;
        private string _branchName;
        private string _branchCode;
        private bool _isPrimary;
        
        // Foreign Key
        public Guid EmployeeId { get; private set; }
        
        // Navigation Property
        public virtual Employee Employee { get; private set; }
        
        // Public properties with private setters
        public string BankName => _bankName;
        public string AccountNumber => _accountNumber;
        public string IBAN { get => _iban; private set => _iban = value; }
        public string SwiftCode => _swiftCode;
        public string BranchName => _branchName;
        public string BranchCode => _branchCode;
        public bool IsPrimary => _isPrimary;
        
        // Required by EF Core
        private BankAccount() { }
        
        public BankAccount(
            Guid employeeId,
            string bankName,
            string accountNumber,
            string iban = null,
            string swiftCode = null,
            string branchName = null,
            string branchCode = null,
            bool isPrimary = false)
        {
            ValidateBankAccount(bankName, accountNumber);
            
            EmployeeId = employeeId;
            _bankName = bankName;
            _accountNumber = accountNumber;
            _iban = iban;
            _swiftCode = swiftCode;
            _branchName = branchName;
            _branchCode = branchCode;
            _isPrimary = isPrimary;
            
            AddDomainEvent(new BankAccountAddedEvent(this));
        }
        
        public void UpdateDetails(
            string bankName = null,
            string accountNumber = null,
            string iban = null,
            string swiftCode = null,
            string branchName = null,
            string branchCode = null)
        {
            bool changed = false;
            
            if (bankName != null && bankName != _bankName)
            {
                ValidateBankName(bankName);
                _bankName = bankName;
                changed = true;
            }
            
            if (accountNumber != null && accountNumber != _accountNumber)
            {
                ValidateAccountNumber(accountNumber);
                _accountNumber = accountNumber;
                changed = true;
            }
            
            if (iban != null && iban != _iban)
            {
                _iban = iban;
                changed = true;
            }
            
            if (swiftCode != null && swiftCode != _swiftCode)
            {
                _swiftCode = swiftCode;
                changed = true;
            }
            
            if (branchName != null && branchName != _branchName)
            {
                _branchName = branchName;
                changed = true;
            }
            
            if (branchCode != null && branchCode != _branchCode)
            {
                _branchCode = branchCode;
                changed = true;
            }
            
            if (changed)
            {
                AddDomainEvent(new BankAccountUpdatedEvent(this));
            }
        }
        
        public void SetAsPrimary()
        {
            if (!_isPrimary)
            {
                _isPrimary = true;
                AddDomainEvent(new BankAccountSetAsPrimaryEvent(this));
            }
        }
        
        public void UnsetAsPrimary()
        {
            if (_isPrimary)
            {
                _isPrimary = false;
                AddDomainEvent(new BankAccountUnsetAsPrimaryEvent(this));
            }
        }
        
        private void ValidateBankAccount(string bankName, string accountNumber)
        {
            ValidateBankName(bankName);
            ValidateAccountNumber(accountNumber);
        }
        
        private void ValidateBankName(string bankName)
        {
            if (string.IsNullOrWhiteSpace(bankName))
            {
                throw new BusinessRuleException("Bank name is required.");
            }
            
            if (bankName.Length > 100)
            {
                throw new BusinessRuleException("Bank name cannot exceed 100 characters.");
            }
        }
        
        private void ValidateAccountNumber(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                throw new BusinessRuleException("Account number is required.");
            }
            
            if (accountNumber.Length > 50)
            {
                throw new BusinessRuleException("Account number cannot exceed 50 characters.");
            }
        }
    }
} 