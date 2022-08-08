using AutoMapper.QueryableExtensions;
using Common.Utilities;
using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using ExpenseManager.ApplicationService.Queries.EventQueries;
using ExpenseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseManager.Infrastructure.Context.EF.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        private DbSet<Expense> ExpenseEntities { get; }
        //public virtual IQueryable<Expense> ExpenseTable => ExpenseEntities;
        //public virtual IQueryable<Expense> ExpenseTableNoTracking => ExpenseEntities.AsNoTracking();

        private DbSet<EventParticipant> EventParticipantEntities { get; }
        private DbSet<ExpensePartTaker> ExpensePartTakerEntities { get; }

        public EventRepository(ExpenseManagerDBContext dbContext)
            : base(dbContext)
        {
            ExpenseEntities = DbContext.Set<Expense>();
            EventParticipantEntities = DbContext.Set<EventParticipant>();
            ExpensePartTakerEntities = DbContext.Set<ExpensePartTaker>();
        }

        public async Task AddParticipantAsync(Event eventInstance, EventParticipant participant, CancellationToken cancellationToken, bool saveNow = true)
        {
            eventInstance.EventParticipants.Add(participant);
            await base.UpdateAsync(eventInstance, cancellationToken);
            if (saveNow)
                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task AddExpenceAsync(Event eventInstance, Expense expense, CancellationToken cancellationToken, bool saveNow = true)
        {
            eventInstance.EventExpenses.Add(expense);
            await base.UpdateAsync(eventInstance, cancellationToken);
            if (saveNow)
                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task AddExpencePartTakerAsync(Expense expense, ExpensePartTaker expenseParticipant, CancellationToken cancellationToken, bool saveNow = true)
        {
            Event eventInstance = expense.Event;
            expense.ExpensePartTakers.Add(expenseParticipant);
            await base.UpdateAsync(eventInstance, cancellationToken);
            if (saveNow)
                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Expense> GetExpenseByIdAsync(CancellationToken cancellationToken, int eventId, int expenseId)
        {
            var eventInstance = await GetByIdAsync(cancellationToken, eventId);
            return eventInstance.EventExpenses.Where(e => e.Id == expenseId).FirstOrDefault();
        }

        public async Task<IEnumerable<EventDto>> GetEvents(GetEventQuery eventQuery, CancellationToken cancellationToken)
        {
            return await TableNoTracking.ProjectTo<EventDto>().Where(v =>
                                  //(string.IsNullOrWhiteSpace(eventQuery.Name) || v.Name.Contains(eventQuery.Name, StringComparison.OrdinalIgnoreCase)) &&
                                  //(string.IsNullOrWhiteSpace(eventQuery.Name) || v.Name.IndexOf(eventQuery.Name, StringComparison.OrdinalIgnoreCase) >= 0) &&
                                  (string.IsNullOrWhiteSpace(eventQuery.Name) || v.Name.ToLower().Contains(eventQuery.Name.ToLower())) &&
                                  (!eventQuery.CreatorId.HasValue || eventQuery.CreatorId.Value <= 0 || v.CreatorId == eventQuery.CreatorId.Value) &&
                                  (!eventQuery.ParticipantId.HasValue || eventQuery.ParticipantId.Value <= 0 || v.EventParticipants.Any(p => p.Id == eventQuery.ParticipantId.Value))
            ).ToListAsync(cancellationToken);
        }

        public async Task<Event> GetByIdIncludingChildrenAsync(int eventId, CancellationToken cancellationToken)
        {
            return await Table.Include(e => e.EventParticipants).Include(e => e.EventExpenses).ThenInclude(ex => ex.ExpensePartTakers).SingleOrDefaultAsync(e => e.Id == eventId);
        }

        public async Task DeleteExpensesAsync(IEnumerable<Expense> expenses, CancellationToken cancellationToken, bool saveNow = true)
        {
            Assert.NotNull(expenses, nameof(expenses));
            ExpenseEntities.RemoveRange(expenses);
            if (saveNow)
                await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteExpensePartTakersAsync(IEnumerable<ExpensePartTaker> partTakers, CancellationToken cancellationToken, bool saveNow = true)
        {
            Assert.NotNull(partTakers, nameof(partTakers));
            ExpensePartTakerEntities.RemoveRange(partTakers);
            if (saveNow)
                await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteEventParticipantsAsync(IEnumerable<EventParticipant> participants, CancellationToken cancellationToken, bool saveNow = true)
        {
            Assert.NotNull(participants, nameof(participants));
            EventParticipantEntities.RemoveRange(participants);
            if (saveNow)
                await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteEventAndChildrenAsync(Event eventInstance, CancellationToken cancellationToken, bool saveNow = true)
        {
            IEnumerable<ExpensePartTaker> partTakers = eventInstance.EventExpenses.SelectMany(ex => ex.ExpensePartTakers);
            await DeleteExpensePartTakersAsync(partTakers, cancellationToken,false);
            await DeleteExpensesAsync(eventInstance.EventExpenses, cancellationToken, false); 
            await DeleteEventParticipantsAsync(eventInstance.EventParticipants, cancellationToken, false);
            await DeleteAsync(eventInstance, cancellationToken, saveNow);
        }
    }
}
