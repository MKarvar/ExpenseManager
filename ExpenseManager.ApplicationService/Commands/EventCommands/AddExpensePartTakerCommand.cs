using ExpenseManager.ApplicationService.Dtos.UserDtos;
using FluentValidation;
using MediatR;

namespace ExpenseManager.ApplicationService.Commands.EventCommands
{
    public class AddExpensePartTakerCommand : IRequest<EventDto>
    {
        public int EventId { get; set; }
        public int ExpenseId { get; set; }
        public int UserId { get; set; }
        public double ShareAmount { get; set; }
    }

    public class AddExpensePartTakerCommandValidator : AbstractValidator<AddExpensePartTakerCommand>
    {
        public AddExpensePartTakerCommandValidator()
        {
            RuleFor(c => c.UserId).NotNull();
            RuleFor(c => c.EventId).NotNull();
            RuleFor(c => c.ExpenseId).NotNull();
            RuleFor(c => c.ShareAmount).GreaterThan(0);
        }
    }
}
