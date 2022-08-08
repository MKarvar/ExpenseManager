using System;

namespace ExpenseManager.Infrastructure.Exceptions
{
    public class ExpenseManagerInfrastructureException : Exception
    {
        public ExpenseManagerInfrastructureException()
        {

        }

        public ExpenseManagerInfrastructureException(string errorMessage) : base(errorMessage)
        {

        }

        public ExpenseManagerInfrastructureException(string errorMessage, System.Exception innerException) : base(errorMessage, innerException)
        {

        }
    }
}
