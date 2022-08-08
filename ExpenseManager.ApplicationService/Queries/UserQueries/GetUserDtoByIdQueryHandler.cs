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
    public class GetUserDtoByIdQueryHandler : IRequestHandler<GetUserDtoByIdQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserDtoByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserDto> Handle(GetUserDtoByIdQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, query.Id);
            if (user == null)
                throw new ExpenseManagerApplicationServiceException("User not fround.");
            return UserDto.FromEntity(user,_mapper);
        }
    }
}
