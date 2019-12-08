using System.Threading.Tasks;
using AuthApi.Data;
using AuthApi.Data.Entities;
using AuthApi.Ex;
using AuthApi.Providers;
using Common.EventModels;

namespace AuthApi.Services.Impl
{
    public class UserCreateService : IUserCreateService
    {
        private readonly IUserDbContext _userDbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IGetUserProvider _getUserProvider;
        private readonly IPublishEventService _publishEventService;

        public UserCreateService(IUserDbContext userDbContext, IPasswordHasher passwordHasher,
            IGetUserProvider getUserProvider, IPublishEventService publishEventService)
        {
            _userDbContext = userDbContext;
            _passwordHasher = passwordHasher;
            _getUserProvider = getUserProvider;
            _publishEventService = publishEventService;
        }

        public async Task<User> CreateUser(string emailAddress, string password)
        {
            var userExists = await _getUserProvider.GetUserByEmailAddress(emailAddress) != null;
            if (userExists)
                throw new DuplicateUserException();

            var newUser = new User
            {
                EmailAddress = emailAddress,
                Password = _passwordHasher.HashPassword(password)
            };

            await _userDbContext.Users.AddAsync(newUser);
            await _userDbContext.SaveChangesAsync();

            // todo: add real values
            await _publishEventService.PublishUserCreateEventAsync(new UserCreatedEvent
            {
                UserId = newUser.Id,
                Username = "testuser1",
                FirstName = "FirstName",
                LastName = "LastName"
            });

            return newUser;
        }
    }
}