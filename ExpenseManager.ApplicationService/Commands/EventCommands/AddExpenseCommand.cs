using ExpenseManager.ApplicationService.Dtos.UserDtos;
using FluentValidation;
using MediatR;

namespace ExpenseManager.ApplicationService.Commands.EventCommands
{
    public class AddExpenseCommand : IRequest<EventDto>
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public double TotalPrice { get; set; }
        public int PayerId { get; set; }

    }

    public class AddExpenseCommandValidator : AbstractValidator<AddExpenseCommand>
    {
        public AddExpenseCommandValidator()
        {
            RuleFor(c => c.EventId).NotNull();
            RuleFor(c => c.CategoryId).NotNull();
            RuleFor(c => c.Name).NotNull().NotEmpty();
            RuleFor(c => c.TotalPrice).NotNull().GreaterThan(0);
            RuleFor(c => c.PayerId).NotNull();
        }
    }
}
