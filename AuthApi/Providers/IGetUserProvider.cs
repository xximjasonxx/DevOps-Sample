using System.Threading.Tasks;
using AuthApi.Data.Entities;

namespace AuthApi.Providers
{
    public interface IGetUserProvider
    {
         Task<User> GetUserByAuthenticationCredentials(string emailAddress, string password);

         Task<User> GetUserByEmailAddress(string emailAddress);
    }
}