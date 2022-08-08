using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.Infrastructure.Context.EF;
using ExpenseManager.Infrastructure.Context.EF.Repositories;

namespace ExpenseManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient(typeof(IEventRepository), typeof(EventRepository));
            services.AddTransient(typeof(IUserRepository), typeof(UserRepository));
            //EF :
            services.AddDbContext<ExpenseManagerDBContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ExpenseManagerDBContext).Assembly.FullName)));

            return services;
        }
    }
}
