using AutoMapper;
using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using ExpenseManager.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseManager.ApplicationService.Commands.EventCommands
{
    public class AddEventCommandHandler : IRequestHandler<AddEventCommand, EventDto>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IHttpContextService _IdentityService;
        private readonly IMapper _mapper;

        public AddEventCommandHandler(IEventRepository eventRepository, IHttpContextService identityService, IMapper mapper)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _IdentityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<EventDto> Handle(AddEventCommand command, CancellationToken cancellationToken)
        {
            var currentUserId = _IdentityService.GetUserId();
            var eventInstance = Event.Create(command.Name, currentUserId);

            var eventParticipant = EventParticipant.Create(currentUserId, eventInstance.Id);
            eventInstance = Event.AddParticipant(eventInstance, eventParticipant);

            await _eventRepository.AddAsync(eventInstance, cancellationToken);

            return EventDto.FromEntity(eventInstance, _mapper);
        }
    }
}
