using System;

namespace UnifiedContract.Application.DTOs.HR
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string BadgeNumber { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string JobTitle { get; set; }
        public string WorkLocation { get; set; }
        public string CompanyPhone { get; set; }
        public string PersonalPhone { get; set; }
        public string IqamaNumber { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }
        public Guid? DirectManagerId { get; set; }
        public string DirectManagerName { get; set; }
        public Guid? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public double WorkTimeRatio { get; set; }
        public int MonthlyHours { get; set; }
        public double AvgLateMinutes { get; set; }
        public DateTime JoinDate { get; set; }
        public string CurrentProject { get; set; }
        public int SickLeaveCounter { get; set; }
        public int OffDays { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
    }

    public class CreateEmployeeDto
    {
        public string BadgeNumber { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public DateTime JoinDate { get; set; }
        public string Nationality { get; set; }
        public Guid DepartmentId { get; set; }
        public string CompanyPhone { get; set; }
        public string PersonalPhone { get; set; }
        public decimal Salary { get; set; }
        public Guid? DirectManagerId { get; set; }
    }

    public class UpdateEmployeeDto
    {
        public string BadgeNumber { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string CompanyPhone { get; set; }
        public string PersonalPhone { get; set; }
        public Guid? DepartmentId { get; set; }
        public decimal? Salary { get; set; }
        public Guid? DirectManagerId { get; set; }
        public bool? IsActive { get; set; }
    }
} 