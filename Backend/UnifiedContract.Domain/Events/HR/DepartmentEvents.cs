using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Domain.Events.HR
{
    public class DepartmentCreatedEvent : DomainEvent
    {
        public Department Department { get; }

        public DepartmentCreatedEvent(Department department)
        {
            Department = department;
        }
    }

    public class DepartmentUpdatedEvent : DomainEvent
    {
        public Department Department { get; }

        public DepartmentUpdatedEvent(Department department)
        {
            Department = department;
        }
    }

    public class DepartmentActivatedEvent : DomainEvent
    {
        public Department Department { get; }

        public DepartmentActivatedEvent(Department department)
        {
            Department = department;
        }
    }

    public class DepartmentDeactivatedEvent : DomainEvent
    {
        public Department Department { get; }

        public DepartmentDeactivatedEvent(Department department)
        {
            Department = department;
        }
    }

    public class DepartmentManagerAssignedEvent : DomainEvent
    {
        public Department Department { get; }

        public DepartmentManagerAssignedEvent(Department department)
        {
            Department = department;
        }
    }

    public class DepartmentHierarchyChangedEvent : DomainEvent
    {
        public Department Department { get; }

        public DepartmentHierarchyChangedEvent(Department department)
        {
            Department = department;
        }
    }

    public class DepartmentBudgetUpdatedEvent : DomainEvent
    {
        public Department Department { get; }

        public DepartmentBudgetUpdatedEvent(Department department)
        {
            Department = department;
        }
    }

    public class DepartmentHeadCountUpdatedEvent : DomainEvent
    {
        public Department Department { get; }

        public DepartmentHeadCountUpdatedEvent(Department department)
        {
            Department = department;
        }
    }

    public class DepartmentDeletedEvent : DomainEvent
    {
        public Department Department { get; }
        public string DeletionReason { get; }

        public DepartmentDeletedEvent(Department department, string deletionReason)
        {
            Department = department;
            DeletionReason = deletionReason;
        }
    }

    public class DepartmentMergedEvent : DomainEvent
    {
        public Department SourceDepartment { get; }
        public Department TargetDepartment { get; }
        public string MergeReason { get; }

        public DepartmentMergedEvent(Department sourceDepartment, Department targetDepartment, string mergeReason)
        {
            SourceDepartment = sourceDepartment;
            TargetDepartment = targetDepartment;
            MergeReason = mergeReason;
        }
    }

    public class DepartmentSplitEvent : DomainEvent
    {
        public Department OriginalDepartment { get; }
        public Department NewDepartment { get; }
        public string SplitReason { get; }

        public DepartmentSplitEvent(Department originalDepartment, Department newDepartment, string splitReason)
        {
            OriginalDepartment = originalDepartment;
            NewDepartment = newDepartment;
            SplitReason = splitReason;
        }
    }
} 