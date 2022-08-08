using ExpenseManager.Domain.Entities;
using System.Linq;


namespace ExpenseManager.Infrastructure.Context.EF
{
    public class DBInitializer
    {
        public static void Initialize(ExpenseManagerDBContext context)
        {

            context.Database.EnsureCreated();

            if (context.Set<Category>().Any())
            {
                return;  
            }
            context.Set<Category>().Add(Category.Create("Food"));
            context.Set<Category>().Add(Category.Create("Hotel"));
            context.Set<Category>().Add(Category.Create("FlightTicket"));

            context.SaveChanges();
        }
    }
}
