using ExpenseManager.Domain.Entities;
using FluentValidation;
using MediatR;

namespace ExpenseManager.ApplicationService.Queries.UserQueries
{
    public class GetUserByUsernameAndPasswordQuery : IRequest<User>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class GetUserByUsernameAndPasswordQueryValidator : AbstractValidator<GetUserByUsernameAndPasswordQuery>
    {
        public GetUserByUsernameAndPasswordQueryValidator()
        {
            RuleFor(c => c.Username).NotNull().NotEmpty();
            RuleFor(c => c.Password).NotNull().NotEmpty();
        }
    }
}
