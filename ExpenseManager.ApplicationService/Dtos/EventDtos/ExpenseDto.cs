using AutoMapper;
using ExpenseManager.ApplicationService.Utilities;
using ExpenseManager.Domain.Entities;
using System.Collections.Generic;

namespace ExpenseManager.ApplicationService.Dtos.EventDtos
{
    public class ExpenseDto : BaseDto<ExpenseDto, Expense>
    {
        public string Name { get; set; }
        public string CreationDateTime { get; set; }
        public string PayDateTime { get; set; }
        public double TotalPrice { get; set; }
        public int CreatorId { get; set; }
        public string CreatorUsername { get; set; }
        public int? PayerId { get; set; }
        public string PayerUsername { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public ICollection<ExpensePartTakerDto> ExpensePartTakers { get; set; }
        public override void CustomMappings(IMappingExpression<Expense, ExpenseDto> mappingExpression)
        {
            mappingExpression.ForMember(
                    dest => dest.CreationDateTime,
                    config => config.MapFrom(src => src.CreationDateTime.ConvertMiladiToShamsi()));
            mappingExpression.ForMember(
              dest => dest.PayDateTime,
              config => config.MapFrom(src => src.PayDateTime.ConvertMiladiToShamsi()));
        }
    }
}
