using ExpenseManager.ApplicationService.Commands.UserCommands;
using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Utilities;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ExpenseManager.ApplicationService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<AddUserCommand>());
            services.AddTransient<IHttpContextService, HttpContextService>();
            services.AddTransient<ISecurityHelper, SecurityHelper>();
            return services;
        }
    }
}
