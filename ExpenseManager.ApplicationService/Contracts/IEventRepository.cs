using ExpenseManager.ApplicationService.Dtos.UserDtos;
using ExpenseManager.ApplicationService.Queries.EventQueries;
using ExpenseManager.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseManager.ApplicationService.Contracts
{
    public interface IEventRepository : IRepository<Event>
    {
        Task AddParticipantAsync(Event eventInstance, EventParticipant participant, CancellationToken cancellationToken, bool saveNow = true);
        Task AddExpenceAsync(Event eventInstance, Expense expense, CancellationToken cancellationToken, bool saveNow = true);
        Task AddExpencePartTakerAsync(Expense expense, ExpensePartTaker expensePartTaker, CancellationToken cancellationToken, bool saveNow = true);
        Task<Expense> GetExpenseByIdAsync(CancellationToken cancellationToken, int eventId, int expenseId);
        Task<IEnumerable<EventDto>> GetEvents(GetEventQuery eventQuery, CancellationToken cancellationToken);
        Task<Event> GetByIdIncludingChildrenAsync(int eventId, CancellationToken cancellationToken);
        Task DeleteEventAndChildrenAsync(Event eventInstance, CancellationToken cancellationToken, bool saveNow = true);
    }
}