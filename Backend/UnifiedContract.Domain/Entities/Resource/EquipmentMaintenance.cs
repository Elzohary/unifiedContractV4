using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Domain.Entities.Resource.Lookups;
using UnifiedContract.Domain.Events.Resource;
using UnifiedContract.Domain.Exceptions;

namespace UnifiedContract.Domain.Entities.Resource
{
    public class EquipmentMaintenance : BaseEntity
    {
        private string _description;
        private DateTime _scheduledDate;
        private DateTime? _completedDate;
        private decimal _cost;
        private string _currency = "SAR";
        private string _notes;
        private string _serviceProvider;
        private string _invoiceNumber;
        private string _documentUrl;
        
        // Foreign keys
        public Guid MaintenanceTypeId { get; private set; }
        public Guid MaintenanceStatusId { get; private set; }
        public Guid EquipmentId { get; private set; }
        public Guid? PerformedById { get; private set; }
        
        // Navigation properties
        public virtual MaintenanceType Type { get; private set; }
        public virtual MaintenanceStatus Status { get; private set; }
        public virtual Equipment Equipment { get; private set; }
        public virtual Employee PerformedBy { get; private set; }
        
        // Public properties with private setters
        public string Description => _description;
        public DateTime ScheduledDate => _scheduledDate;
        public DateTime? CompletedDate => _completedDate;
        public decimal Cost => _cost;
        public string Currency => _currency;
        public string Notes => _notes;
        public string ServiceProvider => _serviceProvider;
        public string InvoiceNumber => _invoiceNumber;
        public string DocumentUrl => _documentUrl;
        
        // Required by EF Core
        private EquipmentMaintenance() { }
        
        public EquipmentMaintenance(
            string description,
            DateTime scheduledDate,
            Guid maintenanceTypeId,
            Guid maintenanceStatusId,
            Guid equipmentId,
            decimal cost = 0,
            string currency = "SAR",
            Guid? performedById = null,
            string serviceProvider = null,
            string notes = null,
            string invoiceNumber = null,
            string documentUrl = null)
        {
            ValidateMaintenance(description, scheduledDate, cost);
            
            _description = description;
            _scheduledDate = scheduledDate;
            _cost = cost;
            _currency = currency ?? "SAR";
            _notes = notes;
            _serviceProvider = serviceProvider;
            _invoiceNumber = invoiceNumber;
            _documentUrl = documentUrl;
            
            MaintenanceTypeId = maintenanceTypeId;
            MaintenanceStatusId = maintenanceStatusId;
            EquipmentId = equipmentId;
            PerformedById = performedById;
            
            AddDomainEvent(new MaintenanceCreatedEvent(this));
        }
        
        public void UpdateDetails(
            string description = null,
            DateTime? scheduledDate = null,
            decimal? cost = null,
            string currency = null,
            string notes = null,
            string serviceProvider = null,
            string invoiceNumber = null,
            string documentUrl = null)
        {
            bool changed = false;
            
            if (description != null && description != _description)
            {
                ValidateDescription(description);
                _description = description;
                changed = true;
            }
            
            if (scheduledDate.HasValue && scheduledDate != _scheduledDate)
            {
                _scheduledDate = scheduledDate.Value;
                changed = true;
            }
            
            if (cost.HasValue && cost != _cost)
            {
                ValidateCost(cost.Value);
                _cost = cost.Value;
                changed = true;
            }
            
            if (currency != null && currency != _currency)
            {
                _currency = currency;
                changed = true;
            }
            
            if (notes != null && notes != _notes)
            {
                _notes = notes;
                changed = true;
            }
            
            if (serviceProvider != null && serviceProvider != _serviceProvider)
            {
                _serviceProvider = serviceProvider;
                changed = true;
            }
            
            if (invoiceNumber != null && invoiceNumber != _invoiceNumber)
            {
                _invoiceNumber = invoiceNumber;
                changed = true;
            }
            
            if (documentUrl != null && documentUrl != _documentUrl)
            {
                _documentUrl = documentUrl;
                changed = true;
            }
            
            if (changed)
            {
                AddDomainEvent(new MaintenanceUpdatedEvent(this));
            }
        }
        
        public void ChangeStatus(Guid newStatusId)
        {
            if (newStatusId == Guid.Empty)
                throw new BusinessRuleException("Maintenance status ID cannot be empty");
                
            if (newStatusId != MaintenanceStatusId)
            {
                MaintenanceStatusId = newStatusId;
                AddDomainEvent(new MaintenanceStatusChangedEvent(this));
            }
        }
        
        public void ChangeType(Guid newTypeId)
        {
            if (newTypeId == Guid.Empty)
                throw new BusinessRuleException("Maintenance type ID cannot be empty");
                
            if (newTypeId != MaintenanceTypeId)
            {
                MaintenanceTypeId = newTypeId;
                AddDomainEvent(new MaintenanceTypeChangedEvent(this));
            }
        }
        
        public void AssignPerformer(Guid? performerId)
        {
            if (performerId != PerformedById)
            {
                PerformedById = performerId;
                AddDomainEvent(new MaintenancePerformerChangedEvent(this));
            }
        }
        
        public void MarkCompleted(DateTime completionDate)
        {
            if (completionDate > DateTime.UtcNow)
                throw new BusinessRuleException("Completion date cannot be in the future");
                
            _completedDate = completionDate;
            AddDomainEvent(new MaintenanceCompletedEvent(this));
        }
        
        #region Validation Methods
        
        private void ValidateMaintenance(string description, DateTime scheduledDate, decimal cost)
        {
            ValidateDescription(description);
            ValidateScheduledDate(scheduledDate);
            ValidateCost(cost);
        }
        
        private void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new BusinessRuleException("Maintenance description is required");
                
            if (description.Length > 500)
                throw new BusinessRuleException("Maintenance description cannot exceed 500 characters");
        }
        
        private void ValidateScheduledDate(DateTime scheduledDate)
        {
            if (scheduledDate < DateTime.UtcNow.AddYears(-1))
                throw new BusinessRuleException("Scheduled date cannot be more than one year in the past");
        }
        
        private void ValidateCost(decimal cost)
        {
            if (cost < 0)
                throw new BusinessRuleException("Cost cannot be negative");
        }
        
        #endregion
    }
} 