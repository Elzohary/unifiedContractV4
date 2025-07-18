using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.Resource;

namespace UnifiedContract.Domain.Events.Resource
{
    public class SupplierCreatedEvent : DomainEvent
    {
        public Supplier Supplier { get; }

        public SupplierCreatedEvent(Supplier supplier)
        {
            Supplier = supplier;
        }
    }
    
    public class SupplierUpdatedEvent : DomainEvent
    {
        public Supplier Supplier { get; }

        public SupplierUpdatedEvent(Supplier supplier)
        {
            Supplier = supplier;
        }
    }
    
    public class SupplierStatusChangedEvent : DomainEvent
    {
        public Supplier Supplier { get; }
        public bool IsActive { get; }

        public SupplierStatusChangedEvent(Supplier supplier, bool isActive)
        {
            Supplier = supplier;
            IsActive = isActive;
        }
    }
    
    public class SupplierCategoryChangedEvent : DomainEvent
    {
        public Supplier Supplier { get; }
        public Guid? CategoryId { get; }

        public SupplierCategoryChangedEvent(Supplier supplier)
        {
            Supplier = supplier;
            CategoryId = supplier.CategoryId;
        }
    }
    
    public class SupplierRatingUpdatedEvent : DomainEvent
    {
        public Supplier Supplier { get; }
        public decimal Rating { get; }

        public SupplierRatingUpdatedEvent(Supplier supplier)
        {
            Supplier = supplier;
            Rating = supplier.Rating;
        }
    }
} 