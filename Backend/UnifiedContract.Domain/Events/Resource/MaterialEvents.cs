using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Domain.Enums;

namespace UnifiedContract.Domain.Events.Resource
{
    #region Purchasable Material Events
    
    public class MaterialCreatedEvent : DomainEvent
    {
        public PurchasableMaterial Material { get; }

        public MaterialCreatedEvent(PurchasableMaterial material)
        {
            Material = material;
        }
    }
    
    public class MaterialUpdatedEvent : DomainEvent
    {
        public PurchasableMaterial Material { get; }

        public MaterialUpdatedEvent(PurchasableMaterial material)
        {
            Material = material;
        }
    }
    
    public class MaterialOrderedEvent : DomainEvent
    {
        public PurchasableMaterial Material { get; }
        public DateTime OrderDate { get; }

        public MaterialOrderedEvent(PurchasableMaterial material)
        {
            Material = material;
            OrderDate = material.OrderDate ?? DateTime.UtcNow;
        }
    }
    
    public class MaterialReceivedEvent : DomainEvent
    {
        public PurchasableMaterial Material { get; }
        public DateTime DeliveryDate { get; }

        public MaterialReceivedEvent(PurchasableMaterial material)
        {
            Material = material;
            DeliveryDate = material.DeliveryDate ?? DateTime.UtcNow;
        }
    }
    
    public class MaterialStatusChangedEvent : DomainEvent
    {
        public PurchasableMaterial Material { get; }
        public string Status { get; }

        public MaterialStatusChangedEvent(PurchasableMaterial material)
        {
            Material = material;
            Status = material.Status;
        }
    }
    
    #endregion
    
    #region Receivable Material Events
    
    public class ReceivableMaterialCreatedEvent : DomainEvent
    {
        public ReceivableMaterial Material { get; }

        public ReceivableMaterialCreatedEvent(ReceivableMaterial material)
        {
            Material = material;
        }
    }
    
    public class ReceivableMaterialUpdatedEvent : DomainEvent
    {
        public ReceivableMaterial Material { get; }

        public ReceivableMaterialUpdatedEvent(ReceivableMaterial material)
        {
            Material = material;
        }
    }
    
    public class ReceivableMaterialReceivedEvent : DomainEvent
    {
        public ReceivableMaterial Material { get; }
        public DateTime ReceivedDate { get; }
        public decimal ReceivedQuantity { get; }

        public ReceivableMaterialReceivedEvent(ReceivableMaterial material)
        {
            Material = material;
            ReceivedDate = material.ReceivedDate ?? DateTime.UtcNow;
            ReceivedQuantity = material.ReceivedQuantity ?? 0;
        }
    }
    
    public class ReceivableMaterialReturnedEvent : DomainEvent
    {
        public ReceivableMaterial Material { get; }
        public DateTime ReturnedDate { get; }
        public decimal ReturnedQuantity { get; }

        public ReceivableMaterialReturnedEvent(ReceivableMaterial material)
        {
            Material = material;
            ReturnedDate = material.ReturnedDate ?? DateTime.UtcNow;
            ReturnedQuantity = material.ReturnedQuantity ?? 0;
        }
    }
    
    public class ReceivableMaterialStatusChangedEvent : DomainEvent
    {
        public ReceivableMaterial Material { get; }
        public MaterialStatus Status { get; }

        public ReceivableMaterialStatusChangedEvent(ReceivableMaterial material)
        {
            Material = material;
            Status = material.Status;
        }
    }
    
    #endregion
} 