using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class AddExpensePartTakerCommandHandler : IRequestHandler<AddExpensePartTakerCommand, EventDto>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextService _IdentityService;
        private readonly IMapper _mapper;

        public AddExpensePartTakerCommandHandler(IEventRepository eventRepository, IUserRepository userRepository, IHttpContextService identityService, IMapper mapper)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _IdentityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<EventDto> Handle(AddExpensePartTakerCommand command, CancellationToken cancellationToken)
        {
            var eventInstance = await _eventRepository.GetByIdIncludingChildrenAsync(command.EventId, cancellationToken);
            Expense eventExpense = ValidateCommand(command, eventInstance);

            var partTaker = ExpensePartTaker.Create(command.UserId, command.ExpenseId, command.ShareAmount);
            eventExpense.ExpensePartTakers.Add(partTaker);

            await _eventRepository.UpdateAsync(eventInstance, cancellationToken);
            return  EventDto.FromEntity(eventInstance, _mapper);
        }

        private Expense ValidateCommand(AddExpensePartTakerCommand command, Event eventInstance)
        {
            if (eventInstance == null)
                throw new ExpenseManagerApplicationServiceException("Event not found.");

            var eventExpense = eventInstance.EventExpenses.SingleOrDefault(p => p.Id == command.ExpenseId);
            if (eventExpense == null)
                throw new ExpenseManagerApplicationServiceException("EventExpense not found.");

            var currentUserId = _IdentityService.GetUserId();
            if (eventExpense.PayerId != currentUserId && eventExpense.CreatorId != currentUserId)
                throw new ExpenseManagerApplicationServiceException("Only expense payer or expense creator is allowed to add parttaker.");

            if (eventExpense.ExpensePartTakers.Any(pt => pt.PartTakerId == command.UserId))
                throw new ExpenseManagerApplicationServiceException("PartTakerId is duplicated.");

            if (eventExpense.ExpensePartTakers.Sum(pt => pt.ShareAmount) + command.ShareAmount > eventExpense.TotalPrice)
                throw new ExpenseManagerApplicationServiceException("Sum of share amounts is greater than total price");

            //following lines are commented because unauthorized or deactive user can't call the API actions 
            //var user = _userRepository.GetById(command.UserId);
            //if (user == null || !user.IsActive)
            //    throw new ExpenseManagerApplicationServiceException("No active user was found with this id.");
            return eventExpense;
        }
    }
}
