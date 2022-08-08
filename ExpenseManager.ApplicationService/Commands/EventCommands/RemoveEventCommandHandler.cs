using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Exceptions;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseManager.ApplicationService.Commands.EventCommands
{
    public class RemoveEventCommandHandler : IRequestHandler<RemoveEventCommand>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextService _IdentityService;

        public RemoveEventCommandHandler(IEventRepository eventRepository, IUserRepository userRepository, IHttpContextService identityService)
        //, IHttpContextAccessor httpContextAccessor)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _IdentityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        public async Task<Unit> Handle(RemoveEventCommand command, CancellationToken cancellationToken)
        {
            var eventInstance = await _eventRepository.GetByIdIncludingChildrenAsync(command.Id, cancellationToken);
            await ValidateCommand(eventInstance, cancellationToken);
            await _eventRepository.DeleteEventAndChildrenAsync(eventInstance, cancellationToken);
            return Unit.Value;
        }

        private async Task ValidateCommand(Domain.Entities.Event eventInstance, CancellationToken cancellationToken)
        {
            var currentUserId = _IdentityService.GetUserId();
            var user = await _userRepository.GetByIdAsync(cancellationToken, currentUserId);
            if (user == null)
                throw new ExpenseManagerApplicationServiceException("User not found.");

            if (eventInstance == null)
                throw new ExpenseManagerApplicationServiceException("Event not found.");

            if (eventInstance.CreatorId != currentUserId)
                throw new ExpenseManagerApplicationServiceException("Only event creator is allowed to remove it.");

            if (eventInstance.EventExpenses.Any(ex => ex.CreatorId != currentUserId))
                throw new ExpenseManagerApplicationServiceException("Event includes expenses created by other users and con't be deleted.");

            if (eventInstance.EventExpenses.Where(e => e.ExpensePartTakers.Where(p => p.PartTakerId != currentUserId && p.IsPaid).Any()).Any())
                throw new ExpenseManagerApplicationServiceException("Event includes paid expenses and can't be deleted.");
        }
    }
}
