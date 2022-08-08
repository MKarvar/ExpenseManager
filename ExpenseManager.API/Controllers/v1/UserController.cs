using WebUtilities;
using ExpenseManager.ApplicationService.Commands.UserCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using ExpenseManager.ApplicationService.Queries.UserQueries;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using System.Collections.Generic;


namespace ExpenseManager.API.Controllers.v1
{
    [ApiVersion("1")]
    public class UserController : BaseApiController
    {
        protected readonly IMediator _mediator;
        protected readonly ILogger<UserController> _logger;
        protected readonly IHttpContextService _identityService;
        private int _currentUserId => _identityService.GetUserId();
        private string _currentUserName => _identityService.GetUserName();
        public UserController(IMediator iMediator, ILogger<UserController> iLogger, IHttpContextService identityService)
        {
            _mediator = iMediator ?? throw new ArgumentNullException(nameof(iMediator));
            _logger = iLogger ?? throw new ArgumentNullException(nameof(iLogger));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        [HttpPost("[action]")]
        public virtual async Task<ApiResult<UserDto>> Create(AddUserCommand command, CancellationToken cancellationToken)
        {
            //var currentUserName = HttpContext.User.Identity.GetUserName();
            var userDto = await _mediator.Send(command, cancellationToken);
            _logger.LogInformation($"User with username {command.Username} created by user {_currentUserName}");
            return Ok(userDto);
        }

        [HttpPost("[action]")]
        [NonAction]
        public virtual async Task<ApiResult<UserDto>> GetByUsernameAndPassword(GetUserDtoByUsernameAndPasswordQuery query, CancellationToken cancellationToken)
        {
            UserDto userDto = await _mediator.Send(query, cancellationToken);
            return Ok(userDto);
        }

        [HttpPost("[action]")]
        public virtual async Task<ApiResult<UserDto>> GetById(GetUserDtoByIdQuery query, CancellationToken cancellationToken)
        {
            UserDto userDto = await _mediator.Send(query, cancellationToken);
            return Ok(userDto);
        }


        [HttpPost("[action]")]
        public virtual async Task<ApiResult<UserDto>> UpdatePassword(ChangeUserPasswordCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            _logger.LogInformation($"User with id {_currentUserId} changed his/her password.");
            return Ok();
        }

        [HttpPost("[action]")]
        public virtual async Task<ApiResult<IEnumerable<UserDto>>> GetAll([FromForm]GetUsersQuery query, CancellationToken cancellationToken)
        {
            var usersDtos = await _mediator.Send(query, cancellationToken);
            return Ok(usersDtos);
        }
    }
}
