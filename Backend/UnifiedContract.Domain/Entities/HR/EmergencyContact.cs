using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.ValueObjects;
using UnifiedContract.Domain.Events.HR;

namespace UnifiedContract.Domain.Entities.HR
{
    public class EmergencyContact : BaseEntity
    {
        public string Name { get; private set; }
        public string Relationship { get; private set; }
        public string PrimaryPhone { get; private set; }
        public string SecondaryPhone { get; private set; }
        public string Email { get; private set; }
        public Address Address { get; private set; }
        public bool IsPrimaryContact { get; private set; }
        public Guid EmployeeId { get; private set; }
        
        // Navigation properties
        public virtual Employee Employee { get; set; }
        
        // For EF Core
        protected EmergencyContact() { }
        
        public EmergencyContact(
            Guid employeeId,
            string name,
            string relationship,
            string primaryPhone,
            bool isPrimaryContact = false,
            string secondaryPhone = null,
            string email = null,
            Address address = null)
        {
            ValidateEmergencyContact(name, relationship, primaryPhone);
            
            EmployeeId = employeeId;
            Name = name;
            Relationship = relationship;
            PrimaryPhone = primaryPhone;
            SecondaryPhone = secondaryPhone;
            Email = email;
            Address = address;
            IsPrimaryContact = isPrimaryContact;
            
            AddDomainEvent(new EmergencyContactAddedEvent(this));
        }
        
        public void UpdateContactDetails(
            string name = null,
            string relationship = null,
            string primaryPhone = null,
            string secondaryPhone = null,
            string email = null,
            Address address = null)
        {
            if (name != null)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new DomainException("Name is required");
                    
                if (name.Length > 100)
                    throw new DomainException("Name cannot be longer than 100 characters");
                    
                Name = name;
            }
            
            if (relationship != null)
            {
                if (string.IsNullOrWhiteSpace(relationship))
                    throw new DomainException("Relationship is required");
                    
                if (relationship.Length > 50)
                    throw new DomainException("Relationship cannot be longer than 50 characters");
                    
                Relationship = relationship;
            }
            
            if (primaryPhone != null)
            {
                if (string.IsNullOrWhiteSpace(primaryPhone))
                    throw new DomainException("Primary phone is required");
                    
                if (primaryPhone.Length > 20)
                    throw new DomainException("Primary phone cannot be longer than 20 characters");
                    
                PrimaryPhone = primaryPhone;
            }
            
            if (secondaryPhone != null)
                SecondaryPhone = secondaryPhone;
                
            if (email != null)
                Email = email;
                
            if (address != null)
                Address = address;
                
            AddDomainEvent(new EmergencyContactUpdatedEvent(this));
        }
        
        public void SetAsPrimaryContact()
        {
            if (!IsPrimaryContact)
            {
                IsPrimaryContact = true;
                AddDomainEvent(new EmergencyContactSetAsPrimaryEvent(this));
            }
        }
        
        public void UnsetAsPrimaryContact()
        {
            if (IsPrimaryContact)
            {
                IsPrimaryContact = false;
                AddDomainEvent(new EmergencyContactUnsetAsPrimaryEvent(this));
            }
        }
        
        private static void ValidateEmergencyContact(string name, string relationship, string primaryPhone)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name is required");
                
            if (name.Length > 100)
                throw new DomainException("Name cannot be longer than 100 characters");
                
            if (string.IsNullOrWhiteSpace(relationship))
                throw new DomainException("Relationship is required");
                
            if (relationship.Length > 50)
                throw new DomainException("Relationship cannot be longer than 50 characters");
                
            if (string.IsNullOrWhiteSpace(primaryPhone))
                throw new DomainException("Primary phone is required");
                
            if (primaryPhone.Length > 20)
                throw new DomainException("Primary phone cannot be longer than 20 characters");
        }
    }
} 