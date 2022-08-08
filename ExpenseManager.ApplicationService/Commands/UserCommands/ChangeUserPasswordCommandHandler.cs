using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Exceptions;
using ExpenseManager.ApplicationService.Utilities;
using ExpenseManager.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseManager.ApplicationService.Commands.UserCommands
{
    public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand>
    //AsyncRequestHandler<ChangeUserPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextService _IdentityService;
        private readonly ISecurityHelper _securityHelper;

        public ChangeUserPasswordCommandHandler(IUserRepository userRepository, IHttpContextService identityService, ISecurityHelper securityHelper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _IdentityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _securityHelper = securityHelper ?? throw new ArgumentNullException(nameof(securityHelper));
        }
        public async Task<Unit> Handle(ChangeUserPasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, _IdentityService.GetUserId());
            ValidateCommand(user);

            var oldPasswordHash = _securityHelper.GetSha256Hash(command.OldPassword);

            if (!user.PasswordHash.Equals(oldPasswordHash))
                throw new ExpenseManagerApplicationServiceException("OldPassword is not valid.");

            var newPasswordHash = _securityHelper.GetSha256Hash(command.NewPassword);
            User.ChangePassword(user, newPasswordHash);

            await _userRepository.UpdateAsync(user, cancellationToken);
            return Unit.Value;
        }

        private static void ValidateCommand(User user)
        {
            if (user == null)
                throw new ExpenseManagerApplicationServiceException("User not found.");
        }
    }
}
