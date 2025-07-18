using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Enums;
using UnifiedContract.Domain.Events.Resource;
using UnifiedContract.Domain.Exceptions;

namespace UnifiedContract.Domain.Entities.Resource
{
    public class ReceivableMaterial : BaseEntity
    {
        private string? _name;
        private string _description;
        private string _unit;
        private decimal _estimatedQuantity;
        private decimal? _receivedQuantity;
        private decimal? _actualQuantity;
        private decimal? _remainingQuantity;
        private decimal? _returnedQuantity;
        private string _sourceLocation;
        private string _notes;
        
        // Status property is exposed as public because it's an enum
        public MaterialStatus Status { get; private set; }
        
        // Foreign keys
        public DateTime? ReceivedDate { get; private set; }
        public DateTime? ReturnedDate { get; private set; }
        public Guid? ReceivedById { get; private set; }
        public Guid? ReturnedById { get; private set; }
        public Guid? WorkOrderId { get; private set; }
        public Guid? MaterialTypeId { get; private set; }
        public Guid? ClientMaterialId { get; private set; }
        
        // Navigation properties
        public virtual HR.Employee ReceivedBy { get; private set; }
        public virtual HR.Employee ReturnedBy { get; private set; }
        public virtual WorkOrder.WorkOrder WorkOrder { get; private set; }
        public virtual Lookups.MaterialType MaterialType { get; private set; }
        public virtual ClientMaterial ClientMaterial { get; private set; }
        
        // Public properties with private setters
        public string Name => _name;
        public string Description => _description;
        public string Unit => _unit;
        public decimal EstimatedQuantity => _estimatedQuantity;
        public decimal? ReceivedQuantity => _receivedQuantity;
        public decimal? ActualQuantity => _actualQuantity;
        public decimal? RemainingQuantity => _remainingQuantity;
        public decimal? ReturnedQuantity => _returnedQuantity;
        public string SourceLocation => _sourceLocation;
        public string Notes => _notes;
        
        // Required by EF Core
        private ReceivableMaterial() { }
        
        public ReceivableMaterial(
            string name,
            string description,
            decimal estimatedQuantity,
            string unit,
            Guid? workOrderId = null,
            Guid? materialTypeId = null,
            Guid? clientMaterialId = null,
            string sourceLocation = null,
            string notes = null)
        {
            ValidateMaterial(name, description, estimatedQuantity);
            
            _name = name;
            _description = description;
            _estimatedQuantity = estimatedQuantity;
            _unit = unit ?? "Units";
            WorkOrderId = workOrderId;
            MaterialTypeId = materialTypeId;
            ClientMaterialId = clientMaterialId;
            _sourceLocation = sourceLocation;
            _notes = notes;
            
            Status = MaterialStatus.Pending;
            
            AddDomainEvent(new ReceivableMaterialCreatedEvent(this));
        }
        
        // Method to create ReceivableMaterial from ClientMaterial
        public static ReceivableMaterial FromClientMaterial(
            ClientMaterial clientMaterial,
            decimal estimatedQuantity,
            Guid? workOrderId = null,
            string sourceLocation = null,
            string notes = null)
        {
            return new ReceivableMaterial(
                clientMaterial.MaterialMasterCode,
                clientMaterial.Description,
                estimatedQuantity,
                clientMaterial.Unit,
                workOrderId,
                null, // MaterialTypeId
                clientMaterial.Id,
                sourceLocation,
                notes);
        }
        
        public void UpdateDetails(
            string name = null,
            string description = null,
            decimal? estimatedQuantity = null,
            string unit = null,
            string sourceLocation = null,
            string notes = null)
        {
            if (Status != MaterialStatus.Pending)
                throw new BusinessRuleException("Cannot update details after material has been processed");
                
            bool changed = false;
            
            if (name != null && name != _name)
            {
                ValidateName(name);
                _name = name;
                changed = true;
            }
            
            if (description != null && description != _description)
            {
                _description = description;
                changed = true;
            }
            
            if (estimatedQuantity.HasValue && estimatedQuantity.Value != _estimatedQuantity)
            {
                ValidateQuantity(estimatedQuantity.Value);
                _estimatedQuantity = estimatedQuantity.Value;
                changed = true;
            }
            
            if (unit != null && unit != _unit)
            {
                _unit = unit;
                changed = true;
            }
            
            if (sourceLocation != null && sourceLocation != _sourceLocation)
            {
                _sourceLocation = sourceLocation;
                changed = true;
            }
            
            if (notes != null && notes != _notes)
            {
                _notes = notes;
                changed = true;
            }
            
            if (changed)
            {
                AddDomainEvent(new ReceivableMaterialUpdatedEvent(this));
            }
        }
        
        public void ReceiveMaterial(decimal receivedQuantity, Guid receivedById, DateTime receivedDate)
        {
            if (Status != MaterialStatus.Pending)
                throw new BusinessRuleException("Material must be in Pending status to be received");
                
            if (receivedDate > DateTime.UtcNow)
                throw new BusinessRuleException("Received date cannot be in the future");
                
            ValidateQuantity(receivedQuantity);
            
            _receivedQuantity = receivedQuantity;
            _actualQuantity = receivedQuantity;
            _remainingQuantity = receivedQuantity;
            ReceivedById = receivedById;
            ReceivedDate = receivedDate;
            
            Status = MaterialStatus.Received;
            
            AddDomainEvent(new ReceivableMaterialReceivedEvent(this));
        }
        
        public void UseMaterial(decimal usedQuantity)
        {
            if (Status != MaterialStatus.Received)
                throw new BusinessRuleException("Material must be in Received status to be used");
                
            if (!_remainingQuantity.HasValue || usedQuantity > _remainingQuantity.Value)
                throw new BusinessRuleException($"Cannot use more than the remaining quantity ({_remainingQuantity})");
                
            ValidateQuantity(usedQuantity);
            
            _remainingQuantity = _remainingQuantity.Value - usedQuantity;
            
            if (_remainingQuantity.Value == 0)
            {
                Status = MaterialStatus.Used;
            }
            
            AddDomainEvent(new ReceivableMaterialStatusChangedEvent(this));
        }
        
        public void ReturnMaterial(decimal returnedQuantity, Guid returnedById, DateTime returnedDate)
        {
            if (Status == MaterialStatus.Pending)
                throw new BusinessRuleException("Material must be received before it can be returned");
                
            if (Status == MaterialStatus.Returned)
                throw new BusinessRuleException("Material has already been fully returned");
                
            if (returnedDate > DateTime.UtcNow)
                throw new BusinessRuleException("Returned date cannot be in the future");
                
            if (returnedDate < ReceivedDate)
                throw new BusinessRuleException("Returned date cannot be before received date");
                
            ValidateQuantity(returnedQuantity);
            
            if (!_remainingQuantity.HasValue || returnedQuantity > _remainingQuantity.Value)
                throw new BusinessRuleException($"Cannot return more than the remaining quantity ({_remainingQuantity})");
                
            _returnedQuantity = (_returnedQuantity ?? 0) + returnedQuantity;
            _remainingQuantity = _remainingQuantity.Value - returnedQuantity;
            ReturnedById = returnedById;
            ReturnedDate = returnedDate;
            
            if (_remainingQuantity.Value == 0)
            {
                Status = MaterialStatus.Returned;
            }
            
            AddDomainEvent(new ReceivableMaterialReturnedEvent(this));
        }
        
        public void ChangeStatus(MaterialStatus status)
        {
            if (status == Status)
                return;
                
            // Validate state transitions
            if (status == MaterialStatus.Received && Status != MaterialStatus.Pending)
                throw new BusinessRuleException("Can only receive materials that are pending");
                
            if (status == MaterialStatus.Used && Status != MaterialStatus.Received)
                throw new BusinessRuleException("Can only use materials that are received");
                
            if (status == MaterialStatus.Returned && Status != MaterialStatus.Received && Status != MaterialStatus.Used)
                throw new BusinessRuleException("Can only return materials that are received or partially used");
                
            Status = status;
            AddDomainEvent(new ReceivableMaterialStatusChangedEvent(this));
        }
        
        #region Validation Methods
        
        private void ValidateMaterial(string name, string description, decimal estimatedQuantity)
        {
            ValidateName(name);
            ValidateDescription(description);
            ValidateQuantity(estimatedQuantity);
        }
        
        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BusinessRuleException("Material name is required");
                
            if (name.Length > 100)
                throw new BusinessRuleException("Material name cannot exceed 100 characters");
        }
        
        private void ValidateDescription(string description)
        {
            if (description != null && description.Length > 500)
                throw new BusinessRuleException("Material description cannot exceed 500 characters");
        }
        
        private void ValidateQuantity(decimal quantity)
        {
            if (quantity <= 0)
                throw new BusinessRuleException("Quantity must be greater than zero");
        }
        
        #endregion
    }
} 