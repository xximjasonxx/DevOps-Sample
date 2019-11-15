using System.Threading.Tasks;
using AuthApi.Controllers;
using AuthApi.Data.Entities;
using AuthApi.Providers;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AuthApi.Tests.Controllers
{
    public class LoginController_LoginTests
    {
        [Fact]
        public void GivenRequestToLogin_IfUserIsNotFoundUnauthorizedReturned()
        {
            // arrange
            var getUserProviderMock = new Mock<IGetUserProvider>();
            getUserProviderMock.Setup(x => x.GetUserByAuthenticationCredentials(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<User>(null));

            var controller = new LoginController(
                getUserProviderMock.Object,
                new Mock<ICreateTokenService>().Object,
                new Mock<ITelemetryService>().Object
            );

            // act
            var result = controller.Login(new Models.LoginRequest()).GetAwaiter().GetResult() as ObjectResult;

            // assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public void GivenRequestToLogin_NonNullUserDeterminationReturnsOkResult()
        {
            // arrange
            var getUserProviderMock = new Mock<IGetUserProvider>();
            getUserProviderMock.Setup(x => x.GetUserByAuthenticationCredentials(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new User { EmailAddress = "test@aol.com" });

            var controller = new LoginController(
                getUserProviderMock.Object,
                new Mock<ICreateTokenService>().Object,
                new Mock<ITelemetryService>().Object
            );

            // act
            var result = controller.Login(new Models.LoginRequest()).GetAwaiter().GetResult() as ObjectResult;

            // assert
            Assert.NotNull(result);
            Assert.Equal(202, result.StatusCode);
        }

        [Fact]
        public void GivenRequestToLogin_VerifyTokenIsCreatedForValidLogin()
        {
            // arrange
            var getUserProviderMock = new Mock<IGetUserProvider>();
            getUserProviderMock.Setup(x => x.GetUserByAuthenticationCredentials(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new User { EmailAddress = "test@aol.com" });

            var createTokenMock = new Mock<ICreateTokenService>();
            var controller = new LoginController(
                getUserProviderMock.Object,
                createTokenMock.Object,
                new Mock<ITelemetryService>().Object
            );

            // act
            controller.Login(new Models.LoginRequest()).GetAwaiter().GetResult();

            // assert
            createTokenMock.Verify(x => x.CreateToken(It.Is<User>(x1 => x1.EmailAddress == "test@aol.com")), Times.Once);
        }

        [Fact]
        public void GivenRequestToLogin_VerifyTokenIsReturnedWithResponse()
        {
            // arrange
            var getUserProviderMock = new Mock<IGetUserProvider>();
            getUserProviderMock.Setup(x => x.GetUserByAuthenticationCredentials(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new User { EmailAddress = "test@aol.com" });

            var createTokenMock = new Mock<ICreateTokenService>();
            createTokenMock.Setup(x => x.CreateToken(It.IsAny<User>()))
                .Returns("TestToken");

            var controller = new LoginController(
                getUserProviderMock.Object,
                createTokenMock.Object,
                new Mock<ITelemetryService>().Object
            );

            // act
            var result = controller.Login(new Models.LoginRequest()).GetAwaiter().GetResult() as ObjectResult;

            // assert
            Assert.Equal("TestToken", result.Value.ToString());
        }
    }
}