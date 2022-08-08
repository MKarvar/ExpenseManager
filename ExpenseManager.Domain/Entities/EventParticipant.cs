
using ExpenseManager.Domain.Exceptions;
using ExpenseManager.Domain.SeedWork;

namespace ExpenseManager.Domain.Entities
{
    public class EventParticipant : BaseEntity
    {
        public int ParticipantId { get; private set; }
        public int EventId { get; private set; }
        public virtual User Participant { get; private set; }
        public virtual Event Event { get; private set; }

        private EventParticipant(int participantId, int eventId)
        {
            ParticipantId = participantId > 0 ? participantId :
               throw new ExpenseManagerDomainException("participantId is not valid");

            //when we are adding a new event with a defaut participant eventid is equal zero
            EventId = eventId >= 0 ? eventId :
               throw new ExpenseManagerDomainException("EventId is not valid");
        }

        public static EventParticipant Create(int participantId, int eventId)
        {
            return new EventParticipant(participantId, eventId);
        }
    }
}
