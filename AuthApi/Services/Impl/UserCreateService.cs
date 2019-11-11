using System.Threading.Tasks;
using AuthApi.Data;
using AuthApi.Data.Entities;

namespace AuthApi.Services.Impl
{
    public class UserCreateService : IUserCreateService
    {
        private readonly IUserDbContext _userDbContext;

        public UserCreateService(IUserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<User> CreateUser(string emailAddress, string password)
        {
            var newUser = new User
            {
                EmailAddress = emailAddress,
                Password = password
            };

            await _userDbContext.Users.AddAsync(newUser);
            await _userDbContext.SaveChangesAsync();

            return newUser;
        }
    }
}