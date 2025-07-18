using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.Resource;

namespace UnifiedContract.Domain.Events.Resource
{
    #region Equipment Assignment Events
    
    public class EquipmentAssignedEvent : DomainEvent
    {
        public EquipmentAssignment Assignment { get; }
        public Guid EquipmentId { get; }
        public Guid WorkOrderId { get; }

        public EquipmentAssignedEvent(EquipmentAssignment assignment)
        {
            Assignment = assignment;
            EquipmentId = assignment.EquipmentId;
            WorkOrderId = assignment.WorkOrderId;
        }
    }
    
    public class EquipmentAssignmentUpdatedEvent : DomainEvent
    {
        public EquipmentAssignment Assignment { get; }

        public EquipmentAssignmentUpdatedEvent(EquipmentAssignment assignment)
        {
            Assignment = assignment;
        }
    }
    
    public class EquipmentAssignmentCompletedEvent : DomainEvent
    {
        public EquipmentAssignment Assignment { get; }
        public DateTime EndDate { get; }

        public EquipmentAssignmentCompletedEvent(EquipmentAssignment assignment)
        {
            Assignment = assignment;
            EndDate = assignment.EndDate ?? DateTime.UtcNow;
        }
    }
    
    #endregion
    
    #region Material Assignment Events
    
    public class MaterialAssignedEvent : DomainEvent
    {
        public MaterialAssignment Assignment { get; }

        public MaterialAssignedEvent(MaterialAssignment assignment)
        {
            Assignment = assignment;
        }
    }
    
    public class MaterialAssignmentUpdatedEvent : DomainEvent
    {
        public MaterialAssignment Assignment { get; }

        public MaterialAssignmentUpdatedEvent(MaterialAssignment assignment)
        {
            Assignment = assignment;
        }
    }
    
    public class MaterialAssignmentCompletedEvent : DomainEvent
    {
        public MaterialAssignment Assignment { get; }

        public MaterialAssignmentCompletedEvent(MaterialAssignment assignment)
        {
            Assignment = assignment;
        }
    }
    
    #endregion
    
    #region Manpower Assignment Events
    
    public class ManpowerAssignedEvent : DomainEvent
    {
        public ManpowerAssignment Assignment { get; }

        public ManpowerAssignedEvent(ManpowerAssignment assignment)
        {
            Assignment = assignment;
        }
    }
    
    public class ManpowerAssignmentUpdatedEvent : DomainEvent
    {
        public ManpowerAssignment Assignment { get; }

        public ManpowerAssignmentUpdatedEvent(ManpowerAssignment assignment)
        {
            Assignment = assignment;
        }
    }
    
    public class ManpowerAssignmentCompletedEvent : DomainEvent
    {
        public ManpowerAssignment Assignment { get; }
        public DateTime EndDate { get; }

        public ManpowerAssignmentCompletedEvent(ManpowerAssignment assignment)
        {
            Assignment = assignment;
            EndDate = assignment.EndDate ?? DateTime.UtcNow;
        }
    }
    
    #endregion
} 