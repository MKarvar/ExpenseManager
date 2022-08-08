using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ExpenseManager.ApplicationService.Queries.UserQueries;
using ExpenseManager.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using WebUtilities.JWT;
using WebUtilities.Swagger;
using ExpenseManager.ApplicationService.Queries.SecurityQueries;

namespace ExpenseManager.API.Controllers.v1
{
    [ApiVersion("1")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SecurityController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SecurityController> _logger;
        private readonly IJwtHelper _jwtHelper;

        public SecurityController(IMediator iMediator, ILogger<SecurityController> iLogger, IJwtHelper iJwtHelper)
        {
            _mediator = iMediator ?? throw new ArgumentNullException(nameof(iMediator));
            _logger = iLogger ?? throw new ArgumentNullException(nameof(iLogger));
            _jwtHelper = iJwtHelper ?? throw new ArgumentNullException(nameof(iJwtHelper));
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
 
        public virtual async Task<ActionResult> GetToken([FromForm] TokenRequest tokenQuery, CancellationToken cancellationToken)
        {
            var authorizationException = ValidateTokenQuery(tokenQuery);
            if (authorizationException != null)
                return new JsonResult(authorizationException);

            var query = new GetUserByUsernameAndPasswordQuery() { Username = tokenQuery.username, Password = tokenQuery.password };
            User user = await _mediator.Send(query, cancellationToken);
            if (user == null || !user.IsActive)
                return new JsonResult(new AuthorizationError("No active user was found with this username and password."));

            var jwt = _jwtHelper.Generate(user);
            var result = new JsonResult(jwt);
            return result;
        }

        private AuthorizationError ValidateTokenQuery(TokenRequest tokenQuery)
        {
            if (string.IsNullOrWhiteSpace(tokenQuery.grant_type))
                return new AuthorizationError("Requested OAuth flow is not correct.");
            if (string.IsNullOrWhiteSpace(tokenQuery.username))
                return new AuthorizationError("Username must not be empty.");
            if (string.IsNullOrWhiteSpace(tokenQuery.password))
                return new AuthorizationError("Password must not be empty.");
            if (!tokenQuery.grant_type.Equals("Password", StringComparison.OrdinalIgnoreCase))
                return new AuthorizationError("Requested OAuth flow is not correct.");

            return null;
        }

    }
}
