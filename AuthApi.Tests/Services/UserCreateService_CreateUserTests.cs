using System.Threading;
using System.Threading.Tasks;
using AuthApi.Data;
using AuthApi.Data.Entities;
using AuthApi.Ex;
using AuthApi.Providers;
using AuthApi.Services;
using AuthApi.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace AuthApi.Tests.Services
{
    public class UserCreateService_CreateUserTests
    {
        [Fact]
        public void GivenRequestToCreateUser_VerifyDuplicateuserExceptionRaisedForDuplicateEmail()
        {
            // arrange
            var getUserProviderMock = new Mock<IGetUserProvider>();
            getUserProviderMock.Setup(x => x.GetUserByEmailAddress(It.IsAny<string>()))
                .ReturnsAsync(new User());

            var service = new UserCreateService(
                new Mock<IUserDbContext>().Object,
                new Mock<IPasswordHasher>().Object,
                getUserProviderMock.Object
                
            );

            // act
            // assert
            Assert.ThrowsAsync<DuplicateUserException>(() => service.CreateUser("test@aol.com", "test", "test"));
        }

        [Fact]
        public void GivenRequestToCreateUser_VerifyUserPasswordIsHashed()
        {
            // arrange
            var getUserProviderMock = new Mock<IGetUserProvider>();
            getUserProviderMock.Setup(x => x.GetUserByEmailAddress(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(null));

            var usersDbSetMock = new Mock<DbSet<User>>();
            var userDbContextMock = new Mock<IUserDbContext>();
            userDbContextMock.Setup(x => x.Users)
                .Returns(usersDbSetMock.Object);

            var passwordHasherMock = new Mock<IPasswordHasher>();
            var service = new UserCreateService(
                userDbContextMock.Object,
                passwordHasherMock.Object,
                getUserProviderMock.Object
            );

            // act
            service.CreateUser("test@aol.com", "test", "test").GetAwaiter().GetResult();

            // assert
            passwordHasherMock.Verify(x => x.HashPassword("test"), Times.Once);
        }

        [Fact]
        public void GivenRequestToCreateUser_VerifyUserObjectAddedToContextAndSaved()
        {
            // arrange
            var getUserProviderMock = new Mock<IGetUserProvider>();
            getUserProviderMock.Setup(x => x.GetUserByEmailAddress(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(null));

            var usersDbSetMock = new Mock<DbSet<User>>();
            var userDbContextMock = new Mock<IUserDbContext>();
            userDbContextMock.Setup(x => x.Users)
                .Returns(usersDbSetMock.Object);

            var passwordHasherMock = new Mock<IPasswordHasher>();
            var service = new UserCreateService(
                userDbContextMock.Object,
                passwordHasherMock.Object,
                getUserProviderMock.Object
            );

            // act
            service.CreateUser("test@aol.com", "test", "test").GetAwaiter().GetResult();

            // assert
            userDbContextMock.Verify(x => x.Users.AddAsync(It.Is<User>(x1 => x1.EmailAddress == "test@aol.com"), default(CancellationToken)), Times.Once);
        }

        [Fact]
        public void GiventRequestToCreateUser_VerifyDuplicateUsernameThrowsException()
        {
            // arrange
            var getUserProviderMock = new Mock<IGetUserProvider>();
            getUserProviderMock.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(Task.FromResult(new User()));
            getUserProviderMock.Setup(x => x.GetUserByEmailAddress(It.IsAny<string>())).Returns(Task.FromResult<User>(null));

            var usersDbSetMock = new Mock<DbSet<User>>();
            var userDbContextMock = new Mock<IUserDbContext>();
            userDbContextMock.Setup(x => x.Users)
                .Returns(usersDbSetMock.Object);

            var service = new UserCreateService(
                userDbContextMock.Object,
                new Mock<IPasswordHasher>().Object,
                getUserProviderMock.Object
            );

            // act
            // assert
            Assert.ThrowsAsync<DuplicateUserException>(() => service.CreateUser("test@aol.com", "test", "test"));
        }
    }
}