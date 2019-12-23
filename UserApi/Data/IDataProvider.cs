using System.Threading.Tasks;
using UserApi.Data.Models;

namespace UserApi.Data
{
    public interface IDataProvider
    {
         Task AddUserAsync(User user);
         Task<User> GetUserByUsername(string username);
    }
}