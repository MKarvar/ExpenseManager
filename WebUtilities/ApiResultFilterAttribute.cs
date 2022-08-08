using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;

namespace WebUtilities
{
    public class ApiResultFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            ApiResult apiResult;

            switch (context.Result)
            {
                case OkObjectResult okObjectResult:
                    apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, okObjectResult.Value, HttpStatusCode.OK);
                    break;
                case OkResult okResult:
                    apiResult = new ApiResult(true, ApiResultStatusCode.Success, HttpStatusCode.OK);
                    break;
                case BadRequestResult badRequestResult:
                    apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest, HttpStatusCode.BadRequest);
                    break;
                case BadRequestObjectResult badRequestObjectResult:
                    {
                        var message = badRequestObjectResult.Value.ToString();
                        if (badRequestObjectResult.Value.GetType() == typeof(ValidationProblemDetails))
                        {
                            var validationErrors = ((ValidationProblemDetails)badRequestObjectResult.Value).Errors;
                            var errorMessages = validationErrors.SelectMany(p => p.Value).Distinct();
                            message = string.Join(" | ", errorMessages);
                        }
                        else if (badRequestObjectResult.Value is SerializableError errors)
                        {
                            var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                            message = string.Join(" | ", errorMessages);
                        }
                        apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest, HttpStatusCode.BadRequest, message);
                        break;
                    }
                case ContentResult contentResult:
                    apiResult = new ApiResult(true, ApiResultStatusCode.Success, HttpStatusCode.OK, contentResult.Content);
                    break;
                case NotFoundResult notFoundResult:
                    apiResult = new ApiResult(false, ApiResultStatusCode.NotFound, HttpStatusCode.NotFound);
                    break;
                case NotFoundObjectResult notFoundObjectResult:
                    apiResult = new ApiResult<object>(false, ApiResultStatusCode.NotFound, notFoundObjectResult.Value, HttpStatusCode.NotFound);
                    break;
                case ForbidResult forbiddenResult:
                    apiResult = new ApiResult(false, ApiResultStatusCode.Forbidden, HttpStatusCode.Forbidden);
                    break;
                case ObjectResult objectResult when objectResult.StatusCode == null && !(objectResult.Value is ApiResult):
                    apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, objectResult.Value, HttpStatusCode.OK);
                    break;
                default:
                    apiResult = null;
                    break;
            }

            if (apiResult != null)
            {
                var jsonResult = new JsonResult(apiResult) { StatusCode = (int)apiResult.HttpStatusCode };
                context.Result = jsonResult;
            }

            base.OnResultExecuting(context);
        }
    }
}
