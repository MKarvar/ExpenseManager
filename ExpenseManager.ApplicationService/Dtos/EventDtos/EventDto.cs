using AutoMapper;
using ExpenseManager.ApplicationService.Dtos.EventDtos;
using ExpenseManager.ApplicationService.Utilities;
using ExpenseManager.Domain.Entities;
using System.Collections.Generic;

namespace ExpenseManager.ApplicationService.Dtos.UserDtos
{
    public class EventDto : BaseDto<EventDto, Event>
    {
        public string Name { get; set; }
        public string CreationDateTime { get; set; }
        public string LastUpdateDateTime { get; set; }
        public int CreatorId { get; set; }
        public string CreatorUsername { get; set; }
        public int? LastUpdatorId { get; set; }
        public string LastUpdatorUsername { get; set; }
        public bool IsFinished { get; set; }
        public ICollection<ExpenseDto> EventExpenses { get; set; }
        public virtual ICollection<EventParticipantDto> EventParticipants { get; set; }

        public override void CustomMappings(IMappingExpression<Event, EventDto> mappingExpression)
        {
            mappingExpression.ForMember(
                    dest => dest.CreationDateTime,
                    config => config.MapFrom(src => src.CreationDateTime.ConvertMiladiToShamsi()));
            mappingExpression.ForMember(
              dest => dest.LastUpdateDateTime,
              config => config.MapFrom(src => src.LastUpdateDateTime.ConvertMiladiToShamsi()));
        }
    }
}
