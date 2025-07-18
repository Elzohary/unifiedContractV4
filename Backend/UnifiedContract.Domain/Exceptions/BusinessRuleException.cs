using System;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.Exceptions
{
    public class BusinessRuleException : DomainException
    {
        public BusinessRuleException()
        {
        }

        public BusinessRuleException(string message)
            : base(message)
        {
        }

        public BusinessRuleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
} 