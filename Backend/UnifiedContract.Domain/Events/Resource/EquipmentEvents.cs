using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.Resource;

namespace UnifiedContract.Domain.Events.Resource
{
    public class EquipmentCreatedEvent : DomainEvent
    {
        public Equipment Equipment { get; }

        public EquipmentCreatedEvent(Equipment equipment)
        {
            Equipment = equipment;
        }
    }

    public class EquipmentUpdatedEvent : DomainEvent
    {
        public Equipment Equipment { get; }

        public EquipmentUpdatedEvent(Equipment equipment)
        {
            Equipment = equipment;
        }
    }

    public class EquipmentStatusChangedEvent : DomainEvent
    {
        public Equipment Equipment { get; }
        public Guid NewStatusId { get; }

        public EquipmentStatusChangedEvent(Equipment equipment)
        {
            Equipment = equipment;
            NewStatusId = equipment.EquipmentStatusId;
        }
    }

    public class EquipmentOperatorChangedEvent : DomainEvent
    {
        public Equipment Equipment { get; }
        public Guid? OperatorId { get; }

        public EquipmentOperatorChangedEvent(Equipment equipment)
        {
            Equipment = equipment;
            OperatorId = equipment.CurrentOperatorId;
        }
    }

    public class EquipmentSupplierChangedEvent : DomainEvent
    {
        public Equipment Equipment { get; }
        public Guid? SupplierId { get; }

        public EquipmentSupplierChangedEvent(Equipment equipment)
        {
            Equipment = equipment;
            SupplierId = equipment.SupplierId;
        }
    }

    public class EquipmentMaintenanceRecordedEvent : DomainEvent
    {
        public Equipment Equipment { get; }
        public DateTime MaintenanceDate { get; }

        public EquipmentMaintenanceRecordedEvent(Equipment equipment)
        {
            Equipment = equipment;
            MaintenanceDate = equipment.LastMaintenanceDate ?? DateTime.UtcNow;
        }
    }

    public class EquipmentMaintenanceScheduledEvent : DomainEvent
    {
        public Equipment Equipment { get; }
        public DateTime ScheduledDate { get; }

        public EquipmentMaintenanceScheduledEvent(Equipment equipment)
        {
            Equipment = equipment;
            ScheduledDate = equipment.NextMaintenanceDate ?? DateTime.UtcNow;
        }
    }
} 