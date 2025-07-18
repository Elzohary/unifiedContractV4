using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.Resource;

namespace UnifiedContract.Domain.Events.Resource
{
    public class MaintenanceCreatedEvent : DomainEvent
    {
        public EquipmentMaintenance Maintenance { get; }

        public MaintenanceCreatedEvent(EquipmentMaintenance maintenance)
        {
            Maintenance = maintenance;
        }
    }
    
    public class MaintenanceUpdatedEvent : DomainEvent
    {
        public EquipmentMaintenance Maintenance { get; }

        public MaintenanceUpdatedEvent(EquipmentMaintenance maintenance)
        {
            Maintenance = maintenance;
        }
    }
    
    public class MaintenanceStatusChangedEvent : DomainEvent
    {
        public EquipmentMaintenance Maintenance { get; }
        public Guid StatusId { get; }

        public MaintenanceStatusChangedEvent(EquipmentMaintenance maintenance)
        {
            Maintenance = maintenance;
            StatusId = maintenance.MaintenanceStatusId;
        }
    }
    
    public class MaintenanceTypeChangedEvent : DomainEvent
    {
        public EquipmentMaintenance Maintenance { get; }
        public Guid TypeId { get; }

        public MaintenanceTypeChangedEvent(EquipmentMaintenance maintenance)
        {
            Maintenance = maintenance;
            TypeId = maintenance.MaintenanceTypeId;
        }
    }
    
    public class MaintenancePerformerChangedEvent : DomainEvent
    {
        public EquipmentMaintenance Maintenance { get; }
        public Guid? PerformerId { get; }

        public MaintenancePerformerChangedEvent(EquipmentMaintenance maintenance)
        {
            Maintenance = maintenance;
            PerformerId = maintenance.PerformedById;
        }
    }
    
    public class MaintenanceCompletedEvent : DomainEvent
    {
        public EquipmentMaintenance Maintenance { get; }
        public DateTime CompletionDate { get; }

        public MaintenanceCompletedEvent(EquipmentMaintenance maintenance)
        {
            Maintenance = maintenance;
            CompletionDate = maintenance.CompletedDate ?? DateTime.UtcNow;
        }
    }
} 