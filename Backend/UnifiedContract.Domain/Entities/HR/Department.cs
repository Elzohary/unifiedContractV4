using System;
using System.Collections.Generic;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Events.HR;

namespace UnifiedContract.Domain.Entities.HR
{
    public class Department : BaseEntity
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public Guid? ParentDepartmentId { get; private set; }
        public Guid? ManagerId { get; private set; }
        public string Location { get; private set; }
        public bool IsActive { get; private set; } = true;
        public int HeadCount { get; private set; }
        public decimal Budget { get; private set; }
        
        // Navigation properties
        public virtual Department ParentDepartment { get; set; }
        public virtual ICollection<Department> ChildDepartments { get; set; } = new List<Department>();
        public virtual Employee Manager { get; set; }
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        
        // For EF Core
        protected Department() { }
        
        public Department(string name, string code, string description = null)
        {
            ValidateDepartment(name, code);
            
            Name = name;
            Code = code;
            Description = description;
            IsActive = true;
            
            AddDomainEvent(new DepartmentCreatedEvent(this));
        }
        
        public void UpdateDetails(string name, string code, string description = null, string location = null)
        {
            ValidateDepartment(name, code);
            
            Name = name;
            Code = code;
            
            if (description != null)
                Description = description;
                
            if (location != null)
                Location = location;
                
            AddDomainEvent(new DepartmentUpdatedEvent(this));
        }
        
        public void Activate()
        {
            if (!IsActive)
            {
                IsActive = true;
                AddDomainEvent(new DepartmentActivatedEvent(this));
            }
        }
        
        public void Deactivate()
        {
            if (IsActive)
            {
                IsActive = false;
                AddDomainEvent(new DepartmentDeactivatedEvent(this));
            }
        }
        
        public void AssignManager(Guid managerId)
        {
            if (managerId == Guid.Empty)
                throw new DomainException("Manager ID cannot be empty");
                
            ManagerId = managerId;
            AddDomainEvent(new DepartmentManagerAssignedEvent(this));
        }
        
        public void AssignParentDepartment(Guid parentDepartmentId)
        {
            if (parentDepartmentId == Guid.Empty)
                throw new DomainException("Parent department ID cannot be empty");
                
            if (parentDepartmentId == Id)
                throw new DomainException("Department cannot be its own parent");
                
            // Note: A more comprehensive implementation would check for circular references
            // throughout the department hierarchy
                
            ParentDepartmentId = parentDepartmentId;
            AddDomainEvent(new DepartmentHierarchyChangedEvent(this));
        }
        
        public void UpdateBudget(decimal budget)
        {
            if (budget < 0)
                throw new DomainException("Budget cannot be negative");
                
            Budget = budget;
            AddDomainEvent(new DepartmentBudgetUpdatedEvent(this));
        }
        
        public void UpdateHeadCount(int headCount)
        {
            if (headCount < 0)
                throw new DomainException("Head count cannot be negative");
                
            HeadCount = headCount;
            AddDomainEvent(new DepartmentHeadCountUpdatedEvent(this));
        }
        
        private static void ValidateDepartment(string name, string code)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Department name is required");
                
            if (name.Length > 100)
                throw new DomainException("Department name cannot be longer than 100 characters");
                
            if (string.IsNullOrWhiteSpace(code))
                throw new DomainException("Department code is required");
                
            if (code.Length > 20)
                throw new DomainException("Department code cannot be longer than 20 characters");
        }
    }
} 