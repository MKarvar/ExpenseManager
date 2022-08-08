using AutoMapper;
using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using ExpenseManager.ApplicationService.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseManager.ApplicationService.Queries.UserQueries
{
    public class GetUserDtoByUsernameAndPasswordQueryHandler : IRequestHandler<GetUserDtoByUsernameAndPasswordQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserDtoByUsernameAndPasswordQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserDto> Handle(GetUserDtoByUsernameAndPasswordQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAndPass(query.Username, query.Password, cancellationToken);
            if (user == null)
                throw new ExpenseManagerApplicationServiceException("User not fround.");
            return UserDto.FromEntity(user, _mapper);
        }
    }
}
