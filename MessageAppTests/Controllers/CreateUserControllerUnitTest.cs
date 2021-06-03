using MessageAppServer.Models;
using MessageAppServer.Repository;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using MessageAppServer.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MessageAppTests.Controllers
{
    class CreateUserControllerUnitTest
    {
        private Mock<IUserRepository> _userRepo;

        [SetUp]
        public void Setup()
        {
            _userRepo = new Mock<IUserRepository>();
        }

        [Test]
        public async Task CreateUser_ShouldReturnError()
        {
            string username = "Test";
            string password = "Test";
            string name = "Test";

            // when the user doesn't exist
            _userRepo.Setup(repo => repo.GetUserBasedOnUsername("Test")).
                Returns(Task.FromResult(GetUser()));

            var controller = new CreateUserController(_userRepo.Object);
            var response = await controller.CreateUser(username, password, name);

            // TODO - check for bad request on return
            _userRepo.Verify(mock => mock.AddUser(It.IsAny<User>()), Times.Never());
            _userRepo.Verify(mock => mock.SaveChangesAsync(), Times.Never());
        }

        [Test]
        public async Task CreateUser_ShouldCreateUser()
        {
            string username = "Test";
            string password = "Test";
            string name = "Test";

            // when the user already exists
            _userRepo.Setup(repo => repo.GetUserBasedOnUsername("Test"))
                .Returns(Task.FromResult((User)null));

            var controller = new CreateUserController(_userRepo.Object);
            var actionResponse = await controller.CreateUser(username, password, name);
            var response = actionResponse.Value;

            // TODO - check for insatnce of user on return
            _userRepo.Verify(mock => mock.AddUser(It.IsAny<User>()), Times.Once());
            _userRepo.Verify(mock => mock.SaveChangesAsync(), Times.Once());
        }

        private User GetUser()
        {
            return new User() {
                Username = "Test", 
                Password = "Test",
                Name = "Test",
            };
        }
    }
}
