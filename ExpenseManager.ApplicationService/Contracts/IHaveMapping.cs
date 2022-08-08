using AutoMapper;

namespace ExpenseManager.ApplicationService.Contracts
{
    public interface IHaveMapping
    {
        void CreateMappings(Profile profile);
    }
}
