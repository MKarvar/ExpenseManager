using ExpenseManager.Domain.Entities;
using System.Threading.Tasks;

namespace WebUtilities.JWT
{
    public interface IJwtHelper
    {
        AccessToken Generate(User user);
    }
}
