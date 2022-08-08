using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Dtos;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseManager.ApplicationService.Queries.UserQueries
{
    public class GetUsersWithPagingQueryHandler : IRequestHandler<GetUsersWithPagingQuery, PagedResult<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersWithPagingQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<PagedResult<UserDto>> Handle(GetUsersWithPagingQuery query, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUsersWithPaging(query, cancellationToken);
        }

    }
}
