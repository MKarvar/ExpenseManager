using ExpenseManager.ApplicationService.Dtos.UserDtos;
using FluentValidation;
using MediatR;

namespace ExpenseManager.ApplicationService.Commands.EventCommands
{
    public class AddEventCommand : IRequest<EventDto>
    {
        public string Name { get; set; }
    }

    public class AddEventCommandValidator : AbstractValidator<AddEventCommand>
    {
        public AddEventCommandValidator()
        {
            RuleFor(c => c.Name).NotNull().NotEmpty();
        }
    }
}
