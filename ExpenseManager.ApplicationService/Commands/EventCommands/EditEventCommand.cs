using ExpenseManager.ApplicationService.Dtos.UserDtos;
using FluentValidation;
using MediatR;
using System;

namespace ExpenseManager.ApplicationService.Commands.EventCommands
{
    public class EditEventCommand : IRequest<EventDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class EditEventCommandValidator : AbstractValidator<EditEventCommand>
    {
        public EditEventCommandValidator()
        {
            RuleFor(c => c.Id).NotNull();
            RuleFor(c => c.Name).NotNull().NotEmpty();
        }
    }
}
