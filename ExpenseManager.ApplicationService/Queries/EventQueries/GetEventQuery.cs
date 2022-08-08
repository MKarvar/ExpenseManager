using ExpenseManager.ApplicationService.Dtos.UserDtos;
using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace ExpenseManager.ApplicationService.Queries.EventQueries
{
    public class GetEventQuery : IRequest<IEnumerable<EventDto>>
    {
        public int? CreatorId { get; set; }
        public int? ParticipantId { get; set; }
        public string Name { get; set; }
    }

    public class GetEventQueryValidator : AbstractValidator<GetEventQuery>
    {
    }
}
