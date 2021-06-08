using Moq;
using NUnit.Framework;
using MessageAppServer.Models;
using MessageAppServer.DAL;
using System.Threading.Tasks;
using MessageAppServer.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MessageAppServer.Repository;

namespace MessageAppTests.Controllers
{
    class UserControllerUnitTest
    {
        private Mock<IUserRepository> _userRepo;

        [SetUp]
        public void Setup()
        {
            _userRepo = new Mock<IUserRepository>();
        }

        [Test]
        public async Task GetUser_ShouldReturnUser()
        {
            User user = GetUser();
            _userRepo.Setup(repo => repo.FindUserAsync("Test"))
                .Returns(Task.FromResult(user));

            var controller = new UsersController(_userRepo.Object);
            var result = await controller.GetUser("Test");

            _userRepo.Verify(repo => repo.FindUserAsync("Test"), Times.Once());
            Assert.IsInstanceOf<User>(result.Value);
        }

        [Test]
        public async Task GetUser_ShouldReturnNull()
        {
            User user = null;
            _userRepo.Setup(repo => repo.FindUserAsync("Test"))
                .Returns(Task.FromResult(user));

            var controller = new UsersController(_userRepo.Object);
            var result = await controller.GetUser("Test");

            _userRepo.Verify(repo => repo.FindUserAsync("Test"), Times.Once());
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        private User GetUser()
        {
            return new User()
            {
                Username = "Test",
                Name = "Test"
            };
        }
    }
}
