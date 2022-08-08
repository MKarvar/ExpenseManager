using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseManager.ApplicationService.Queries.UserQueries
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUsers(query, cancellationToken);
        }

    }
}
