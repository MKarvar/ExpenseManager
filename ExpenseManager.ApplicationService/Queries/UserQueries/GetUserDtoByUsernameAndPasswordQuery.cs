using ExpenseManager.ApplicationService.Dtos.UserDtos;
using FluentValidation;
using MediatR;

namespace ExpenseManager.ApplicationService.Queries.UserQueries
{
    public class GetUserDtoByUsernameAndPasswordQuery : IRequest<UserDto>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class GetUserDtoByUsernameAndPasswordQueryValidator : AbstractValidator<GetUserDtoByUsernameAndPasswordQuery>
    {
        public GetUserDtoByUsernameAndPasswordQueryValidator()
        {
            RuleFor(c => c.Username).NotNull().NotEmpty();
            RuleFor(c => c.Password).NotNull().NotEmpty();
        }
    }
}
