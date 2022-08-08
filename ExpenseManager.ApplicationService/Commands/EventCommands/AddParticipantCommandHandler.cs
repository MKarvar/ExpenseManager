using AutoMapper;
using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using ExpenseManager.ApplicationService.Exceptions;
using ExpenseManager.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseManager.ApplicationService.Commands.EventCommands
{
    public class AddParticipantCommandHandler : IRequestHandler<AddParticipantCommand, EventDto>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IHttpContextService _identityService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AddParticipantCommandHandler(IEventRepository eventRepository, IHttpContextService identityService, IUserRepository userRepository, IMapper mapper)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<EventDto> Handle(AddParticipantCommand command, CancellationToken cancellationToken)
        {
            var eventInstance = await _eventRepository.GetByIdIncludingChildrenAsync(command.EventId, cancellationToken);
            ValidateCommand(command, eventInstance);

            var eventParticipant = EventParticipant.Create(command.UserId, command.EventId);
            await _eventRepository.AddParticipantAsync(eventInstance, eventParticipant, cancellationToken);
            eventInstance = await _eventRepository.GetByIdAsync(cancellationToken, command.EventId);
            return EventDto.FromEntity(eventInstance, _mapper);
            //return _eventRepository.TableNoTracking.ProjectTo<EventDto>().SingleOrDefault(e => e.Id == eventInstance.Id);
        }

        private void ValidateCommand(AddParticipantCommand command, Event eventInstance)
        {
            if (eventInstance == null)
                throw new ExpenseManagerApplicationServiceException("Event not found.");
            if (eventInstance.EventParticipants.Any(p => p.ParticipantId == command.UserId))
                throw new ExpenseManagerApplicationServiceException("ParticipantId is duplicated.");
            var currentUserId = _identityService.GetUserId();
            if (eventInstance.CreatorId != currentUserId)
                throw new ExpenseManagerApplicationServiceException("Only event creator is allowed to add participants.");
            //following lines are commented because unauthorized or deactive user can't call the API actions 
            //var user = _userRepository.GetById(command.UserId);
            //if(user == null || !user.IsActive)
            //    throw new ExpenseManagerApplicationServiceException("No active user was found with this id.");
        }
    }
}
