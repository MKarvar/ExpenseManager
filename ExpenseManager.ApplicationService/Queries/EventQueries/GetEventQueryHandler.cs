using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseManager.ApplicationService.Queries.EventQueries
{
    public class GetEventQueryHandler : IRequestHandler<GetEventQuery, IEnumerable<EventDto>>
    {
        private readonly IEventRepository _eventRepository;

        public GetEventQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        }

        public async Task<IEnumerable<EventDto>> Handle(GetEventQuery query, CancellationToken cancellationToken)
        {
            return await _eventRepository.GetEvents(query, cancellationToken);
        }

    }
}
