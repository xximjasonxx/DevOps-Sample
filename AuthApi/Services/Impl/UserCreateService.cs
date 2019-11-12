using System.Threading.Tasks;
using AuthApi.Data;
using AuthApi.Data.Entities;

namespace AuthApi.Services.Impl
{
    public class UserCreateService : IUserCreateService
    {
        private readonly IUserDbContext _userDbContext;
        private readonly IPasswordHasher _passwordHasher;

        public UserCreateService(IUserDbContext userDbContext, IPasswordHasher passwordHasher)
        {
            _userDbContext = userDbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> CreateUser(string emailAddress, string password)
        {
            var newUser = new User
            {
                EmailAddress = emailAddress,
                Password = _passwordHasher.HashPassword(password)
            };

            await _userDbContext.Users.AddAsync(newUser);
            await _userDbContext.SaveChangesAsync();

            return newUser;
        }
    }
}