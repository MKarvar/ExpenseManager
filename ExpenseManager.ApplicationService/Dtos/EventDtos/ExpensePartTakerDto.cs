using AutoMapper;
using ExpenseManager.Domain.Entities;
using ExpenseManager.ApplicationService.Utilities;


namespace ExpenseManager.ApplicationService.Dtos.EventDtos
{
    public class ExpensePartTakerDto : BaseDto<ExpensePartTakerDto, ExpensePartTaker>
    {
        public int PartTakerId { get; set; }
        public string PartTakerUsername { get; set; }
        public int ExpenseId { get; set; }
        public string ExpenseName { get; set; }
        public double ShareAmount { get; set; }
        public bool IsPaid { get; set; }
        public string SettleDateTime { get; set; }
        public override void CustomMappings(IMappingExpression<ExpensePartTaker, ExpensePartTakerDto> mappingExpression)
        {
            mappingExpression.ForMember(
                    dest => dest.SettleDateTime,
                    config => config.MapFrom(src => src.SettleDateTime.ConvertMiladiToShamsi()));
        }
    }
}
