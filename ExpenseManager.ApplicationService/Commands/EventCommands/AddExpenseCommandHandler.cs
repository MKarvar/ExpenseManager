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
    public class AddExpenseCommandHandler : IRequestHandler<AddExpenseCommand, EventDto>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IHttpContextService _IdentityService;
        private readonly IMapper _mapper;
        public AddExpenseCommandHandler(IEventRepository eventRepository, IHttpContextService identityService, IMapper mapper)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _IdentityService = identityService ?? throw new ArgumentNullException(nameof(identityService)); 
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<EventDto> Handle(AddExpenseCommand command, CancellationToken cancellationToken)
        {
            var currentUserId = _IdentityService.GetUserId();

            var eventInstance = await _eventRepository.GetByIdAsync(cancellationToken, command.EventId);
            ValidateCommand(eventInstance);

            var expense = Expense.Create(command.Name, DateTime.Now, command.TotalPrice, command.PayerId, currentUserId, command.CategoryId, command.EventId);
            await _eventRepository.AddExpenceAsync(eventInstance, expense, cancellationToken);


            return EventDto.FromEntity(eventInstance, _mapper);
           // return _eventRepository.TableNoTracking.ProjectTo<EventDto>().SingleOrDefault(e => e.Id == eventInstance.Id);
        }

        private static void ValidateCommand(Event eventInstance)
        {
            if (eventInstance == null)
                throw new ExpenseManagerApplicationServiceException("Event not found.");
        }
    }
}
