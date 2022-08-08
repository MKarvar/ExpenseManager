using ExpenseManager.Domain.Exceptions;
using ExpenseManager.Domain.SeedWork;
using System.Collections.Generic;

namespace ExpenseManager.Domain.Entities
{
    public class Category : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public int? ParentCategoryId { get; private set; }
        public Category ParentCategory { get; private set; }
        public ICollection<Category> ChildCategories { get; private set; }
        public ICollection<Expense> Expenses { get; private set; }

        private Category(int id, string name, int? parentCategoryId)
        {
            if (id > 0)
                Id = id;

            if (parentCategoryId.HasValue && parentCategoryId.Value < 0)
                throw new ExpenseManagerDomainException("ParentCategoryId is not valid");
            else
                ParentCategoryId = parentCategoryId;

            Name = !string.IsNullOrWhiteSpace(name) ? name.Trim() :
                throw new ExpenseManagerDomainException("Name is not valid");
        }

        public static Category Create(string name, int? parentCategoryId = null)
        {
            return new Category(0, name, parentCategoryId);
        }

        // Need id parameter for seeding db
        public static Category Create(int id, string name, int? parentCategoryId = null)
        {
            return new Category(id, name, parentCategoryId);
        }
    }
}
