using System;
using ExpenseManager.Domain.Exceptions;
using ExpenseManager.Domain.SeedWork;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseManager.Domain.Entities
{
    public class Event : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public DateTime CreationDateTime { get; private set; }
        public DateTime? LastUpdateDateTime { get; private set; }
        public int CreatorId { get; private set; }
        public User Creator { get; private set; }
        public int? LastUpdatorId { get; private set; }
        public User LastUpdator { get; private set; }
        public bool IsFinished { get; private set; }

        public ICollection<Expense> EventExpenses { get; private set; }
        public virtual ICollection<EventParticipant> EventParticipants { get; private set; }

        private Event(string name, DateTime creationDateTime, int creatorId)
        {
            CreatorId = creatorId >= 0 ? creatorId :
                throw new ExpenseManagerDomainException("CreatorId is not valid");

            Name = !string.IsNullOrWhiteSpace(name) ? name.Trim() :
                throw new ExpenseManagerDomainException("Name is not valid");

            CreationDateTime = creationDateTime;
            IsFinished = false;
            EventExpenses = new List<Expense>();
            EventParticipants = new List<EventParticipant>();
        }

        public static Event Create(string name, int creatorId)
        {
            return new Event(name, DateTime.Now, creatorId);
        }
        public static Event Edit(Event eventInstance, int updatorId, string name)
        {
            eventInstance.Name = name;
            eventInstance.LastUpdatorId = updatorId;
            eventInstance.LastUpdateDateTime = DateTime.Now;
            return eventInstance;
        }
        public static Event AddParticipant(Event eventInstance, EventParticipant participant)
        {
            var participants = new List<EventParticipant>() { participant };
            return AddParticipants(eventInstance, participants);
        }
        public static Event AddParticipants(Event eventInstance, ICollection<EventParticipant> participants)
        {
            eventInstance.EventParticipants = participants;
            return eventInstance;
        }
    }
}
