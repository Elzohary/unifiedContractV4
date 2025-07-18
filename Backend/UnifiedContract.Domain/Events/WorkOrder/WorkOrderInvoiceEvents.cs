using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Domain.Events.WorkOrder
{
    public class WorkOrderInvoiceCreatedEvent : DomainEvent
    {
        public WorkOrderInvoice Invoice { get; }

        public WorkOrderInvoiceCreatedEvent(WorkOrderInvoice invoice)
        {
            Invoice = invoice;
        }
    }
    
    public class WorkOrderInvoiceUpdatedEvent : DomainEvent
    {
        public WorkOrderInvoice Invoice { get; }

        public WorkOrderInvoiceUpdatedEvent(WorkOrderInvoice invoice)
        {
            Invoice = invoice;
        }
    }
    
    public class WorkOrderInvoicePaidEvent : DomainEvent
    {
        public WorkOrderInvoice Invoice { get; }
        public DateTime PaymentDate { get; }

        public WorkOrderInvoicePaidEvent(WorkOrderInvoice invoice, DateTime paymentDate)
        {
            Invoice = invoice;
            PaymentDate = paymentDate;
        }
    }
    
    public class WorkOrderInvoiceCanceledEvent : DomainEvent
    {
        public WorkOrderInvoice Invoice { get; }
        public string CancellationReason { get; }

        public WorkOrderInvoiceCanceledEvent(WorkOrderInvoice invoice, string cancellationReason)
        {
            Invoice = invoice;
            CancellationReason = cancellationReason;
        }
    }
} 