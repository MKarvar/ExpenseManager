using AutoMapper;
using ExpenseManager.Domain.Entities;


namespace ExpenseManager.ApplicationService.Dtos.EventDtos
{
    public class EventParticipantDto : BaseDto<EventParticipantDto, EventParticipant>
    {
        public int ParticipantId { get; set; }
        public int EventId { get; set; }
        public string ParticipantUsername { get; set; }
        public string EventName { get; set; }
        public override void CustomMappings(IMappingExpression<EventParticipant, EventParticipantDto> mappingExpression)
        {
        }
    }
   
}
