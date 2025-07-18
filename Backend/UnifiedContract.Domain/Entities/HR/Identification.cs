using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Events.HR;

namespace UnifiedContract.Domain.Entities.HR
{
    public class Identification : BaseEntity
    {
        public string Type { get; private set; } // Passport, National ID, Driver's License, etc.
        public string Number { get; private set; }
        public string IssuingCountry { get; private set; }
        public DateTime IssueDate { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public string DocumentUrl { get; set; }
        public bool IsVerified { get; private set; }
        
        // Foreign Key
        public Guid EmployeeId { get; private set; }
        
        // Navigation Property
        public virtual Employee Employee { get; set; }
        
        // For EF Core
        protected Identification() { }
        
        public Identification(
            Guid employeeId,
            string type,
            string number,
            string issuingCountry,
            DateTime issueDate,
            DateTime expiryDate,
            string documentUrl = null)
        {
            ValidateIdentification(type, number, issuingCountry, issueDate, expiryDate);
            
            EmployeeId = employeeId;
            Type = type;
            Number = number;
            IssuingCountry = issuingCountry;
            IssueDate = issueDate;
            ExpiryDate = expiryDate;
            DocumentUrl = documentUrl;
            IsVerified = false;
            
            AddDomainEvent(new IdentificationAddedEvent(this));
        }
        
        public void UpdateDetails(
            string type = null,
            string number = null,
            string issuingCountry = null,
            DateTime? issueDate = null,
            DateTime? expiryDate = null)
        {
            bool changed = false;
            
            if (type != null && type != Type)
            {
                ValidateType(type);
                Type = type;
                changed = true;
            }
            
            if (number != null && number != Number)
            {
                ValidateNumber(number);
                Number = number;
                changed = true;
            }
            
            if (issuingCountry != null && issuingCountry != IssuingCountry)
            {
                ValidateIssuingCountry(issuingCountry);
                IssuingCountry = issuingCountry;
                changed = true;
            }
            
            if (issueDate.HasValue && issueDate != IssueDate)
            {
                ValidateIssueDate(issueDate.Value);
                
                if (issueDate > ExpiryDate)
                    throw new DomainException("Issue date cannot be after expiry date");
                    
                IssueDate = issueDate.Value;
                changed = true;
            }
            
            if (expiryDate.HasValue && expiryDate != ExpiryDate)
            {
                if (expiryDate < IssueDate)
                    throw new DomainException("Expiry date cannot be before issue date");
                    
                ExpiryDate = expiryDate.Value;
                changed = true;
            }
            
            if (changed)
            {
                // Reset verification when details are updated
                IsVerified = false;
                AddDomainEvent(new IdentificationUpdatedEvent(this));
            }
        }
        
        public void Verify()
        {
            if (!IsVerified)
            {
                IsVerified = true;
                AddDomainEvent(new IdentificationVerifiedEvent(this));
            }
        }
        
        public void RejectVerification()
        {
            if (IsVerified)
            {
                IsVerified = false;
                AddDomainEvent(new IdentificationRejectedEvent(this));
            }
        }
        
        public bool IsExpired()
        {
            return ExpiryDate < DateTime.UtcNow.Date;
        }
        
        public bool IsExpiringSoon(int daysThreshold = 90)
        {
            var daysToExpiry = (ExpiryDate - DateTime.UtcNow.Date).TotalDays;
            return daysToExpiry > 0 && daysToExpiry <= daysThreshold;
        }
        
        private static void ValidateIdentification(
            string type,
            string number,
            string issuingCountry,
            DateTime issueDate,
            DateTime expiryDate)
        {
            ValidateType(type);
            ValidateNumber(number);
            ValidateIssuingCountry(issuingCountry);
            ValidateIssueDate(issueDate);
            
            if (expiryDate < issueDate)
                throw new DomainException("Expiry date cannot be before issue date");
        }
        
        private static void ValidateType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new DomainException("Identification type is required");
                
            if (type.Length > 50)
                throw new DomainException("Identification type cannot be longer than 50 characters");
        }
        
        private static void ValidateNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new DomainException("Identification number is required");
                
            if (number.Length > 50)
                throw new DomainException("Identification number cannot be longer than 50 characters");
        }
        
        private static void ValidateIssuingCountry(string issuingCountry)
        {
            if (string.IsNullOrWhiteSpace(issuingCountry))
                throw new DomainException("Issuing country is required");
                
            if (issuingCountry.Length > 50)
                throw new DomainException("Issuing country cannot be longer than 50 characters");
        }
        
        private static void ValidateIssueDate(DateTime issueDate)
        {
            if (issueDate > DateTime.UtcNow)
                throw new DomainException("Issue date cannot be in the future");
        }
    }
} 