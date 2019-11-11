using System.Threading.Tasks;
using AuthApi.Data.Entities;

namespace AuthApi.Services.Impl
{
    public class UserCreateService : IUserCreateService
    {
        public Task<User> CreateUser(string emailAddress, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}