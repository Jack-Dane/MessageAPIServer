using Moq;
using NUnit.Framework;
using MessageAppServer.Helpers;

namespace MessageAppTests.Helpers
{
    class PasswordHasherUnitTest
    {
        [Test]
        public void HashPassword_ShouldReturnCorrectHash()
        {
            string password = "Password";
            string salt = "1234";
            string expected = "oPMoWwfCbA3NIZFEfzkRcNBgNejVfjGgSLqHB086mhU=";

            string actual = PasswordHasher.HashPassword(password, salt);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CheckPassword_ShouldReturnTrue()
        {
            string hashedPassword = "oPMoWwfCbA3NIZFEfzkRcNBgNejVfjGgSLqHB086mhU=";
            string password = "Password";
            string salt = "1234";

            bool result = PasswordHasher.CheckPassword(password, salt, hashedPassword);
            Assert.IsTrue(result);
        }
    }
}
