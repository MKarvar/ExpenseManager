
using ExpenseManager.Domain.Exceptions;
using ExpenseManager.Domain.SeedWork;

namespace ExpenseManager.Domain.Entities
{
    public class ExpensePayer : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User Payer { get; set; }
        public int ExpenseId { get; set; }
        public virtual Expense Expense { get; set; }
        private ExpensePayer(int userId, int expenseId)
        {
            UserId = userId >= 0 ? userId :
                   throw new ExpenseManagerDomainException("UserId is not valid");

            ExpenseId = expenseId >= 0 ? expenseId :
               throw new ExpenseManagerDomainException("ExpenseId is not valid");
        }

        public static ExpensePayer CreateEventPyer(int userId, int expenseId)
        {
            return new ExpensePayer(userId, expenseId);
        }
    }
}
