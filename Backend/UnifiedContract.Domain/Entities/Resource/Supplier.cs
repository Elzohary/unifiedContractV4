using System;
using System.Collections.Generic;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.Resource.Lookups;
using UnifiedContract.Domain.Events.Resource;
using UnifiedContract.Domain.Exceptions;
using UnifiedContract.Domain.ValueObjects;

namespace UnifiedContract.Domain.Entities.Resource
{
    public class Supplier : BaseEntity
    {
        private string _name = string.Empty;
        private string _contactPerson = string.Empty;
        private string _email = string.Empty;
        private string _phone = string.Empty;
        private string _alternatePhone = string.Empty;
        private Address _address;
        private string _website = string.Empty;
        private string _vatNumber = string.Empty;
        private string _paymentTerms = string.Empty;
        private string _bankAccount = string.Empty;
        private bool _isActive = true;
        private string _notes = string.Empty;
        private decimal _rating;
        private readonly HashSet<PurchasableMaterial> _materials = new HashSet<PurchasableMaterial>();
        private readonly HashSet<Equipment> _equipment = new HashSet<Equipment>();
        
        // Foreign keys
        public Guid? CategoryId { get; private set; }
        
        // Navigation properties
        public virtual SupplierCategory Category { get; private set; }
        
        // Public properties with private setters
        public string Name => _name;
        public string ContactPerson => _contactPerson;
        public string Email => _email;
        public string Phone => _phone;
        public string AlternatePhone => _alternatePhone;
        public Address Address => _address;
        public string Website => _website;
        public string VatNumber => _vatNumber;
        public string PaymentTerms => _paymentTerms;
        public string BankAccount => _bankAccount;
        public bool IsActive => _isActive;
        public string Notes => _notes;
        public decimal Rating => _rating;
        public IReadOnlyCollection<PurchasableMaterial> Materials => _materials;
        public IReadOnlyCollection<Equipment> Equipment => _equipment;
        
        // Required by EF Core
        private Supplier() { }
        
        public Supplier(
            string name,
            string contactPerson,
            string email,
            string phone,
            Address address,
            Guid? categoryId = null,
            string? alternatePhone = null,
            string? website = null,
            string? vatNumber = null,
            string? paymentTerms = null,
            string? bankAccount = null,
            string? notes = null)
        {
            ValidateSupplier(name, email, phone);
            
            _name = name;
            _contactPerson = contactPerson;
            _email = email;
            _phone = phone;
            _address = address;
            _alternatePhone = alternatePhone ?? string.Empty;
            _website = website ?? string.Empty;
            _vatNumber = vatNumber ?? string.Empty;
            _paymentTerms = paymentTerms ?? string.Empty;
            _bankAccount = bankAccount ?? string.Empty;
            _notes = notes ?? string.Empty;
            _rating = 0;
            CategoryId = categoryId;
            
            AddDomainEvent(new SupplierCreatedEvent(this));
        }
        
        public void UpdateDetails(
            string? name = null,
            string? contactPerson = null,
            string? email = null,
            string? phone = null,
            Address? address = null,
            string? alternatePhone = null,
            string? website = null,
            string? vatNumber = null,
            string? paymentTerms = null,
            string? bankAccount = null,
            string? notes = null)
        {
            bool changed = false;
            
            if (name != null && name != _name)
            {
                ValidateName(name);
                _name = name;
                changed = true;
            }
            
            if (contactPerson != null && contactPerson != _contactPerson)
            {
                _contactPerson = contactPerson;
                changed = true;
            }
            
            if (email != null && email != _email)
            {
                ValidateEmail(email);
                _email = email;
                changed = true;
            }
            
            if (phone != null && phone != _phone)
            {
                ValidatePhone(phone);
                _phone = phone;
                changed = true;
            }
            
            if (address != null && !address.Equals(_address))
            {
                _address = address;
                changed = true;
            }
            
            if (alternatePhone != null && alternatePhone != _alternatePhone)
            {
                _alternatePhone = alternatePhone;
                changed = true;
            }
            
            if (website != null && website != _website)
            {
                _website = website;
                changed = true;
            }
            
            if (vatNumber != null && vatNumber != _vatNumber)
            {
                _vatNumber = vatNumber;
                changed = true;
            }
            
            if (paymentTerms != null && paymentTerms != _paymentTerms)
            {
                _paymentTerms = paymentTerms;
                changed = true;
            }
            
            if (bankAccount != null && bankAccount != _bankAccount)
            {
                _bankAccount = bankAccount;
                changed = true;
            }
            
            if (notes != null && notes != _notes)
            {
                _notes = notes;
                changed = true;
            }
            
            if (changed)
            {
                AddDomainEvent(new SupplierUpdatedEvent(this));
            }
        }
        
        public void ChangeCategory(Guid? categoryId)
        {
            if (categoryId != CategoryId)
            {
                CategoryId = categoryId;
                AddDomainEvent(new SupplierCategoryChangedEvent(this));
            }
        }
        
        public void SetActiveStatus(bool active)
        {
            if (active != _isActive)
            {
                _isActive = active;
                AddDomainEvent(new SupplierStatusChangedEvent(this, active));
            }
        }
        
        public void UpdateRating(decimal rating)
        {
            if (rating < 0 || rating > 5)
                throw new BusinessRuleException("Rating must be between 0 and 5");
                
            if (rating != _rating)
            {
                _rating = rating;
                AddDomainEvent(new SupplierRatingUpdatedEvent(this));
            }
        }
        
        public void AddMaterial(PurchasableMaterial material)
        {
            if (material != null && !_materials.Contains(material))
            {
                _materials.Add(material);
            }
        }
        
        public void RemoveMaterial(PurchasableMaterial material)
        {
            if (material != null && _materials.Contains(material))
            {
                _materials.Remove(material);
            }
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
        
        #region Validation Methods
        
        private void ValidateSupplier(string name, string email, string phone)
        {
            ValidateName(name);
            ValidateEmail(email);
            ValidatePhone(phone);
        }
        
        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BusinessRuleException("Supplier name cannot be empty");
                
            if (name.Length > 100)
                throw new BusinessRuleException("Supplier name cannot exceed 100 characters");
        }
        
        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new BusinessRuleException("Supplier email cannot be empty");
                
            if (email.Length > 100)
                throw new BusinessRuleException("Supplier email cannot exceed 100 characters");
                
            // Simple email validation
            if (!email.Contains("@") || !email.Contains("."))
                throw new BusinessRuleException("Supplier email is not valid");
        }
        
        private void ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new BusinessRuleException("Supplier phone cannot be empty");
                
            if (phone.Length > 20)
                throw new BusinessRuleException("Supplier phone cannot exceed 20 characters");
        }
        
        #endregion
    }
} 