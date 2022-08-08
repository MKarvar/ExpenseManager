using ExpenseManager.ApplicationService.Dtos.UserDtos;
using FluentValidation;
using MediatR;

namespace ExpenseManager.ApplicationService.Commands.EventCommands
{
    public class AddParticipantCommand : IRequest<EventDto>
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
    }

    public class AddParticipantCommandValidator : AbstractValidator<AddParticipantCommand>
    {
        public AddParticipantCommandValidator()
        {
            RuleFor(c => c.UserId).NotNull();
            RuleFor(c => c.EventId).NotNull();
        }
    }
}
