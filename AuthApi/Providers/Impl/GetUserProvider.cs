using System.Threading.Tasks;
using AuthApi.Data;
using AuthApi.Data.Entities;
using AuthApi.Services;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Providers.Impl
{
    public class GetUserProvider : IGetUserProvider
    {
        private readonly IUserDbContext _userDbContext;

        private readonly IPasswordHasher _passwordHasher;

        public GetUserProvider(IUserDbContext userDbContext, IPasswordHasher passwordHasher)
        {
            _userDbContext = userDbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> GetUserByAuthenticationCredentials(string emailAddress, string password)
        {
            var potentialUser = await _userDbContext.Users.FirstOrDefaultAsync(x => x.EmailAddress == emailAddress);
            if (potentialUser == null)
                return null;

            var passwordIsMatch = _passwordHasher.CheckPasswordHash(potentialUser.Password, password);
            if (passwordIsMatch == false)
                return null;

            return potentialUser;
        }
    }
}