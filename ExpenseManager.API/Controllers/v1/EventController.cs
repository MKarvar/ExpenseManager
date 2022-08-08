using WebUtilities;
using ExpenseManager.ApplicationService.Commands.EventCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using Microsoft.Extensions.Logging;
using ExpenseManager.ApplicationService.Queries.EventQueries;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseManager.API.Controllers.v1
{
    [ApiVersion("1")]
    public class EventController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EventController> _logger;
        private readonly IHttpContextService _identityService;

        private int _currentUserId => _identityService.GetUserId();
        private string _currentUserName => _identityService.GetUserName();
     
        public EventController(IMediator iMediator, ILogger<EventController> iLogger, IHttpContextService identityService)
        {
            _mediator = iMediator ?? throw new ArgumentNullException(nameof(iMediator));
            _logger = iLogger ?? throw new ArgumentNullException(nameof(iLogger));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        [HttpPost("[action]")]
        public virtual async Task<ApiResult<EventDto>> Create(AddEventCommand command, CancellationToken cancellationToken)
        {
            var eventDto = await _mediator.Send(command, cancellationToken);
            _logger.LogInformation($"Event with name {eventDto.Id} created by user {_currentUserName}");
            return Ok(eventDto);
        }


        [HttpPost("[action]")]
        public virtual async Task<ApiResult<EventDto>> Edit(EditEventCommand command, CancellationToken cancellationToken)
        {
            var eventDto = await _mediator.Send(command, cancellationToken);
            _logger.LogInformation($"Event with id {command.Id} edited by user {_currentUserName}");
            return Ok(eventDto);
        }

        [HttpPost("[action]")]
        public virtual async Task<ApiResult> Remove(RemoveEventCommand command, CancellationToken cancellationToken)
        {
          
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation($"Event with id {command.Id} deleted by user {_currentUserName}");
            return Ok();
        }

        [HttpPost("[action]")]
        public virtual async Task<ApiResult<EventDto>> AddParticipant(AddParticipantCommand command, CancellationToken cancellationToken)
        {
            var eventDto = await _mediator.Send(command, cancellationToken);
            _logger.LogInformation($"Participant with Id {command.UserId} added to the event with id {command.EventId} by user {_currentUserName}");
            return Ok(eventDto);
        }

        [HttpPost("[action]")]
        public virtual async Task<ApiResult<EventDto>> AddExpense(AddExpenseCommand command, CancellationToken cancellationToken)
        {
            var eventDto = await _mediator.Send(command, cancellationToken);
            _logger.LogInformation($"Expense with name {command.Name} added to the event with id {command.EventId} by user {_currentUserName}");
            return Ok(eventDto);
        }

        [HttpPost("[action]")]
        public virtual async Task<ApiResult<EventDto>> AddExpensePartTaker(AddExpensePartTakerCommand command, CancellationToken cancellationToken)
        {
            var eventDto = await _mediator.Send(command, cancellationToken);
            _logger.LogInformation($"PartTaker with Id {command.UserId} added to the event with id {command.EventId} by user {_currentUserName}");
            return Ok(eventDto);
        }


        [HttpPost("[action]")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<IEnumerable<EventDto>>> Get(GetEventQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<EventDto> events = await _mediator.Send(query, cancellationToken);
            return Ok(events);
        }
    }
}
