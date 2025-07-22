using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Events.Resource;
using UnifiedContract.Domain.Exceptions;

namespace UnifiedContract.Domain.Entities.Resource
{
    public class PurchasableMaterial : BaseEntity
    {
        private string _name;
        private string _description;
        private decimal _quantity;
        private string _unit;
        private decimal? _unitCost;
        private decimal? _totalCost;
        private string _status;
        private string _supplier;
        private DateTime? _orderDate;
        private DateTime? _deliveryDate;
        
        // Foreign keys
        public Guid? SupplierId { get; private set; }
        public Guid? MaterialTypeId { get; private set; }
        
        // Navigation properties
        public virtual Supplier Supplier { get; private set; }
        public virtual Lookups.MaterialType MaterialType { get; private set; }
        
        // Public properties with private setters
        public string Name => _name;
        public string Description => _description;
        public decimal Quantity => _quantity;
        public string Unit => _unit;
        public decimal? UnitCost => _unitCost;
        public decimal? TotalCost => _totalCost;
        public string Status => _status;
        public string SupplierName { get => _supplier; private set => _supplier = value; }
        public DateTime? OrderDate => _orderDate;
        public DateTime? DeliveryDate => _deliveryDate;
        
        // Required by EF Core
        private PurchasableMaterial() { }
        
        public PurchasableMaterial(
            string name,
            string description,
            decimal quantity,
            string unit,
            decimal? unitCost = null,
            string status = "Pending",
            Guid? supplierId = null,
            string supplier = null,
            Guid? materialTypeId = null)
        {
            ValidateMaterial(name, description, quantity, unitCost);
            
            _name = name;
            _description = description;
            _quantity = quantity;
            _unit = unit ?? "Units";
            _unitCost = unitCost;
            _totalCost = unitCost.HasValue ? unitCost.Value * quantity : null;
            _status = status ?? "Pending";
            SupplierId = supplierId;
            _supplier = supplier;
            MaterialTypeId = materialTypeId;
            
            AddDomainEvent(new MaterialCreatedEvent(this));
        }
        
        public void UpdateDetails(
            string name = null,
            string description = null,
            decimal? quantity = null,
            string unit = null,
            decimal? unitCost = null,
            string supplier = null,
            Guid? supplierId = null)
        {
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
            
            if (quantity.HasValue && quantity.Value != _quantity)
            {
                ValidateQuantity(quantity.Value);
                _quantity = quantity.Value;
                RecalculateTotalCost();
                changed = true;
            }
            
            if (unit != null && unit != _unit)
            {
                _unit = unit;
                changed = true;
            }
            
            if (unitCost.HasValue && unitCost.Value != _unitCost)
            {
                ValidateUnitCost(unitCost.Value);
                _unitCost = unitCost.Value;
                RecalculateTotalCost();
                changed = true;
            }
            
            if (supplier != null && supplier != _supplier)
            {
                _supplier = supplier;
                changed = true;
            }
            
            if (supplierId != SupplierId)
            {
                SupplierId = supplierId;
                changed = true;
            }
            
            if (changed)
            {
                AddDomainEvent(new MaterialUpdatedEvent(this));
            }
        }
        
        public void Order(DateTime orderDate)
        {
            if (_status != "Pending")
                throw new BusinessRuleException("Material must be in Pending status to be ordered");
                
            if (orderDate > DateTime.UtcNow)
                throw new BusinessRuleException("Order date cannot be in the future");
                
            _orderDate = orderDate;
            _status = "Ordered";
            AddDomainEvent(new MaterialOrderedEvent(this));
        }
        
        public void Receive(DateTime deliveryDate, decimal? actualQuantity = null)
        {
            if (_status != "Ordered")
                throw new BusinessRuleException("Material must be in Ordered status to be received");
                
            if (deliveryDate < _orderDate)
                throw new BusinessRuleException("Delivery date cannot be before order date");
                
            if (deliveryDate > DateTime.UtcNow)
                throw new BusinessRuleException("Delivery date cannot be in the future");
                
            _deliveryDate = deliveryDate;
            
            if (actualQuantity.HasValue)
            {
                ValidateQuantity(actualQuantity.Value);
                _quantity = actualQuantity.Value;
                RecalculateTotalCost();
            }
            
            _status = "Received";
            AddDomainEvent(new MaterialReceivedEvent(this));
        }
        
        public void ChangeStatus(string status)
        {
            if (status == _status)
                return;
                
            string[] validStatuses = { "Pending", "Ordered", "Received", "Used", "Returned", "Cancelled" };
            if (!Array.Exists(validStatuses, s => s == status))
                throw new BusinessRuleException($"Invalid status: {status}");
                
            _status = status;
            AddDomainEvent(new MaterialStatusChangedEvent(this));
        }
        
        #region Validation Methods
        
        private void ValidateMaterial(string name, string description, decimal quantity, decimal? unitCost)
        {
            ValidateName(name);
            ValidateDescription(description);
            ValidateQuantity(quantity);
            
            if (unitCost.HasValue)
            {
                ValidateUnitCost(unitCost.Value);
            }
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
        
        private void ValidateUnitCost(decimal unitCost)
        {
            if (unitCost < 0)
                throw new BusinessRuleException("Unit cost cannot be negative");
        }
        
        private void RecalculateTotalCost()
        {
            if (_unitCost.HasValue)
            {
                _totalCost = _unitCost.Value * _quantity;
            }
        }
        
        #endregion
    }
} 