using System;
using System.Text;
using AuthApi.Services.Impl;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace AuthApi.Tests.Services
{
    public class JwtCreateTokenService_CreateTokenTests
    {
        [Fact]
        public void GivenRequestToCreateToken_VerifyNullUserThrowsException()
        {
            // arrange
            var service = new JwtCreateTokenService(
                new Mock<IConfiguration>().Object
            );

            // act
            // assert
            Assert.Throws<ArgumentException>(() => service.CreateToken(null));
        }
    }
}