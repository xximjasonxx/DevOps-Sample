
using System;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using Newtonsoft.Json.Linq;
using UserApi.Data;
using UserApi.Data.Models;
using UserApi.Functions;
using Xunit;

namespace UserApi.Tests.Functions
{
    public class UserCreatedFunctionTests
    {
        [Fact]
        public void given_an_incoming_request_with_user_data_assert_the_adduser_method_is_called()
        {
            // arrange
            var userId = Guid.NewGuid();
            var dataProviderMock = new Mock<IDataProvider>();
            var userData = new EventGridEvent
            {
                Data = new JObject(
                    new JProperty("UserId", userId.ToString()),
                    new JProperty("Username", "testuser"),
                    new JProperty("FirstName", "Yang"),
                    new JProperty("LastName", "WenLi")
                )
            };

            var function = new UserCreatedFunction(dataProviderMock.Object);

            // act
            function.Run(userData, new Mock<ILogger>().Object).GetAwaiter().GetResult();

            // assert
            dataProviderMock.Verify(x => x.AddUserAsync(It.Is<User>(x1 => x1.UserId == userId)), Times.Once);
        }

        [Fact]
        public void given_an_incoming_request_with_no_user_data_assert_exception_is_logged()
        {
            // arrange
            var dataProviderMock = new Mock<IDataProvider>();
            var loggingMock = new Mock<ILogger>();
            var userData = new EventGridEvent
            {
                Data = new JObject(
                    new JProperty("ErrorMessage", "SomeError")
                )
            };

            var function = new UserCreatedFunction(dataProviderMock.Object);

            // act
            function.Run(userData, loggingMock.Object).GetAwaiter().GetResult();

            // assert
            loggingMock.Verify(x => x.Log(It.Is<LogLevel>(x1 => x1 == LogLevel.Error), 0,
                It.IsAny<FormattedLogValues>(),
                It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }

        [Fact]
        public void given_an_incoming_request_assert_exception_from_data_provider_call_is_logged_on_failure()
        {
            // arrange
            var dataProviderMock = new Mock<IDataProvider>();
            dataProviderMock.Setup(x => x.AddUserAsync(It.IsAny<User>())).ThrowsAsync(new InvalidOperationException("boom"));

            var loggingMock = new Mock<ILogger>();
            var userData = new EventGridEvent
            {
                Data = new JObject(
                    new JProperty("UserId", Guid.NewGuid().ToString()),
                    new JProperty("Username", "testuser"),
                    new JProperty("FirstName", "Anton"),
                    new JProperty("LastName", "Minovsky")
                )
            };

            var function = new UserCreatedFunction(dataProviderMock.Object);

            // act
            function.Run(userData, loggingMock.Object).GetAwaiter().GetResult();

            // assert
            loggingMock.Verify(x => x.Log(It.Is<LogLevel>(x1 => x1 == LogLevel.Error), 0,
                It.IsAny<FormattedLogValues>(),
                It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }
    }
}