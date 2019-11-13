using System;
using AuthApi.Services.Impl;
using Xunit;

namespace AuthApi.Tests.Services
{
    public class Rfc2898DeriveBytesPasswordHasher_CheckPasswordHashTests
    {
        [Fact]
        public void GivenRequestToCheckPasswordHash_VerifyExceptionIsThrownForEmptyHashedPassword()
        {
            // arrange
            var service = new Rfc2898DeriveBytesPasswordHasher();

            // act
            // assert
            Assert.Throws<ArgumentException>(() => service.CheckPasswordHash(string.Empty, "test"));
        }

        [Fact]
        public void GivenRequestToCheckPasswordHash_VerifyExceptionIsThrownForEmptyRawPassword()
        {
            // arrange
            var service = new Rfc2898DeriveBytesPasswordHasher();

            // act
            // assert
            Assert.Throws<ArgumentException>(() => service.CheckPasswordHash("test", string.Empty));
        }

        [Fact]
        public void GivenRequestToCheckPasswordHash_VerifyTrueIfHashAndRawAreEquivalent()
        {
            // arrange
            var service = new Rfc2898DeriveBytesPasswordHasher();
            var hashedPassword = service.HashPassword("test");

            // act
            var isMatch = service.CheckPasswordHash(hashedPassword, "test");

            // assert
            Assert.True(isMatch);
        }
    }
}