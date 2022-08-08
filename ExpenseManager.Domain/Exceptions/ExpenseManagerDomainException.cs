
using System;

namespace ExpenseManager.Domain.Exceptions
{
    public class ExpenseManagerDomainException : Exception
    {
        public ExpenseManagerDomainException()
        {

        }

        public ExpenseManagerDomainException(string errorMessage) : base(errorMessage)
        {

        }

        public ExpenseManagerDomainException(string errorMessage, Exception innerException) : base(errorMessage, innerException)
        {

        }
    }
}
