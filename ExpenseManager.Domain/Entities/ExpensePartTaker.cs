
using ExpenseManager.Domain.Exceptions;
using ExpenseManager.Domain.SeedWork;
using System;

namespace ExpenseManager.Domain.Entities
{
    public class ExpensePartTaker : BaseEntity
    {
        public int PartTakerId { get; private set; }
        public virtual User PartTaker { get; private set; }
        public int ExpenseId { get; private set; }
        public virtual Expense Expense { get; private set; }
        public double ShareAmount { get; private set; }
        public bool IsPaid { get; private set; }
        public DateTime? SettleDateTime { get; private set; }

        private ExpensePartTaker(int partTakerId, int expenseId, double shareAmount)
        {
          
            PartTakerId = partTakerId > 0 ? partTakerId :
                   throw new ExpenseManagerDomainException("PartTakerId is not valid");

            ExpenseId = expenseId > 0 ? expenseId :
               throw new ExpenseManagerDomainException("ExpenseId is not valid");

            ShareAmount = shareAmount > 0 ? shareAmount :
              throw new ExpenseManagerDomainException("ShareAmount must be greater than 0");

            IsPaid = false;
        }

        public static ExpensePartTaker Create(int partTakerId, int expenseId, double shareAmount)
        {
            return new ExpensePartTaker(partTakerId, expenseId, shareAmount);
        }

        public static ExpensePartTaker Pay(ExpensePartTaker partTaker)
        {
            partTaker.IsPaid = true;
            return partTaker;
        }
    }
}
