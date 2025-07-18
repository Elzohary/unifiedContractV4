using System;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.ValueObjects
{
    public class Money
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = "SAR"; // Default to Saudi Riyal

        // Constructors
        private Money() { } // For EF Core

        public Money(decimal amount, string currency = "SAR")
        {
            if (amount < 0)
                throw new DomainException("Amount cannot be negative");
                
            if (string.IsNullOrWhiteSpace(currency))
                throw new DomainException("Currency is required");
                
            if (currency.Length != 3)
                throw new DomainException("Currency must be a 3-letter ISO code");
                
            Amount = amount;
            Currency = currency.ToUpper();
        }
        
        public static Money FromSAR(decimal amount)
        {
            return new Money(amount);
        }
        
        public Money Add(Money other)
        {
            if (other.Currency != Currency)
                throw new DomainException($"Cannot add money with different currencies: {Currency} and {other.Currency}");
                
            return new Money(Amount + other.Amount, Currency);
        }
        
        public Money Subtract(Money other)
        {
            if (other.Currency != Currency)
                throw new DomainException($"Cannot subtract money with different currencies: {Currency} and {other.Currency}");
                
            return new Money(Amount - other.Amount, Currency);
        }
        
        public Money Multiply(decimal factor)
        {
            return new Money(Amount * factor, Currency);
        }

        // Equality overrides
        public override bool Equals(object obj)
        {
            if (obj is not Money money)
                return false;

            return Amount == money.Amount && 
                   Currency == money.Currency;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }

        public static bool operator ==(Money left, Money right)
        {
            if (left is null)
                return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(Money left, Money right)
        {
            return !(left == right);
        }
        
        public static Money operator +(Money left, Money right)
        {
            return left.Add(right);
        }
        
        public static Money operator -(Money left, Money right)
        {
            return left.Subtract(right);
        }
        
        public static Money operator *(Money left, decimal right)
        {
            return left.Multiply(right);
        }

        // Convert to string
        public override string ToString()
        {
            return $"{Amount:N2} {Currency}";
        }
    }
} 