using ExpenseManager.ApplicationService.Dtos.UserDtos;
using FluentValidation;
using MediatR;

namespace ExpenseManager.ApplicationService.Commands.UserCommands
{
    public class AddUserCommand : IRequest<UserDto>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator()
        {
            var passwordRulesMessage = "Password should contains at least one lowercase letter, one uppercase letter, one digit and one special character";
            RuleFor(c => c.Username).NotNull()
                .MinimumLength(4).WithMessage("Min username length is 4 character.");
            RuleFor(c => c.Password).NotNull()
                .MinimumLength(8).WithMessage("Min password length is 8 character.")
                .Matches("[A-Z]").WithMessage(passwordRulesMessage)
                .Matches("[a-z]").WithMessage(passwordRulesMessage)
                .Matches("[0-9]").WithMessage(passwordRulesMessage)
                .Matches("[^a-zA-Z0-9]").WithMessage(passwordRulesMessage);
        }
    }
}
