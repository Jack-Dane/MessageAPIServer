using MessageAppServer.Helpers;
using MessageAppServer.Models;
using MessageAppServer.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MessageAppServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateUserController : ControllerBaseAuthMethods
    {
        private IUserRepository _userRepo;

        public CreateUserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(string username, string password, string name)
        {
            string salt = PasswordHasher.GenerateSalt(32);
            string hashedPassword = PasswordHasher.HashPassword(password, salt);

            User user = new()
            {
                Name = name,
                Username = username,
                Password = hashedPassword,
                Salt = salt,
            };

            bool userExists = await UserExistsBasedOnEmail(username);
            if (!userExists)
            {
                _userRepo.AddUser(user);
                await _userRepo.SaveChangesAsync();

                return CreatedAtRoute("GetUser", new { id = user.UserId }, user);
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<bool> UserExistsBasedOnEmail(string email)
        {
            return await _userRepo.GetUserBasedOnUsername(email) is not null;
        }
    }
}
