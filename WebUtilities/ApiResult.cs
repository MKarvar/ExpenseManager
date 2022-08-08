using Common.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;

namespace WebUtilities
{
    public class ApiResult 
    {
        public bool IsSuccess { get; set; }
        public ApiResultStatusCode StatusCode { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] //properties with a default value aren't included in the JSON result.
        public string Message { get; set; }

        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, HttpStatusCode httpStatusCode, string message = null) 
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Message = message ?? statusCode.ToDisplay();
            HttpStatusCode = httpStatusCode;
        }

        #region Implicit Operators
        public static implicit operator ApiResult(OkResult result)
        {
            return new ApiResult(true, ApiResultStatusCode.Success, HttpStatusCode.OK);
        }

        public static implicit operator ApiResult(OkObjectResult result)
        {
            return new ApiResult(true, ApiResultStatusCode.Success, HttpStatusCode.OK);
        }


        public static implicit operator ApiResult(BadRequestResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.BadRequest, HttpStatusCode.BadRequest);
        }

        public static implicit operator ApiResult(BadRequestObjectResult result)
        {
            var message = result.Value.ToString();
            if (result.Value.GetType() == typeof(ValidationProblemDetails))
            {
                var validationErrors = ((ValidationProblemDetails)result.Value).Errors;
                var errorMessages = validationErrors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            else if(result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            return new ApiResult(false, ApiResultStatusCode.BadRequest, HttpStatusCode.BadRequest, message);
        }

        public static implicit operator ApiResult(ContentResult result)
        {
            return new ApiResult(true, ApiResultStatusCode.Success, HttpStatusCode.OK, result.Content);
        }

        public static implicit operator ApiResult(NotFoundResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.NotFound, HttpStatusCode.NotFound);
        }

        public static implicit operator ApiResult(ForbidResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.Forbidden, HttpStatusCode.Forbidden);
        }
        #endregion
    }

    public class ApiResult<TData> : ApiResult
        where TData : class
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TData Data { get; set; }

        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, TData data, HttpStatusCode httpStatusCode, string message = null)
            : base(isSuccess, statusCode, httpStatusCode, message)
        {
            Data = data;
        }

        #region Implicit Operators
        public static implicit operator ApiResult<TData>(TData data)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success, data, HttpStatusCode.OK);
        }

        public static implicit operator ApiResult<TData>(OkResult result)
        {
            return  new ApiResult<TData>(true, ApiResultStatusCode.Success, null, HttpStatusCode.OK);
        }

        public static implicit operator ApiResult<TData>(OkObjectResult result)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success, (TData)result.Value, HttpStatusCode.OK);
        }

        public static implicit operator ApiResult<TData>(BadRequestResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null, HttpStatusCode.BadRequest);
        }

        public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
        {
            var message = result.Value.ToString();

            if (result.Value.GetType() == typeof(ValidationProblemDetails))
            {
                var validationErrors = ((ValidationProblemDetails)result.Value).Errors;
                var errorMessages = validationErrors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            else if (result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null, HttpStatusCode.BadRequest, message);
        }

        public static implicit operator ApiResult<TData>(ContentResult result)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success, null, HttpStatusCode.OK, result.Content);
        }

        public static implicit operator ApiResult<TData>(NotFoundResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.NotFound, null, HttpStatusCode.NotFound);
        }

        public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.NotFound, (TData)result.Value, HttpStatusCode.NotFound);
        }

        public static implicit operator ApiResult<TData>(ForbidResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.Forbidden, null, HttpStatusCode.Forbidden);
        }

        #endregion
    }
}
