using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Events.HR;

namespace UnifiedContract.Domain.Entities.HR
{
    public class WorkExperience : BaseEntity
    {
        public string CompanyName { get; private set; }
        public string Position { get; private set; }
        public string JobDescription { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public string Location { get; private set; }
        public bool IsCurrentEmployer { get; private set; }
        public string ContactReference { get; private set; }
        public string ContactReferencePhone { get; private set; }
        public string ContactReferenceEmail { get; private set; }
        public string Achievements { get; private set; }
        public string ReasonForLeaving { get; private set; }
        
        // Foreign Key
        public Guid EmployeeId { get; private set; }
        
        // Navigation Property
        public virtual Employee Employee { get; set; }
        
        // For EF Core
        protected WorkExperience() { }
        
        public WorkExperience(
            Guid employeeId,
            string companyName,
            string position,
            DateTime startDate,
            string jobDescription = null,
            DateTime? endDate = null,
            string location = null,
            bool isCurrentEmployer = false)
        {
            ValidateWorkExperience(companyName, position, startDate, endDate, isCurrentEmployer);
            
            EmployeeId = employeeId;
            CompanyName = companyName;
            Position = position;
            StartDate = startDate;
            JobDescription = jobDescription;
            EndDate = endDate;
            Location = location;
            IsCurrentEmployer = isCurrentEmployer;
            
            if (isCurrentEmployer)
                EndDate = null;
                
            AddDomainEvent(new WorkExperienceAddedEvent(this));
        }
        
        public void UpdateDetails(
            string companyName = null,
            string position = null,
            string jobDescription = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string location = null,
            bool? isCurrentEmployer = null,
            string contactReference = null,
            string contactReferencePhone = null,
            string contactReferenceEmail = null,
            string achievements = null,
            string reasonForLeaving = null)
        {
            bool changed = false;
            
            // Handle current employer setting first as it affects end date
            if (isCurrentEmployer.HasValue && isCurrentEmployer.Value != IsCurrentEmployer)
            {
                IsCurrentEmployer = isCurrentEmployer.Value;
                
                // Clear end date if now current employer
                if (IsCurrentEmployer)
                    EndDate = null;
                    
                changed = true;
            }
            
            if (companyName != null && companyName != CompanyName)
            {
                ValidateCompanyName(companyName);
                CompanyName = companyName;
                changed = true;
            }
            
            if (position != null && position != Position)
            {
                ValidatePosition(position);
                Position = position;
                changed = true;
            }
            
            if (jobDescription != null && jobDescription != JobDescription)
            {
                if (jobDescription.Length > 1000)
                    throw new DomainException("Job description cannot be longer than 1000 characters");
                    
                JobDescription = jobDescription;
                changed = true;
            }
            
            if (startDate.HasValue && startDate != StartDate)
            {
                ValidateStartDate(startDate.Value);
                
                if (EndDate.HasValue && startDate > EndDate)
                    throw new DomainException("Start date cannot be after end date");
                    
                StartDate = startDate.Value;
                changed = true;
            }
            
            if (!IsCurrentEmployer && endDate.HasValue && endDate != EndDate)
            {
                if (endDate < StartDate)
                    throw new DomainException("End date cannot be before start date");
                    
                EndDate = endDate;
                changed = true;
            }
            
            if (location != null && location != Location)
            {
                if (location.Length > 100)
                    throw new DomainException("Location cannot be longer than 100 characters");
                    
                Location = location;
                changed = true;
            }
            
            if (contactReference != null && contactReference != ContactReference)
            {
                if (contactReference.Length > 100)
                    throw new DomainException("Contact reference cannot be longer than 100 characters");
                    
                ContactReference = contactReference;
                changed = true;
            }
            
            if (contactReferencePhone != null && contactReferencePhone != ContactReferencePhone)
            {
                if (contactReferencePhone.Length > 20)
                    throw new DomainException("Contact reference phone cannot be longer than 20 characters");
                    
                ContactReferencePhone = contactReferencePhone;
                changed = true;
            }
            
            if (contactReferenceEmail != null && contactReferenceEmail != ContactReferenceEmail)
            {
                if (contactReferenceEmail.Length > 100)
                    throw new DomainException("Contact reference email cannot be longer than 100 characters");
                    
                ContactReferenceEmail = contactReferenceEmail;
                changed = true;
            }
            
            if (achievements != null && achievements != Achievements)
            {
                if (achievements.Length > 1000)
                    throw new DomainException("Achievements cannot be longer than 1000 characters");
                    
                Achievements = achievements;
                changed = true;
            }
            
            if (reasonForLeaving != null && reasonForLeaving != ReasonForLeaving)
            {
                if (reasonForLeaving.Length > 500)
                    throw new DomainException("Reason for leaving cannot be longer than 500 characters");
                    
                ReasonForLeaving = reasonForLeaving;
                changed = true;
            }
            
            if (changed)
            {
                AddDomainEvent(new WorkExperienceUpdatedEvent(this));
            }
        }
        
        public void MarkAsCurrentEmployer()
        {
            if (!IsCurrentEmployer)
            {
                IsCurrentEmployer = true;
                EndDate = null;
                AddDomainEvent(new WorkExperienceSetAsCurrentEvent(this));
            }
        }
        
        public void MarkAsFormerEmployer(DateTime endDate)
        {
            if (endDate < StartDate)
                throw new DomainException("End date cannot be before start date");
                
            if (endDate > DateTime.UtcNow)
                throw new DomainException("End date cannot be in the future");
                
            if (IsCurrentEmployer || EndDate == null)
            {
                IsCurrentEmployer = false;
                EndDate = endDate;
                AddDomainEvent(new WorkExperienceSetAsFormerEvent(this));
            }
        }
        
        private static void ValidateWorkExperience(
            string companyName,
            string position,
            DateTime startDate,
            DateTime? endDate,
            bool isCurrentEmployer)
        {
            ValidateCompanyName(companyName);
            ValidatePosition(position);
            ValidateStartDate(startDate);
            
            if (!isCurrentEmployer && !endDate.HasValue)
                throw new DomainException("End date is required for non-current employers");
            
            if (!isCurrentEmployer && endDate.HasValue && endDate < startDate)
                throw new DomainException("End date cannot be before start date");
        }
        
        private static void ValidateCompanyName(string companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName))
                throw new DomainException("Company name is required");
                
            if (companyName.Length > 100)
                throw new DomainException("Company name cannot be longer than 100 characters");
        }
        
        private static void ValidatePosition(string position)
        {
            if (string.IsNullOrWhiteSpace(position))
                throw new DomainException("Position is required");
                
            if (position.Length > 100)
                throw new DomainException("Position cannot be longer than 100 characters");
        }
        
        private static void ValidateStartDate(DateTime startDate)
        {
            if (startDate > DateTime.UtcNow)
                throw new DomainException("Start date cannot be in the future");
        }
    }
} 