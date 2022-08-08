using System;

namespace ExpenseManager.ApplicationService.Exceptions
{
 
    public class ExpenseManagerApplicationServiceException : Exception
    {
        public ExpenseManagerApplicationServiceException()
        {

        }

        public ExpenseManagerApplicationServiceException(string errorMessage) : base(errorMessage)
        {

        }

        public ExpenseManagerApplicationServiceException(string errorMessage, Exception innerException) : base(errorMessage, innerException)
        {

        }
    }
}
