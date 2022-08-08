using Common.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebUtilities.Swagger
{
    public static class SwaggerConfigurationExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {

            Assert.NotNull(services, nameof(services));
            //Add services to use Example Filters in swagger
            //services.AddSwaggerExamples();
            services.AddSwaggerGen(options =>
            {
                var xmlDocPath = Path.Combine(AppContext.BaseDirectory, "Comments.xml");
                options.IncludeXmlComments(xmlDocPath, true);

                options.EnableAnnotations();
                options.UseInlineDefinitionsForEnums();
                options.IgnoreObsoleteActions();
                options.IgnoreObsoleteProperties();

                options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "API V1" });
                options.SwaggerDoc("v2", new OpenApiInfo { Version = "v2", Title = "API V2" });

                #region Filters
                ////Enable to use [SwaggerRequestExample] & [SwaggerResponseExample]
                //options.ExampleFilters();

                //Set summary of action if not already set
                options.OperationFilter<ApplySummariesOperationFilter>();

                //Add 401 response and security requirements (Lock icon) to actions that need authorization
                options.OperationFilter<GeneralResponsesOperationFilter>(true, "Bearer");
                #endregion

                #region Add Jwt Authentication & Oauth
                //Add Lockout icon on top of swagger ui page to authenticate
                var hostUrl = "https://localhost:44312/api";

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    // BearerFormat = "JWT",
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri(hostUrl + "/v1/Security/GetToken"),
                            //Scopes = new Dictionary<string, string>
                            //{
                            //    { "wholeAPI", "Access to entire API" },
                            //}
                        }

                    }
                });
                #endregion

                #region Versioning
                // Remove version parameter from all Operations
                options.OperationFilter<RemoveVersionParameters>();

                //set version "api/v{version}/[controller]" from current swagger doc verion
                options.DocumentFilter<SetVersionInPaths>();

                //Seperate and categorize end-points by doc version
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var versions = methodInfo.DeclaringType
                        .GetCustomAttributes<ApiVersionAttribute>(true)
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v.ToString()}" == docName);
                });
                #endregion

            });
            services.AddFluentValidationRulesToSwagger();
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
            services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            return services;
        }

        public static void UseCustomSwaggerAndUI(this IApplicationBuilder app)
        {
            Assert.NotNull(app, nameof(app));
            //Swagger middleware for generate "Open API Documentation" in swagger.json
            app.UseSwagger(options =>
            {
                //options.RouteTemplate = "api-docs/{documentName}/swagger.json";
            });

            //Swagger middleware for generate UI from swagger.json
            app.UseSwaggerUI(options =>
            {
                #region Customizing
                options.DocExpansion(DocExpansion.None);
                options.OAuthClientId("SampleClientId");
                options.OAuthClientSecret("SampleClientSecret");
                //options.EnableFilter();
                //options.ShowExtensions();
                //options.EnableValidator();
                options.InjectStylesheet("/swagger/custom.css");

                #endregion
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "V2 Docs");
            });
        }
    }
}
