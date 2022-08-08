using ExpenseManager.ApplicationService.Commands.UserCommands;
using ExpenseManager.ApplicationService.Dtos;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using ExpenseManager.ApplicationService.Queries;
using ExpenseManager.ApplicationService.Queries.UserQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebUtilities;
using WebUtilities.PagingResult;

namespace ExpenseManager.API.Controllers.v2
{
    [ApiVersion("2")]
    public class UserController : v1.UserController
    {
        private readonly IUriService _uriService;
        public UserController(IMediator iMediator, ILogger<UserController> iLogger, IHttpContextService identityService, IUriService uriService) 
            : base(iMediator, iLogger, identityService)
        {
            _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
        }

        [HttpPost("[action]")]
        public override async Task<ApiResult<UserDto>> Create(AddUserCommand command, CancellationToken cancellationToken)
        {
            return await base.Create(command, cancellationToken);
        }

       
        [NonAction]
        public override async Task<ApiResult<UserDto>> GetByUsernameAndPassword(GetUserDtoByUsernameAndPasswordQuery query, CancellationToken cancellationToken)
        {
            return await base.GetByUsernameAndPassword(query, cancellationToken);
        }


        [HttpPost("[action]")]
        public override async Task<ApiResult<UserDto>> GetById(GetUserDtoByIdQuery query, CancellationToken cancellationToken)
        {
            return await base.GetById(query, cancellationToken);
        }


        [HttpPost("[action]")]
        public override async Task<ApiResult<UserDto>> UpdatePassword(ChangeUserPasswordCommand command, CancellationToken cancellationToken)
        {
            return await base.UpdatePassword(command, cancellationToken);
        }

        [HttpPost("[action]")]
        [NonAction]
        public override async Task<ApiResult<IEnumerable<UserDto>>> GetAll(GetUsersQuery query, CancellationToken cancellationToken)
        {
            return await base.GetAll(query, cancellationToken);
        }


        [HttpPost("[action]")]
        public virtual async Task<ApiResult<PagedResult<UserDto>>> GetAll(GetUsersQuery userQuery, [FromQuery] PaginationQuery pagingQuery, CancellationToken cancellationToken)
        {
            var route = Request.Path.Value;
            GetUsersWithPagingQuery query = new GetUsersWithPagingQuery() { GetUsersQuery = userQuery, PageNumber = pagingQuery.PageNumber, PageSize = pagingQuery.PageSize };
            var usersDtos = await _mediator.Send(query, cancellationToken);
            var pagedReponse = PaginationHelper.CreatePagedReponse(usersDtos.DataList, pagingQuery, usersDtos.TotalRecords, _uriService, route);
            return Ok(pagedReponse);
        }
    }
}
