
using FluentValidation;

namespace ExpenseManager.ApplicationService.Queries
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; } 
        public int PageSize { get; set; }
        public class PaginationQueryValidator : AbstractValidator<PaginationQuery>
        {
            public PaginationQueryValidator()
            {
                RuleFor(c => c.PageNumber).GreaterThan(0).WithMessage("Page number must be greater than 0.");
                RuleFor(c => c.PageSize).GreaterThan(9).WithMessage("Page size must be greater than 9.");
            }
        }
    }
}