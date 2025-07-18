using System;
using System.Collections.Generic;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.ValueObjects;
using UnifiedContract.Domain.Events.HR;

namespace UnifiedContract.Domain.Entities.HR
{
    public class Employee : BaseEntity
    {
        // Properties with private setters to enforce business rules
        public string BadgeNumber { get; private set; }
        public string Name { get; private set; }
        public string Photo { get; set; }
        public string JobTitle { get; private set; }
        public string WorkLocation { get; private set; }
        public Address HomeAddress { get; private set; }
        public string HomeType { get; private set; } // 'company' or 'personal'
        public string CompanyPhone { get; private set; }
        public string PersonalPhone { get; private set; }
        public string IqamaNumber { get; private set; }
        public int Age { get; private set; }
        public string Nationality { get; private set; }
        public Guid? DirectManagerId { get; private set; }
        public Guid? DepartmentId { get; private set; }
        public double WorkTimeRatio { get; private set; } // Percentage of expected work hours fulfilled
        public int MonthlyHours { get; private set; }
        public double AvgLateMinutes { get; private set; }
        public DateTime JoinDate { get; private set; }
        public string CurrentProject { get; private set; }
        public int SickLeaveCounter { get; private set; }
        public int OffDays { get; private set; }
        public Money Salary { get; private set; }
        public Guid? UserId { get; private set; }
        
        // Navigation properties
        public virtual Employee DirectManager { get; set; }
        public virtual Department Department { get; set; }
        // Using object instead of ApplicationUser until Identity framework is implemented
        public virtual object User { get; set; }
        public virtual ICollection<Employee> ManagedEmployees { get; set; } = new List<Employee>();
        public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; } = new List<EmergencyContact>();
        public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public virtual ICollection<WorkExperience> WorkExperiences { get; set; } = new List<WorkExperience>();
        public virtual ICollection<Education> EducationHistory { get; set; } = new List<Education>();
        public virtual ICollection<EmployeeSkill> Skills { get; set; } = new List<EmployeeSkill>();
        public virtual ICollection<Leave> Leaves { get; set; } = new List<Leave>();
        public virtual ICollection<PerformanceReview> PerformanceReviews { get; set; } = new List<PerformanceReview>();
        public virtual ICollection<PerformanceReview> ReviewsGiven { get; set; } = new List<PerformanceReview>();
        public virtual ICollection<Department> ManagedDepartments { get; set; } = new List<Department>();
        public virtual ICollection<Identification> Identifications { get; set; } = new List<Identification>();
        public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
        public virtual ICollection<Training> Trainings { get; set; } = new List<Training>();
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public virtual ICollection<Salary> SalaryHistory { get; set; } = new List<Salary>();
        
        // For EF Core
        protected Employee() { }
        
        public Employee(
            string badgeNumber, 
            string name, 
            string jobTitle, 
            DateTime joinDate,
            string nationality = null,
            Address homeAddress = null)
        {
            ValidateEmployee(badgeNumber, name, jobTitle);
            
            BadgeNumber = badgeNumber;
            Name = name;
            JobTitle = jobTitle;
            JoinDate = joinDate;
            Nationality = nationality;
            HomeAddress = homeAddress;
            WorkTimeRatio = 100; // Default to 100%
            OffDays = 30; // Default annual leave balance
            
            AddDomainEvent(new EmployeeCreatedEvent(this));
        }
        
        // Business methods
        public void AssignDepartment(Guid departmentId)
        {
            if (departmentId == Guid.Empty)
                throw new DomainException("Department ID cannot be empty");
                
            DepartmentId = departmentId;
            AddDomainEvent(new EmployeeDepartmentChangedEvent(this));
        }
        
        public void AssignManager(Guid managerId)
        {
            if (managerId == Guid.Empty)
                throw new DomainException("Manager ID cannot be empty");
                
            if (managerId == Id)
                throw new DomainException("Employee cannot be their own manager");
                
            DirectManagerId = managerId;
            AddDomainEvent(new EmployeeManagerChangedEvent(this));
        }
        
        public void UpdateContactInformation(
            string companyPhone = null, 
            string personalPhone = null, 
            Address homeAddress = null,
            string homeType = null)
        {
            if (companyPhone != null)
                CompanyPhone = companyPhone;
                
            if (personalPhone != null)
                PersonalPhone = personalPhone;
                
            if (homeAddress != null)
                HomeAddress = homeAddress;
                
            if (homeType != null)
            {
                if (homeType != "company" && homeType != "personal")
                    throw new DomainException("Home type must be either 'company' or 'personal'");
                    
                HomeType = homeType;
            }
            
            AddDomainEvent(new EmployeeContactInfoUpdatedEvent(this));
        }
        
        public void UpdateJobInformation(string jobTitle, string workLocation = null, string currentProject = null)
        {
            if (string.IsNullOrWhiteSpace(jobTitle))
                throw new DomainException("Job title is required");
                
            if (jobTitle.Length > 100)
                throw new DomainException("Job title cannot be longer than 100 characters");
                
            JobTitle = jobTitle;
            
            if (workLocation != null)
                WorkLocation = workLocation;
                
            if (currentProject != null)
                CurrentProject = currentProject;
                
            AddDomainEvent(new EmployeeJobInfoUpdatedEvent(this));
        }
        
        public void UpdateSalary(Money newSalary)
        {
            if (newSalary == null)
                throw new DomainException("Salary cannot be null");
                
            if (newSalary.Amount < 0)
                throw new DomainException("Salary cannot be negative");
                
            Salary = newSalary;
            AddDomainEvent(new EmployeeSalaryUpdatedEvent(this));
        }
        
        public void DecrementLeaveBalance(int days)
        {
            if (days <= 0)
                throw new DomainException("Days must be positive");
                
            if (days > OffDays)
                throw new DomainException("Insufficient leave balance");
                
            OffDays -= days;
        }
        
        public void IncrementLeaveBalance(int days)
        {
            if (days <= 0)
                throw new DomainException("Days must be positive");
                
            OffDays += days;
        }
        
        public void IncrementSickLeaveCounter(int days)
        {
            if (days <= 0)
                throw new DomainException("Days must be positive");
                
            SickLeaveCounter += days;
        }
        
        private static void ValidateEmployee(string badgeNumber, string name, string jobTitle)
        {
            if (string.IsNullOrWhiteSpace(badgeNumber))
                throw new DomainException("Badge number is required");
                
            if (badgeNumber.Length > 20)
                throw new DomainException("Badge number cannot be longer than 20 characters");
                
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name is required");
                
            if (name.Length > 100)
                throw new DomainException("Name cannot be longer than 100 characters");
                
            if (string.IsNullOrWhiteSpace(jobTitle))
                throw new DomainException("Job title is required");
                
            if (jobTitle.Length > 100)
                throw new DomainException("Job title cannot be longer than 100 characters");
        }
    }
} 