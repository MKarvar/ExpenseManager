using ExpenseManager.ApplicationService.Dtos.UserDtos;
using FluentValidation;
using MediatR;

namespace ExpenseManager.ApplicationService.Queries.UserQueries
{
    public class GetUserDtoByIdQuery : IRequest<UserDto>
    {
        public int Id { get; set; }
    }

    public class GetUserDtoByIdQueryValidator : AbstractValidator<GetUserDtoByIdQuery>
    {
        public GetUserDtoByIdQueryValidator()
        {
            RuleFor(c => c.Id).NotNull();
        }
    }
}
