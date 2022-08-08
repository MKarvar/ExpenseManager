
using ExpenseManager.ApplicationService.Commands.UserCommands;
using ExpenseManager.ApplicationService.Dtos;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using ExpenseManager.ApplicationService.Queries.UserQueries;
using ExpenseManager.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseManager.ApplicationService.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAndPass(string username, string password, CancellationToken cancellationToken);
        Task<bool> IsDuplicatedUsername(string username, CancellationToken cancellationToken);
        Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
        Task<IEnumerable<UserDto>> GetUsers(GetUsersQuery query, CancellationToken cancellationToken);
        Task<PagedResult<UserDto>> GetUsersWithPaging(GetUsersWithPagingQuery query, CancellationToken cancellationToken);
    }
}
