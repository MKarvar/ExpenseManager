using AutoMapper;
using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using ExpenseManager.ApplicationService.Exceptions;
using ExpenseManager.ApplicationService.Utilities;
using ExpenseManager.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseManager.ApplicationService.Commands.UserCommands
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ISecurityHelper _securityHelper;

        public AddUserCommandHandler(IUserRepository userRepository, IMapper mapper, ISecurityHelper securityHelper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _securityHelper = securityHelper ?? throw new ArgumentNullException(nameof(securityHelper));
        }
        public async Task<UserDto> Handle(AddUserCommand command, CancellationToken cancellationToken)
        {
            bool issDuplicatedUsername = await _userRepository.IsDuplicatedUsername(command.Username, cancellationToken);
            if (issDuplicatedUsername)
                throw new ExpenseManagerApplicationServiceException("Username is duplicated");

            var passwordHash = _securityHelper.GetSha256Hash(command.Password);
            User user = User.Create(command.Username, passwordHash);
            await _userRepository.AddAsync(user, cancellationToken);

            return UserDto.FromEntity(user, _mapper);
        }
    }
}
