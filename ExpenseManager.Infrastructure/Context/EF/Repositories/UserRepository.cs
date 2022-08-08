using Common.Utilities;
using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.ApplicationService.Queries.UserQueries;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using System.Collections.Generic;
using AutoMapper.QueryableExtensions;
using ExpenseManager.ApplicationService.Dtos;

namespace ExpenseManager.Infrastructure.Context.EF.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ExpenseManagerDBContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<User> GetByUsernameAndPass(string username, string password, CancellationToken cancellationToken)
        {
            var passwordHash = SecurityHelper.GetSha256Hash(password);
            return await Table.Where(p => p.Username == username && p.PasswordHash == passwordHash).SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> IsDuplicatedUsername(string username, CancellationToken cancellationToken)
        {
            return await TableNoTracking.AnyAsync(p => p.Username == username, cancellationToken);
        }

        public async Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
        {
            User.UpdateLastLoginDate(user);
            await UpdateAsync(user, cancellationToken);
        }

        public async Task<IEnumerable<UserDto>> GetUsers(GetUsersQuery query, CancellationToken cancellationToken)
        {
            return await GetFilteredUsers(query).ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<UserDto>> GetUsersWithPaging(GetUsersWithPagingQuery query, CancellationToken cancellationToken)
        {
            IQueryable<UserDto> users = GetFilteredUsers(query.GetUsersQuery);

            Task<int> totalRecordsTask = users.CountAsync();
            Task<List<UserDto>> usersSlotTask = users.Skip((query.PageNumber - 1) * query.PageSize)
                              .Take(query.PageSize).ToListAsync(cancellationToken);

            await Task.WhenAll(totalRecordsTask, usersSlotTask);
            int totalRecords = await totalRecordsTask;
            List<UserDto> usersSlot = await usersSlotTask;

            return new PagedResult<UserDto>(query.PageSize, query.PageNumber, usersSlot, totalRecords);
        }

        private IQueryable<UserDto> GetFilteredUsers(GetUsersQuery query)
        {
            return TableNoTracking.ProjectTo<UserDto>().Where(u =>
                                           (string.IsNullOrWhiteSpace(query.Username) || u.Username.ToLower().Contains(query.Username.ToLower())) &&
                                           (!query.Id.HasValue || query.Id.Value <= 0 || u.Id == query.Id.Value));
        }
    }
}
