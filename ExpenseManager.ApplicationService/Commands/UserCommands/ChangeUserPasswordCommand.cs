
using FluentValidation;
using MediatR;

namespace ExpenseManager.ApplicationService.Commands.UserCommands
{
    public class ChangeUserPasswordCommand : IRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
    public class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
    {

        public ChangeUserPasswordCommandValidator()
        {
            var passwordRulesMessage = "Password should contains at least one lowercase letter, one uppercase letter, one digit and one special character";
            RuleFor(c => c.NewPassword).NotNull()
                .MinimumLength(8).WithMessage("Min password length is 8 character.")
                .Matches("[A-Z]").WithMessage(passwordRulesMessage)
                .Matches("[a-z]").WithMessage(passwordRulesMessage)
                .Matches("[0-9]").WithMessage(passwordRulesMessage)
                .Matches("[^a-zA-Z0-9]").WithMessage(passwordRulesMessage);
            RuleFor(c => c.ConfirmNewPassword).NotNull()
               .MinimumLength(8).WithMessage("Min password length is 8 character.")
               .Matches("[A-Z]").WithMessage(passwordRulesMessage)
               .Matches("[a-z]").WithMessage(passwordRulesMessage)
               .Matches("[0-9]").WithMessage(passwordRulesMessage)
               .Matches("[^a-zA-Z0-9]").WithMessage(passwordRulesMessage);
            RuleFor(c => c.ConfirmNewPassword).Equal(c => c.NewPassword)
                .WithMessage(c => $"{nameof(c.NewPassword)} and {nameof(c.ConfirmNewPassword)} are not equal");
        }
    }
}
