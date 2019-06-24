using RestAPI.Services;
using System;
using Xunit;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace RestAPI.Tests
{
    public class PasswordHashManagerTests
    {
        [Theory]
        [InlineData("admin", "123456", "2Q75X7K6+uRmFQqK9XRjE3+E+TfeYnpelZogcT1c3R4=")]
        public void GetHashStringTest(string login, string password, string expectedResult)
        {
            var hashManager = new PasswordHashManager();
            var result = hashManager.GetHashString(login, password);
            Assert.Equal(expectedResult, result);
        }
    }
}
