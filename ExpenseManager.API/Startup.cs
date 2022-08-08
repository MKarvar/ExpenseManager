using WebUtilities.Middlewares;
using ExpenseManager.API.CustomExtensions;
using ExpenseManager.ApplicationService;
using ExpenseManager.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebUtilities;
using WebUtilities.Swagger;
using WebUtilities.PagingResult;
using AutoMapper;

namespace ExpenseManager.API
{

    public class Startup
    {
        private readonly SiteSettings _siteSettings;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AutoMapperConfiguration.InitializeAutoMapper();
            _siteSettings = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));//to be able to access sitesetting in dependency injection

            services.AddCustomCors()
                .AddCustomMvc()
                .AddCustomSwagger()
                .AddUriService()
                .AddInfrastructure(Configuration)
                .AddApplication()
                .AddAutoMapper(typeof(Startup))
                .AddJwtAuthentication(_siteSettings.JwtSettings);
        }

    
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomExceptionHandler();
            app.UseAuthentication();
            app.UseSwagger();
            app.UseCustomSwaggerAndUI();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseStaticFiles();
        }
    }
}
