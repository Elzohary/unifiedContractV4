using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Events.HR;

namespace UnifiedContract.Domain.Entities.HR
{
    public class Certificate : BaseEntity
    {
        public string Name { get; private set; }
        public string Issuer { get; private set; }
        public DateTime IssueDate { get; private set; }
        public DateTime? ExpiryDate { get; private set; }
        public string DocumentUrl { get; set; }
        public bool Verified { get; private set; }
        
        // Foreign Key
        public Guid EmployeeId { get; private set; }
        
        // Navigation Property
        public virtual Employee Employee { get; set; }
        
        // For EF Core
        protected Certificate() { }
        
        public Certificate(Guid employeeId, string name, string issuer, DateTime issueDate, DateTime? expiryDate = null)
        {
            ValidateCertificate(name, issuer, issueDate, expiryDate);
            
            EmployeeId = employeeId;
            Name = name;
            Issuer = issuer;
            IssueDate = issueDate;
            ExpiryDate = expiryDate;
            Verified = false;
            
            AddDomainEvent(new CertificateAddedEvent(this));
        }
        
        public void UpdateDetails(string name, string issuer, DateTime issueDate, DateTime? expiryDate = null)
        {
            ValidateCertificate(name, issuer, issueDate, expiryDate);
            
            Name = name;
            Issuer = issuer;
            IssueDate = issueDate;
            ExpiryDate = expiryDate;
            
            // Reset verification status when certificate details are updated
            if (Verified)
            {
                Verified = false;
                AddDomainEvent(new CertificateUpdatedEvent(this));
            }
        }
        
        public void VerifyCertificate()
        {
            if (!Verified)
            {
                Verified = true;
                AddDomainEvent(new CertificateVerifiedEvent(this));
            }
        }
        
        public void RejectVerification()
        {
            if (Verified)
            {
                Verified = false;
                AddDomainEvent(new CertificateRejectedEvent(this));
            }
        }
        
        public bool IsExpired()
        {
            return ExpiryDate.HasValue && ExpiryDate.Value < DateTime.UtcNow.Date;
        }
        
        public bool IsExpiringSoon(int daysThreshold = 30)
        {
            if (!ExpiryDate.HasValue)
                return false;
                
            var daysToExpiry = (ExpiryDate.Value - DateTime.UtcNow.Date).TotalDays;
            return daysToExpiry > 0 && daysToExpiry <= daysThreshold;
        }
        
        private static void ValidateCertificate(string name, string issuer, DateTime issueDate, DateTime? expiryDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Certificate name is required");
                
            if (name.Length > 100)
                throw new DomainException("Certificate name cannot be longer than 100 characters");
                
            if (string.IsNullOrWhiteSpace(issuer))
                throw new DomainException("Certificate issuer is required");
                
            if (issuer.Length > 100)
                throw new DomainException("Certificate issuer cannot be longer than 100 characters");
                
            if (issueDate > DateTime.UtcNow)
                throw new DomainException("Issue date cannot be in the future");
                
            if (expiryDate.HasValue && expiryDate.Value < issueDate)
                throw new DomainException("Expiry date must be after issue date");
        }
    }
} 