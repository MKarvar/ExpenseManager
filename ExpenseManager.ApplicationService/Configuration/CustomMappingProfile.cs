using AutoMapper;
using ExpenseManager.ApplicationService.Contracts;
using System.Collections.Generic;

namespace ExpenseManager.ApplicationService
{
    public class CustomMappingProfile : Profile
    {
        public CustomMappingProfile(IEnumerable<IHaveMapping> haveCustomMappings)
        {
            foreach (var item in haveCustomMappings)
                item.CreateMappings(this);
        }
    }
}
