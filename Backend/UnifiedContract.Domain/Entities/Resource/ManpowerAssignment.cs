using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Events.Resource;
using UnifiedContract.Domain.Exceptions;

namespace UnifiedContract.Domain.Entities.Resource
{
    public class ManpowerAssignment : BaseEntity
    {
        private string _badgeNumber;
        private string _name;
        private string _role;
        private int _hoursAssigned;
        private DateTime _startDate;
        private DateTime? _endDate;
        private string _notes;
        private string _workOrderNumber;
        
        // Foreign keys
        public Guid? UserId { get; private set; }
        public Guid? EmployeeId { get; private set; }
        public Guid WorkOrderId { get; private set; }
        
        // Navigation properties
        public virtual HR.Employee Employee { get; private set; }
        public virtual Auth.User User { get; private set; }
        public virtual WorkOrder.WorkOrder WorkOrder { get; private set; }
        
        // Public properties with private setters
        public string BadgeNumber => _badgeNumber;
        public string Name => _name;
        public string Role => _role;
        public int HoursAssigned => _hoursAssigned;
        public DateTime StartDate => _startDate;
        public DateTime? EndDate => _endDate;
        public string Notes => _notes;
        public string WorkOrderNumber => _workOrderNumber;
        
        // Required by EF Core
        private ManpowerAssignment() { }
        
        public ManpowerAssignment(
            string badgeNumber,
            string name,
            string role,
            int hoursAssigned,
            DateTime startDate,
            Guid workOrderId,
            string workOrderNumber,
            Guid? userId = null,
            Guid? employeeId = null,
            string notes = null)
        {
            ValidateAssignment(badgeNumber, name, role, hoursAssigned, startDate);
            
            _badgeNumber = badgeNumber;
            _name = name;
            _role = role;
            _hoursAssigned = hoursAssigned;
            _startDate = startDate;
            WorkOrderId = workOrderId;
            _workOrderNumber = workOrderNumber;
            UserId = userId;
            EmployeeId = employeeId;
            _notes = notes;
            
            AddDomainEvent(new ManpowerAssignedEvent(this));
        }
        
        public void UpdateDetails(
            string role = null,
            int? hoursAssigned = null,
            DateTime? startDate = null,
            string notes = null)
        {
            bool changed = false;
            
            if (role != null && role != _role)
            {
                ValidateRole(role);
                _role = role;
                changed = true;
            }
            
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
            
            if (changed)
            {
                AddDomainEvent(new ManpowerAssignmentUpdatedEvent(this));
            }
        }
        
        public void CompleteAssignment(DateTime endDate)
        {
            if (endDate < _startDate)
                throw new BusinessRuleException("End date cannot be before start date");
                
            if (endDate > DateTime.UtcNow)
                throw new BusinessRuleException("End date cannot be in the future");
                
            _endDate = endDate;
            AddDomainEvent(new ManpowerAssignmentCompletedEvent(this));
        }
        
        #region Validation Methods
        
        private void ValidateAssignment(string badgeNumber, string name, string role, int hoursAssigned, DateTime startDate)
        {
            ValidateBadgeNumber(badgeNumber);
            ValidateName(name);
            ValidateRole(role);
            ValidateHoursAssigned(hoursAssigned);
            ValidateStartDate(startDate);
        }
        
        private void ValidateBadgeNumber(string badgeNumber)
        {
            if (string.IsNullOrWhiteSpace(badgeNumber))
                throw new BusinessRuleException("Badge number is required");
                
            if (badgeNumber.Length > 20)
                throw new BusinessRuleException("Badge number cannot exceed 20 characters");
        }
        
        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BusinessRuleException("Name is required");
                
            if (name.Length > 100)
                throw new BusinessRuleException("Name cannot exceed 100 characters");
        }
        
        private void ValidateRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new BusinessRuleException("Role is required");
                
            if (role.Length > 50)
                throw new BusinessRuleException("Role cannot exceed 50 characters");
        }
        
        private void ValidateHoursAssigned(int hoursAssigned)
        {
            if (hoursAssigned <= 0)
                throw new BusinessRuleException("Hours assigned must be greater than zero");
                
            if (hoursAssigned > 24 * 30)  // Maximum 30 days of continuous work
                throw new BusinessRuleException("Hours assigned cannot exceed 720 hours (30 days)");
        }
        
        private void ValidateStartDate(DateTime startDate)
        {
            if (startDate < DateTime.UtcNow.AddMonths(-6))
                throw new BusinessRuleException("Start date cannot be more than 6 months in the past");
                
            if (startDate > DateTime.UtcNow.AddMonths(6))
                throw new BusinessRuleException("Start date cannot be more than 6 months in the future");
        }
        
        #endregion
    }
} 