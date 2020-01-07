
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserApi.Data;
using UserApi.Data.Models;
using UserApi.Functions;
using Xunit;

namespace UserApi.Tests.Functions
{
    public class GetCurrentUserFunctionTests
    {
        [Fact]
        public void given_a_request_to_fetch_current_user_with_an_empty_token_a_badrequest_response_is_returned()
        {
            // arrange
            var dataProviderMock = new Mock<IDataProvider>();
            var function = new GetCurrentUserFunction(dataProviderMock.Object);
            
            // act
            var result = function.Run(new System.Net.Http.HttpRequestMessage(), new Mock<ILogger>().Object,
                new Framework.Binding.UserTokenResult { TokenState = Framework.Binding.TokenState.Empty }).GetAwaiter().GetResult();

            // assert
            var responseResult = result as BadRequestResult;
            Assert.NotNull(responseResult);
        }

        [Fact]
        public void given_a_request_to_fetch_current_user_with_a_nonvalid_token_an_unauthorized_response_is_returned()
        {
            // arrange
            var dataProviderMock = new Mock<IDataProvider>();
            var function = new GetCurrentUserFunction(dataProviderMock.Object);
            
            // act
            var result = function.Run(new System.Net.Http.HttpRequestMessage(), new Mock<ILogger>().Object,
                new Framework.Binding.UserTokenResult { TokenState = Framework.Binding.TokenState.Invalid }).GetAwaiter().GetResult();

            // assert
            var responseResult = result as UnauthorizedResult;
            Assert.NotNull(responseResult);
        }

        [Fact]
        public void given_a_request_to_fetch_current_user_with_a_valid_token_and_null_user_notfound_response_is_returned()
        {
            // arrange
            var dataProviderMock = new Mock<IDataProvider>();
            var function = new GetCurrentUserFunction(dataProviderMock.Object);

            // act
            var result = function.Run(new System.Net.Http.HttpRequestMessage(), new Mock<ILogger>().Object,
                new Framework.Binding.UserTokenResult { TokenState = Framework.Binding.TokenState.Valid }).GetAwaiter().GetResult();

            // assert
            var responseResult = result as NotFoundResult;
            Assert.NotNull(responseResult);
        }

        [Fact]
        public void given_a_request_to_fetch_current_user_with_a_valid_token_and_nonnull_user_ok_response_is_returned()
        {
            // arrange
            var dataProviderMock = new Mock<IDataProvider>();
            dataProviderMock.Setup(x => x.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(new User());
            var function = new GetCurrentUserFunction(dataProviderMock.Object);

            // act
            var result = function.Run(new System.Net.Http.HttpRequestMessage(), new Mock<ILogger>().Object,
                new Framework.Binding.UserTokenResult { TokenState = Framework.Binding.TokenState.Valid }).GetAwaiter().GetResult();

            // assert
            var responseResult = result as OkObjectResult;
            Assert.NotNull(responseResult);
        }
    }
}