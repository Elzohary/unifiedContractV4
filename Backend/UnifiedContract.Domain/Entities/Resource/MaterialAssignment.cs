using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Enums;
using UnifiedContract.Domain.Events.Resource;
using UnifiedContract.Domain.Exceptions;

namespace UnifiedContract.Domain.Entities.Resource
{
    public class MaterialAssignment : BaseEntity
    {
        private DateTime _assignDate;
        private string _storingLocation;
        private string _workOrderNumber;
        private decimal _quantity;
        private string _unit;
        private string _notes;
        
        // Foreign keys
        public MaterialType MaterialType { get; private set; }
        public Guid? PurchasableMaterialId { get; private set; }
        public Guid? ReceivableMaterialId { get; private set; }
        public Guid? WorkOrderId { get; private set; }
        public Guid AssignedById { get; private set; }
        
        // Navigation properties
        public virtual PurchasableMaterial PurchasableMaterial { get; private set; }
        public virtual ReceivableMaterial ReceivableMaterial { get; private set; }
        public virtual WorkOrder.WorkOrder WorkOrder { get; private set; }
        public virtual HR.Employee AssignedBy { get; private set; }
        
        // Public properties with private setters
        public DateTime AssignDate => _assignDate;
        public string StoringLocation => _storingLocation;
        public string WorkOrderNumber => _workOrderNumber;
        public decimal Quantity => _quantity;
        public string Unit => _unit;
        public string Notes => _notes;
        
        // Required by EF Core
        private MaterialAssignment() { }
        
        public MaterialAssignment(
            MaterialType materialType,
            DateTime assignDate,
            Guid assignedById,
            decimal quantity,
            string unit,
            string storingLocation = null,
            string workOrderNumber = null,
            Guid? workOrderId = null,
            Guid? purchasableMaterialId = null,
            Guid? receivableMaterialId = null,
            string notes = null)
        {
            ValidateAssignment(materialType, assignDate, quantity);
            
            if ((purchasableMaterialId == null && receivableMaterialId == null) ||
                (purchasableMaterialId != null && receivableMaterialId != null))
            {
                throw new BusinessRuleException("Either purchasable material or receivable material must be specified");
            }
            
            MaterialType = materialType;
            _assignDate = assignDate;
            AssignedById = assignedById;
            _quantity = quantity;
            _unit = unit ?? "Units";
            _storingLocation = storingLocation;
            _workOrderNumber = workOrderNumber;
            WorkOrderId = workOrderId;
            PurchasableMaterialId = purchasableMaterialId;
            ReceivableMaterialId = receivableMaterialId;
            _notes = notes;
            
            AddDomainEvent(new MaterialAssignedEvent(this));
        }
        
        public void UpdateDetails(
            decimal? quantity = null, 
            string storingLocation = null,
            string notes = null)
        {
            bool changed = false;
            
            if (quantity.HasValue && quantity.Value != _quantity)
            {
                ValidateQuantity(quantity.Value);
                _quantity = quantity.Value;
                changed = true;
            }
            
            if (storingLocation != null && storingLocation != _storingLocation)
            {
                _storingLocation = storingLocation;
                changed = true;
            }
            
            if (notes != null && notes != _notes)
            {
                _notes = notes;
                changed = true;
            }
            
            if (changed)
            {
                AddDomainEvent(new MaterialAssignmentUpdatedEvent(this));
            }
        }
        
        public void CompleteAssignment()
        {
            AddDomainEvent(new MaterialAssignmentCompletedEvent(this));
        }
        
        #region Validation Methods
        
        private void ValidateAssignment(MaterialType materialType, DateTime assignDate, decimal quantity)
        {
            ValidateMaterialType(materialType);
            ValidateAssignDate(assignDate);
            ValidateQuantity(quantity);
        }
        
        private void ValidateMaterialType(MaterialType materialType)
        {
            if (!Enum.IsDefined(typeof(MaterialType), materialType))
                throw new BusinessRuleException("Invalid material type");
        }
        
        private void ValidateAssignDate(DateTime assignDate)
        {
            if (assignDate > DateTime.UtcNow)
                throw new BusinessRuleException("Assign date cannot be in the future");
                
            if (assignDate < DateTime.UtcNow.AddMonths(-6))
                throw new BusinessRuleException("Assign date cannot be more than 6 months in the past");
        }
        
        private void ValidateQuantity(decimal quantity)
        {
            if (quantity <= 0)
                throw new BusinessRuleException("Quantity must be greater than zero");
        }
        
        #endregion
    }
} 