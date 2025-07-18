using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Domain.Events.HR
{
    public class CertificateAddedEvent : DomainEvent
    {
        public Certificate Certificate { get; }

        public CertificateAddedEvent(Certificate certificate)
        {
            Certificate = certificate;
        }
    }

    public class CertificateUpdatedEvent : DomainEvent
    {
        public Certificate Certificate { get; }

        public CertificateUpdatedEvent(Certificate certificate)
        {
            Certificate = certificate;
        }
    }

    public class CertificateVerifiedEvent : DomainEvent
    {
        public Certificate Certificate { get; }

        public CertificateVerifiedEvent(Certificate certificate)
        {
            Certificate = certificate;
        }
    }

    public class CertificateRejectedEvent : DomainEvent
    {
        public Certificate Certificate { get; }

        public CertificateRejectedEvent(Certificate certificate)
        {
            Certificate = certificate;
        }
    }

    public class CertificateExpiringEvent : DomainEvent
    {
        public Certificate Certificate { get; }
        public int DaysUntilExpiry { get; }

        public CertificateExpiringEvent(Certificate certificate, int daysUntilExpiry)
        {
            Certificate = certificate;
            DaysUntilExpiry = daysUntilExpiry;
        }
    }
} 