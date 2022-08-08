using FluentValidation;
using MediatR;

namespace ExpenseManager.ApplicationService.Commands.EventCommands
{
    public class RemoveEventCommand :  IRequest
    {
        public int Id { get; set; }
    }

    public class RemoveEventCommandValidator : AbstractValidator<RemoveEventCommand>
    {
        public RemoveEventCommandValidator()
        {
            RuleFor(c => c.Id).NotNull();
        }
    }
}
