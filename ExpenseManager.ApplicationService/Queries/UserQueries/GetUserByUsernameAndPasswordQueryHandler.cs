using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseManager.ApplicationService.Queries.UserQueries
{
    public class GetUserByUsernameAndPasswordQueryHandler : IRequestHandler<GetUserByUsernameAndPasswordQuery, User>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByUsernameAndPasswordQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<User> Handle(GetUserByUsernameAndPasswordQuery query, CancellationToken cancellationToken)
        {
           return await _userRepository.GetByUsernameAndPass(query.Username, query.Password, cancellationToken);
        }
    }
}
