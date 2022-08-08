using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using WebUtilities.Swagger;

namespace ExpenseManager.API.CustomExtensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            return services;
        }

        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {

            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter());//set all controllers methods to need authorization by default
                options.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
            })
             .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
             .AddControllersAsServices();

            return services;
        }
    }
}
