using ExpenseManager.ApplicationService.Dtos;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using FluentValidation;
using MediatR;
namespace ExpenseManager.ApplicationService.Queries.UserQueries
{
    public class GetUsersWithPagingQuery : PaginationQuery, IRequest<PagedResult<UserDto>>
    {
        public GetUsersQuery GetUsersQuery { get; set; }

        public class GetUsersWithPagingQueryValidator : AbstractValidator<GetUsersWithPagingQuery>
        {
            public GetUsersWithPagingQueryValidator(PaginationQueryValidator baseValidator)
            {
                RuleFor(c => c).SetValidator(baseValidator);
            }
        }
    }
}
