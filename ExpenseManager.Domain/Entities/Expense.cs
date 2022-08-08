using ExpenseManager.Domain.Exceptions;
using ExpenseManager.Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Domain.Entities
{
    public class Expense : BaseEntity
    {
        public string Name { get; private set; }
        public DateTime CreationDateTime { get; private set; }
        public DateTime PayDateTime { get; private set; }
        public double TotalPrice { get; private set; }
        public int CreatorId { get; private set; }
        public User Creator { get; private set; }
        public int PayerId { get; private set; }
        public User Payer { get; private set; }
        public int CategoryId { get; private set; }
        public Category Category { get; private set; }
        public int EventId { get; private set; }
        public Event Event { get; private set; }

        public virtual ICollection<ExpensePartTaker> ExpensePartTakers { get; private set; }

        private Expense(string name, DateTime creationDateTime, DateTime payDateTime, double totalPrice, int payerId, int creatorId, 
            int categoryId, int eventId)
        {

            TotalPrice = totalPrice > 0 ? totalPrice :
                throw new ExpenseManagerDomainException("TotalPrice must be greater than 0");

            CreatorId = creatorId > 0 ? creatorId :
                throw new ExpenseManagerDomainException("CreatorId is not valid");

            PayerId = payerId > 0 ? payerId :
                throw new ExpenseManagerDomainException("PayerId is not valid");

            CategoryId = categoryId > 0 ? categoryId :
             throw new ExpenseManagerDomainException("CategoryId is not valid");

            EventId = eventId > 0 ? eventId :
             throw new ExpenseManagerDomainException("EventId is not valid");

            Name = !string.IsNullOrWhiteSpace(name) ? name.Trim() :
             throw new ExpenseManagerDomainException("Name is not valid");

            CreationDateTime = creationDateTime;
            PayDateTime = payDateTime;
            ExpensePartTakers = new List<ExpensePartTaker>();
        }

        public static Expense Create(string name, DateTime payDateTime, double totalPrice, int payerId, int creatorId, int categoryId, int eventId)
        {
            return new Expense(name, DateTime.Now, payDateTime, totalPrice, payerId, creatorId, categoryId, eventId);
        }
    }
}
