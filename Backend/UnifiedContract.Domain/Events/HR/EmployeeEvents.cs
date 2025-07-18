using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Domain.Events.HR
{
    public class EmployeeCreatedEvent : DomainEvent
    {
        public Employee Employee { get; }

        public EmployeeCreatedEvent(Employee employee)
        {
            Employee = employee;
        }
    }

    public class EmployeeDepartmentChangedEvent : DomainEvent
    {
        public Employee Employee { get; }

        public EmployeeDepartmentChangedEvent(Employee employee)
        {
            Employee = employee;
        }
    }

    public class EmployeeManagerChangedEvent : DomainEvent
    {
        public Employee Employee { get; }

        public EmployeeManagerChangedEvent(Employee employee)
        {
            Employee = employee;
        }
    }

    public class EmployeeContactInfoUpdatedEvent : DomainEvent
    {
        public Employee Employee { get; }

        public EmployeeContactInfoUpdatedEvent(Employee employee)
        {
            Employee = employee;
        }
    }

    public class EmployeeJobInfoUpdatedEvent : DomainEvent
    {
        public Employee Employee { get; }

        public EmployeeJobInfoUpdatedEvent(Employee employee)
        {
            Employee = employee;
        }
    }

    public class EmployeeSalaryUpdatedEvent : DomainEvent
    {
        public Employee Employee { get; }

        public EmployeeSalaryUpdatedEvent(Employee employee)
        {
            Employee = employee;
        }
    }

    public class EmployeeDeletedEvent : DomainEvent
    {
        public Employee Employee { get; }
        public string DeletionReason { get; }

        public EmployeeDeletedEvent(Employee employee, string deletionReason)
        {
            Employee = employee;
            DeletionReason = deletionReason;
        }
    }

    public class EmployeeStatusChangedEvent : DomainEvent
    {
        public Employee Employee { get; }
        public string PreviousStatus { get; }
        public string NewStatus { get; }

        public EmployeeStatusChangedEvent(Employee employee, string previousStatus, string newStatus)
        {
            Employee = employee;
            PreviousStatus = previousStatus;
            NewStatus = newStatus;
        }
    }

    public class EmployeeTerminatedEvent : DomainEvent
    {
        public Employee Employee { get; }
        public DateTime TerminationDate { get; }
        public string TerminationReason { get; }

        public EmployeeTerminatedEvent(Employee employee, DateTime terminationDate, string terminationReason)
        {
            Employee = employee;
            TerminationDate = terminationDate;
            TerminationReason = terminationReason;
        }
    }
} 