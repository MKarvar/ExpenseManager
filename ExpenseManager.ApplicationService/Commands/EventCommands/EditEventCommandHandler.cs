using AutoMapper;
using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using ExpenseManager.ApplicationService.Exceptions;
using ExpenseManager.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseManager.ApplicationService.Commands.EventCommands
{
    public class EditEventCommandHandler : IRequestHandler<EditEventCommand, EventDto>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IHttpContextService _IdentityService;
        private readonly IMapper _mapper;
        public EditEventCommandHandler(IEventRepository eventRepository, IHttpContextService identityService, IMapper mapper)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _IdentityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<EventDto> Handle(EditEventCommand command, CancellationToken cancellationToken)
        {
            Event eventInstance = await _eventRepository.GetByIdAsync(cancellationToken, command.Id);

            ValidateCommand(eventInstance);

            var currentUserId = _IdentityService.GetUserId();
            Event.Edit(eventInstance, currentUserId, command.Name);

            await _eventRepository.UpdateAsync(eventInstance, cancellationToken);
            return EventDto.FromEntity(eventInstance, _mapper);
        }

        private static void ValidateCommand(Event eventInstance)
        {
            if (eventInstance == null)
                throw new ExpenseManagerApplicationServiceException("Event not found.");
        }
    }
}
