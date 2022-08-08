using AutoMapper;
using ExpenseManager.ApplicationService.Utilities;
using ExpenseManager.Domain.Entities;
using System;

namespace ExpenseManager.ApplicationService.Dtos.UserDtos
{
    public class UserDto : BaseDto<UserDto, User>
    {
        public string Username { get; set; }
        public string RegistrationDateTime { get; set; }
        public bool IsActive { get; set; }
        public string LastLoginDate { get; set; }
        public string LastUpdatedDate { get; set; }

        public override void CustomMappings(IMappingExpression<User, UserDto> mappingExpression)
        {
            mappingExpression.ForMember(
                    dest => dest.RegistrationDateTime,
                    config => config.MapFrom(src => src.RegistrationDateTime.ConvertMiladiToShamsi()));
            mappingExpression.ForMember(
                    dest => dest.LastLoginDate,
                    config => config.MapFrom(src => src.LastLoginDate.ConvertMiladiToShamsi()));
            mappingExpression.ForMember(
                    dest => dest.LastUpdatedDate,
                    config => config.MapFrom(src => src.LastUpdatedDate.ConvertMiladiToShamsi()));
        }
    }
}
