using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebUtilities.Swagger
{
    public class GeneralResponsesOperationFilter : IOperationFilter
    {
        private readonly bool includeUnauthorizedAndForbiddenResponses;
        private readonly string schemeName;

        public GeneralResponsesOperationFilter(bool includeUnauthorizedAndForbiddenResponses, string schemeName = "Bearer")
        {
            this.includeUnauthorizedAndForbiddenResponses = includeUnauthorizedAndForbiddenResponses;
            this.schemeName = schemeName;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Responses.TryAdd("400", new OpenApiResponse { Description = "Bad Request" });
            operation.Responses.TryAdd("404", new OpenApiResponse { Description = "Not Found" });
            operation.Responses.TryAdd("500", new OpenApiResponse { Description = "Internal Server Error" });

            var filters = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var metaData = context.ApiDescription.ActionDescriptor.EndpointMetadata;

            bool hasAllowAnonymous = metaData.Any(em => em.GetType() == typeof(AllowAnonymousAttribute)) || filters.Any(p => p.Filter is AllowAnonymousFilter);
            if (hasAllowAnonymous)
                return;

            bool hasAuthorize = metaData.Any(em => em.GetType() == typeof(AuthorizeAttribute)) || filters.Any(p => p.Filter is AuthorizeFilter);
            if (!hasAuthorize)
                return;

            if (includeUnauthorizedAndForbiddenResponses)
            {
                operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });
            }

            var securityRequirement = new OpenApiSecurityRequirement{{
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = schemeName },
                Scheme = schemeName,
                Name = schemeName,
                In = ParameterLocation.Header
            },
            new string[]  { }} };

            operation.Security = new List<OpenApiSecurityRequirement>() { securityRequirement };
           
        }
    }
}
