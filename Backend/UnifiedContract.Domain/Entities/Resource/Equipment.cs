using System;
using System.Collections.Generic;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Domain.Entities.Resource.Lookups;
using UnifiedContract.Domain.Events.Resource;
using UnifiedContract.Domain.Exceptions;

namespace UnifiedContract.Domain.Entities.Resource
{
    public class Equipment : BaseEntity
    {
        private string _companyNumber = string.Empty;
        private string _name = string.Empty;
        private string _type = string.Empty;
        private string _model = string.Empty;
        private string _serialNumber = string.Empty;
        private string _manufacturer = string.Empty;
        private decimal _dailyCost;
        private string _currency = "SAR";
        private DateTime _purchaseDate;
        private DateTime? _lastMaintenanceDate;
        private DateTime? _nextMaintenanceDate;
        private string _currentLocation = string.Empty;
        private string _description = string.Empty;
        private string _imageUrl = string.Empty;
        private readonly List<EquipmentAssignment> _assignments = new List<EquipmentAssignment>();
        private readonly List<EquipmentMaintenance> _maintenanceRecords = new List<EquipmentMaintenance>();
        
        // Foreign keys
        public Guid EquipmentStatusId { get; private set; }
        public Guid? CurrentOperatorId { get; private set; }
        public Guid? SupplierId { get; private set; }
        
        // Navigation properties
        public virtual EquipmentStatus Status { get; private set; }
        public virtual Employee? CurrentOperator { get; private set; }
        public virtual Supplier? Supplier { get; private set; }
        
        // Public properties with private setters
        public string CompanyNumber => _companyNumber;
        public string Name => _name;
        public string Type => _type;
        public string Model => _model;
        public string SerialNumber => _serialNumber;
        public string Manufacturer => _manufacturer;
        public decimal DailyCost => _dailyCost;
        public string Currency => _currency;
        public DateTime PurchaseDate => _purchaseDate;
        public DateTime? LastMaintenanceDate => _lastMaintenanceDate;
        public DateTime? NextMaintenanceDate => _nextMaintenanceDate;
        public string CurrentLocation => _currentLocation;
        public string Description => _description;
        public string ImageUrl => _imageUrl;
        public IReadOnlyCollection<EquipmentAssignment> Assignments => _assignments.AsReadOnly();
        public IReadOnlyCollection<EquipmentMaintenance> MaintenanceRecords => _maintenanceRecords.AsReadOnly();
        
        // Required by EF Core
        private Equipment() { }
        
        public Equipment(
            string companyNumber,
            string name,
            string type,
            string model,
            string serialNumber,
            DateTime purchaseDate,
            Guid equipmentStatusId,
            decimal dailyCost = 0,
            string currency = "SAR",
            string manufacturer = "",
            string currentLocation = "",
            string description = "",
            string imageUrl = "",
            Guid? currentOperatorId = null,
            Guid? supplierId = null)
        {
            ValidateEquipment(companyNumber, name, type, model, serialNumber, dailyCost);
            
            _companyNumber = companyNumber;
            _name = name;
            _type = type;
            _model = model;
            _serialNumber = serialNumber;
            _manufacturer = manufacturer;
            _dailyCost = dailyCost;
            _currency = currency;
            _purchaseDate = purchaseDate;
            _currentLocation = currentLocation;
            _description = description;
            _imageUrl = imageUrl;
            EquipmentStatusId = equipmentStatusId;
            CurrentOperatorId = currentOperatorId;
            SupplierId = supplierId;
            
            AddDomainEvent(new EquipmentCreatedEvent(this));
        }
        
        public void UpdateDetails(
            string? name = null,
            string? type = null,
            string? model = null,
            string? manufacturer = null,
            decimal? dailyCost = null,
            string? currency = null,
            string? currentLocation = null,
            string? description = null,
            string? imageUrl = null)
        {
            bool changed = false;
            
            if (name != null && name != _name)
            {
                ValidateName(name);
                _name = name;
                changed = true;
            }
            
            if (type != null && type != _type)
            {
                ValidateType(type);
                _type = type;
                changed = true;
            }
            
            if (model != null && model != _model)
            {
                ValidateModel(model);
                _model = model;
                changed = true;
            }
            
            if (manufacturer != null && manufacturer != _manufacturer)
            {
                _manufacturer = manufacturer;
                changed = true;
            }
            
            if (dailyCost.HasValue && dailyCost.Value != _dailyCost)
            {
                ValidateDailyCost(dailyCost.Value);
                _dailyCost = dailyCost.Value;
                changed = true;
            }
            
            if (currency != null && currency != _currency)
            {
                _currency = currency;
                changed = true;
            }
            
            if (currentLocation != null && currentLocation != _currentLocation)
            {
                _currentLocation = currentLocation;
                changed = true;
            }
            
            if (description != null && description != _description)
            {
                _description = description;
                changed = true;
            }
            
            if (imageUrl != null && imageUrl != _imageUrl)
            {
                _imageUrl = imageUrl;
                changed = true;
            }
            
            if (changed)
            {
                AddDomainEvent(new EquipmentUpdatedEvent(this));
            }
        }
        
        public void ChangeStatus(Guid newStatusId)
        {
            if (newStatusId == Guid.Empty)
                throw new BusinessRuleException("Equipment status ID cannot be empty");
                
            if (newStatusId != EquipmentStatusId)
            {
                EquipmentStatusId = newStatusId;
                AddDomainEvent(new EquipmentStatusChangedEvent(this));
            }
        }
        
        public void AssignOperator(Guid? operatorId)
        {
            if (operatorId != CurrentOperatorId)
            {
                CurrentOperatorId = operatorId;
                AddDomainEvent(new EquipmentOperatorChangedEvent(this));
            }
        }
        
        public void ChangeSupplier(Guid? supplierId)
        {
            if (supplierId != SupplierId)
            {
                SupplierId = supplierId;
                AddDomainEvent(new EquipmentSupplierChangedEvent(this));
            }
        }
        
        public void RecordMaintenance(DateTime maintenanceDate)
        {
            _lastMaintenanceDate = maintenanceDate;
            AddDomainEvent(new EquipmentMaintenanceRecordedEvent(this));
        }
        
        public void ScheduleNextMaintenance(DateTime nextMaintenanceDate)
        {
            if (nextMaintenanceDate <= DateTime.UtcNow)
                throw new BusinessRuleException("Next maintenance date must be in the future");
                
            _nextMaintenanceDate = nextMaintenanceDate;
            AddDomainEvent(new EquipmentMaintenanceScheduledEvent(this));
        }
        
        public void AddAssignment(EquipmentAssignment assignment)
        {
            if (!_assignments.Contains(assignment))
            {
                _assignments.Add(assignment);
            }
        }
        
        public void AddMaintenanceRecord(EquipmentMaintenance maintenance)
        {
            if (!_maintenanceRecords.Contains(maintenance))
            {
                _maintenanceRecords.Add(maintenance);
                _lastMaintenanceDate = maintenance.CompletedDate ?? maintenance.ScheduledDate;
                
                // Optionally schedule next maintenance based on maintenance type
                if (maintenance.CompletedDate.HasValue && maintenance.Type != null && maintenance.Type.TypicalIntervalDays.HasValue && maintenance.Type.TypicalIntervalDays.Value > 0)
                {
                    _nextMaintenanceDate = maintenance.CompletedDate.Value.AddDays((double)maintenance.Type.TypicalIntervalDays.Value);
                }
            }
        }
        
        #region Validation Methods
        
        private void ValidateEquipment(string companyNumber, string name, string type, string model, string serialNumber, decimal dailyCost)
        {
            ValidateCompanyNumber(companyNumber);
            ValidateName(name);
            ValidateType(type);
            ValidateModel(model);
            ValidateSerialNumber(serialNumber);
            ValidateDailyCost(dailyCost);
        }
        
        private void ValidateCompanyNumber(string companyNumber)
        {
            if (string.IsNullOrWhiteSpace(companyNumber))
                throw new BusinessRuleException("Company number is required");
                
            if (companyNumber.Length > 50)
                throw new BusinessRuleException("Company number cannot exceed 50 characters");
        }
        
        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BusinessRuleException("Equipment name is required");
                
            if (name.Length > 100)
                throw new BusinessRuleException("Equipment name cannot exceed 100 characters");
        }
        
        private void ValidateType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new BusinessRuleException("Equipment type is required");
                
            if (type.Length > 50)
                throw new BusinessRuleException("Equipment type cannot exceed 50 characters");
        }
        
        private void ValidateModel(string model)
        {
            if (string.IsNullOrWhiteSpace(model))
                throw new BusinessRuleException("Equipment model is required");
                
            if (model.Length > 100)
                throw new BusinessRuleException("Equipment model cannot exceed 100 characters");
        }
        
        private void ValidateSerialNumber(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                throw new BusinessRuleException("Serial number is required");
                
            if (serialNumber.Length > 50)
                throw new BusinessRuleException("Serial number cannot exceed 50 characters");
        }
        
        private void ValidateDailyCost(decimal dailyCost)
        {
            if (dailyCost < 0)
                throw new BusinessRuleException("Daily cost cannot be negative");
        }
        
        #endregion
    }
} 