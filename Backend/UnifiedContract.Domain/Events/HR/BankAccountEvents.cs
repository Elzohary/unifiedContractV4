using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Domain.Events.HR
{
    public class BankAccountAddedEvent : DomainEvent
    {
        public BankAccount BankAccount { get; }

        public BankAccountAddedEvent(BankAccount bankAccount)
        {
            BankAccount = bankAccount;
        }
    }

    public class BankAccountUpdatedEvent : DomainEvent
    {
        public BankAccount BankAccount { get; }

        public BankAccountUpdatedEvent(BankAccount bankAccount)
        {
            BankAccount = bankAccount;
        }
    }

    public class BankAccountSetAsPrimaryEvent : DomainEvent
    {
        public BankAccount BankAccount { get; }

        public BankAccountSetAsPrimaryEvent(BankAccount bankAccount)
        {
            BankAccount = bankAccount;
        }
    }

    public class BankAccountUnsetAsPrimaryEvent : DomainEvent
    {
        public BankAccount BankAccount { get; }

        public BankAccountUnsetAsPrimaryEvent(BankAccount bankAccount)
        {
            BankAccount = bankAccount;
        }
    }
} 