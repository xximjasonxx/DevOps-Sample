using System.Threading.Tasks;
using AuthApi.Data.Entities;

namespace AuthApi.Services
{
    public interface IUserCreateService
    {
         Task<User> CreateUser(string emailAddress, string password);
    }
}