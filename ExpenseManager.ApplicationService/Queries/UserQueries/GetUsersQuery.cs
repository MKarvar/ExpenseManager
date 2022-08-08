using ExpenseManager.ApplicationService.Dtos.UserDtos;
using MediatR;
using System.Collections.Generic;
namespace ExpenseManager.ApplicationService.Queries.UserQueries
{
    public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
    {
        public string Username { get; set; }
        public int? Id { get; set; }
    }
}
