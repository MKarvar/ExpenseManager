using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using Pluralize.NET;
using System.Linq;

namespace WebUtilities.Swagger
{
    public class ApplySummariesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null) return;

            var pluralizer = new Pluralizer();

            var actionName = controllerActionDescriptor.ActionName;
            var singularizeName = pluralizer.Singularize(controllerActionDescriptor.ControllerName);
            var article = IsStartedWithVowels(singularizeName) ? "an" : "a";
            var pluralizeName = pluralizer.Pluralize(singularizeName);

            var parameterCount = operation.Parameters.Where(p => p.Name != "version" && p.Name != "api-version").Count();

            if (IsGetAllAction())
            {
                if (string.IsNullOrWhiteSpace(operation.Summary))
                    operation.Summary = $"Returns all {pluralizeName}";
            }
            else if (IsActionName("Post", "Create" , "Insert"))
            {
                if (string.IsNullOrWhiteSpace(operation.Summary))
                    operation.Summary = $"Creates {article} {singularizeName}";

                if (string.IsNullOrWhiteSpace(operation.Parameters[0].Description))
                    operation.Parameters[0].Description = $"{article.ToUpper()} {singularizeName} representation";
            }
            else if (IsActionName("Read", "Get" , "Select"))
            {
                if (string.IsNullOrWhiteSpace(operation.Summary))
                    operation.Summary = $"Retrieves {article} {singularizeName} by unique id";

                if (string.IsNullOrWhiteSpace(operation.Parameters[0].Description))
                    operation.Parameters[0].Description = $"a unique id for the {singularizeName}";
            }
            else if (IsActionName("Put", "Edit", "Update"))
            {
                if (string.IsNullOrWhiteSpace(operation.Summary))
                    operation.Summary = $"Updates {article} {singularizeName} by unique id";

                //if (!operation.Parameters[0].Description.HasValue())
                //    operation.Parameters[0].Description = $"A unique id for the {singularizeName}";

                if (string.IsNullOrWhiteSpace(operation.Parameters[0].Description))
                    operation.Parameters[0].Description = $"{article.ToUpper()} {singularizeName} representation";
            }
            else if (IsActionName("Delete", "Remove"))
            {
                if (string.IsNullOrWhiteSpace(operation.Summary))
                    operation.Summary = $"Deletes {article} {singularizeName} by unique id";

                if (string.IsNullOrWhiteSpace(operation.Parameters[0].Description))
                    operation.Parameters[0].Description = $"A unique id for the {singularizeName}";
            }

            #region Local Functions
            bool IsGetAllAction()
            {
                foreach (var name in new[] { "Get", "Read", "Select" })
                {
                    if ((actionName.Equals(name, StringComparison.OrdinalIgnoreCase) && parameterCount == 0) ||
                        actionName.Equals($"{name}All", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Equals($"{name}{pluralizeName}", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Equals($"{name}All{singularizeName}", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Equals($"{name}All{pluralizeName}", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }

            bool IsActionName(params string[] names)
            {
                foreach (var name in names)
                {
                    if (actionName.Equals(name, StringComparison.OrdinalIgnoreCase) ||
                        actionName.Equals($"{name}ById", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Equals($"{name}{singularizeName}", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Equals($"{name}{singularizeName}ById", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }

            bool IsStartedWithVowels(string name)
            {
                string[] vowels = new []{ "u", "a", "i", "o", "e", "U", "A", "I", "O", "E" };
                return vowels.Contains(name.Substring(0, 1));
            }
            #endregion
        }
    }
}
