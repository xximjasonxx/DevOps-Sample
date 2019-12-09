using AuthApi.Controllers;
using AuthApi.Data.Entities;
using AuthApi.Ex;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Common.EventModels;
using System.Threading.Tasks;

namespace AuthApi.Tests.Controllers
{
    public class UserController_CreateTests
    {
        [Fact]
        public void GivenRequestToCreateUser_CreateUserServiceMethodIsCalled()
        {
            // arrange
            var userCreateServiceMock = new Mock<IUserCreateService>();
            userCreateServiceMock.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new User()));
            var controller = new UserController(
                userCreateServiceMock.Object,
                new Mock<ICreateTokenService>().Object,
                new Mock<ITelemetryService>().Object,
                new Mock<IPublishEventService>().Object
            );

            // act
            controller.Create(new Models.UserCreateRequest()).GetAwaiter().GetResult();

            // assert
            userCreateServiceMock.Verify(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GivenRequestToCreateUser_ConflictResponseReturnedForDuplicateEmailAddress()
        {
            // arrange
            var userCreateServiceMock = new Mock<IUserCreateService>();
            userCreateServiceMock.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new DuplicateUserException("boom"));
            var controller = new UserController(
                userCreateServiceMock.Object,
                new Mock<ICreateTokenService>().Object,
                new Mock<ITelemetryService>().Object,
                new Mock<IPublishEventService>().Object
            );

            // act
            var result = controller.Create(new Models.UserCreateRequest()).GetAwaiter().GetResult() as ObjectResult;

            // assert
            Assert.NotNull(result);
            Assert.Equal(409, result.StatusCode);
        }

        [Fact]
        public void GivenRequestToCreateUser_CallToCreateWebTokenIsMade()
        {
            // arrange
            var createTokenMock = new Mock<ICreateTokenService>();
            var userCreateServiceMock = new Mock<IUserCreateService>();
            userCreateServiceMock.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new User { EmailAddress = "test@aol.com" });

            var controller = new UserController(
                userCreateServiceMock.Object,
                createTokenMock.Object,
                new Mock<ITelemetryService>().Object,
                new Mock<IPublishEventService>().Object
            );

            // act
            controller.Create(new Models.UserCreateRequest()).GetAwaiter().GetResult();

            // assert
            createTokenMock.Verify(x => x.CreateToken(It.Is<User>(x1 => x1.EmailAddress == "test@aol.com")), Times.Once);
        }

        [Fact]
        public void GivenRequestToCreateUser_CreatedTokenIsReturned()
        {
            // arrange
            var userCreateServiceMock = new Mock<IUserCreateService>();
            userCreateServiceMock.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new User()));
            var createTokenMock = new Mock<ICreateTokenService>();
            createTokenMock.Setup(x => x.CreateToken(It.IsAny<User>())).Returns("TestToken");
            var controller = new UserController(
                userCreateServiceMock.Object,
                createTokenMock.Object,
                new Mock<ITelemetryService>().Object,
                new Mock<IPublishEventService>().Object
            );

            // act
            var result = controller.Create(new Models.UserCreateRequest()).GetAwaiter().GetResult() as ObjectResult;

            // assert
            Assert.NotNull(result);
            Assert.Equal("TestToken", result.Value.ToString());
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public void GivenRequestToCreateUser_AssertCreatedEventPublished()
        {
            // arrange
            var userCreateServiceMock = new Mock<IUserCreateService>();
            userCreateServiceMock.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new User()));
            var publishEventMock = new Mock<IPublishEventService>();
            var controller = new UserController(
                userCreateServiceMock.Object,
                new Mock<ICreateTokenService>().Object,
                new Mock<ITelemetryService>().Object,
                publishEventMock.Object
            );

            // act
            controller.Create(new Models.UserCreateRequest { FirstName = "TestName" }).GetAwaiter().GetResult();

            // assert
            publishEventMock.Verify(x => x.PublishUserCreateEventAsync(It.Is<UserCreatedEvent>(x1 => x1.FirstName == "TestName")), Times.Once);
        }
    }
}