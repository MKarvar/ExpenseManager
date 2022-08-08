using Common.Utilities;
using System.Net;

namespace WebUtilities.Swagger
{
    public class AuthorizationError
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Error { get; set; }
        public string Error_Description { get; set; }

        public AuthorizationError(string errorDescription)
            : this(HttpStatusCode.BadRequest, errorDescription)
        {
        }

        public AuthorizationError(HttpStatusCode httpStatusCode, string errorDescription)
         : this(httpStatusCode, SecurityErrors.Invalid_Grant.ToDisplay(), errorDescription)
        {

        }

        public AuthorizationError(HttpStatusCode httpStatusCode, string error, string errorDescription)
        {
            Error = error;
            HttpStatusCode = httpStatusCode;
            Error_Description = errorDescription;
        }

    }

    public enum SecurityErrors
    {
        Invalid_Request, //wrong parameters
        Invalid_Client, //invalid client id or client secret
        Invalid_Grant, //invalid or expired user
        Invalid_Scope,
        Unauthorized_Client, //client is not authorized to use the requested grant type. 
        Unsupported_Grant_Type // requestde granttype is not recognized by server
    }
}
