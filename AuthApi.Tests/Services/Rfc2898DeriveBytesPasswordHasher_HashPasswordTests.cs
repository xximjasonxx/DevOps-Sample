using System;
using AuthApi.Services.Impl;
using Xunit;

namespace AuthApi.Tests.Services
{
    public class Rfc2898DeriveBytesPasswordHasher_HashPasswordTests
    {
        [Fact]
        public void GivenRequestToHashPassword_VerifyEmptyPasswordRaisesException()
        {
            // arrange
            var service = new Rfc2898DeriveBytesPasswordHasher();

            // act
            // assert
            Assert.Throws<ArgumentException>(() => service.HashPassword(string.Empty));
        }

        [Fact]
        public void GivenRequestToHashPassword_VerifyReturnIsNotEmpty()
        {
            // arrange
            var service = new Rfc2898DeriveBytesPasswordHasher();

            // act
            var result = service.HashPassword("testpassword");

            // assert
            Assert.NotEmpty(result);
        }
    }
}