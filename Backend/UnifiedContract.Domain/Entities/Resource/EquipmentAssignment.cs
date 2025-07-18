using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Events.Resource;
using UnifiedContract.Domain.Exceptions;

namespace UnifiedContract.Domain.Entities.Resource
{
    public class EquipmentAssignment : BaseEntity
    {
        private string _companyNumber;
        private string _type;
        private string _operatorBadgeNumber;
        private int _hoursAssigned;
        private DateTime _startDate;
        private DateTime? _endDate;
        private string _notes;
        private string _workOrderNumber;
        
        // Foreign keys
        public Guid EquipmentId { get; private set; }
        public Guid WorkOrderId { get; private set; }
        public Guid? OperatorId { get; private set; }
        
        // Navigation properties
        public virtual Equipment Equipment { get; private set; }
        public virtual WorkOrder.WorkOrder WorkOrder { get; private set; }
        public virtual HR.Employee Operator { get; private set; }
        
        // Public properties with private setters
        public string CompanyNumber => _companyNumber;
        public string Type => _type;
        public string OperatorBadgeNumber => _operatorBadgeNumber;
        public int HoursAssigned => _hoursAssigned;
        public DateTime StartDate => _startDate;
        public DateTime? EndDate => _endDate;
        public string Notes => _notes;
        public string WorkOrderNumber => _workOrderNumber;
        
        // Required by EF Core
        private EquipmentAssignment() { }
        
        public EquipmentAssignment(
            Guid equipmentId,
            Guid workOrderId,
            string companyNumber,
            string type,
            DateTime startDate,
            int hoursAssigned,
            string workOrderNumber,
            Guid? operatorId = null,
            string operatorBadgeNumber = null,
            string notes = null)
        {
            ValidateAssignment(companyNumber, type, startDate, hoursAssigned);
            
            EquipmentId = equipmentId;
            WorkOrderId = workOrderId;
            _companyNumber = companyNumber;
            _type = type;
            _startDate = startDate;
            _hoursAssigned = hoursAssigned;
            _workOrderNumber = workOrderNumber;
            OperatorId = operatorId;
            _operatorBadgeNumber = operatorBadgeNumber;
            _notes = notes;
            
            AddDomainEvent(new EquipmentAssignedEvent(this));
        }
        
        public void UpdateDetails(
            int? hoursAssigned = null,
            DateTime? startDate = null,
            string notes = null,
            Guid? operatorId = null,
            string operatorBadgeNumber = null)
        {
            bool changed = false;
            
            if (hoursAssigned.HasValue && hoursAssigned.Value != _hoursAssigned)
            {
                ValidateHoursAssigned(hoursAssigned.Value);
                _hoursAssigned = hoursAssigned.Value;
                changed = true;
            }
            
            if (startDate.HasValue && startDate.Value != _startDate)
            {
                ValidateStartDate(startDate.Value);
                _startDate = startDate.Value;
                changed = true;
            }
            
            if (notes != null && notes != _notes)
            {
                _notes = notes;
                changed = true;
            }
            
            if (operatorId != OperatorId)
            {
                OperatorId = operatorId;
                changed = true;
            }
            
            if (operatorBadgeNumber != null && operatorBadgeNumber != _operatorBadgeNumber)
            {
                _operatorBadgeNumber = operatorBadgeNumber;
                changed = true;
            }
            
            if (changed)
            {
                AddDomainEvent(new EquipmentAssignmentUpdatedEvent(this));
            }
        }
        
        public void CompleteAssignment(DateTime endDate)
        {
            if (endDate < _startDate)
                throw new BusinessRuleException("End date cannot be before start date");
                
            if (endDate > DateTime.UtcNow)
                throw new BusinessRuleException("End date cannot be in the future");
                
            _endDate = endDate;
            AddDomainEvent(new EquipmentAssignmentCompletedEvent(this));
        }
        
        #region Validation Methods
        
        private void ValidateAssignment(string companyNumber, string type, DateTime startDate, int hoursAssigned)
        {
            ValidateCompanyNumber(companyNumber);
            ValidateType(type);
            ValidateStartDate(startDate);
            ValidateHoursAssigned(hoursAssigned);
        }
        
        private void ValidateCompanyNumber(string companyNumber)
        {
            if (string.IsNullOrWhiteSpace(companyNumber))
                throw new BusinessRuleException("Equipment company number is required");
                
            if (companyNumber.Length > 50)
                throw new BusinessRuleException("Equipment company number cannot exceed 50 characters");
        }
        
        private void ValidateType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new BusinessRuleException("Equipment type is required");
                
            if (type.Length > 50)
                throw new BusinessRuleException("Equipment type cannot exceed 50 characters");
        }
        
        private void ValidateStartDate(DateTime startDate)
        {
            if (startDate < DateTime.UtcNow.AddMonths(-6))
                throw new BusinessRuleException("Start date cannot be more than 6 months in the past");
                
            if (startDate > DateTime.UtcNow.AddMonths(6))
                throw new BusinessRuleException("Start date cannot be more than 6 months in the future");
        }
        
        private void ValidateHoursAssigned(int hoursAssigned)
        {
            if (hoursAssigned <= 0)
                throw new BusinessRuleException("Hours assigned must be greater than zero");
                
            if (hoursAssigned > 24 * 30)  // Maximum 30 days of continuous operation
                throw new BusinessRuleException("Hours assigned cannot exceed 720 hours (30 days)");
        }
        
        #endregion
    }
} 