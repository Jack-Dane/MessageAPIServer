using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessageAppServer.Repository;
using MessageAppServer.Models;
using MessageAppServer.Helpers;
using MessageAppServer.Filters;

namespace MessageAppServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BasicAuthorisationFilter]
    public class UsersController : ControllerBaseAuthMethods
    {
        private readonly IUserRepository _userRepo; 

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _userRepo.GetUsers();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userRepo.FindUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (ValidateUser(id))
            {
                if (id != user.UserId)
                {
                    return BadRequest();
                }

                _userRepo.MarkAsModified(user);

                try
                {
                    await _userRepo.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(string username, string password, string name)
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

            _userRepo.AddUser(user);
            await _userRepo.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (ValidateUser(id)) 
            {
                var user = await _userRepo.FindUserAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                _userRepo.RemoveUser(user);
                await _userRepo.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("{id}/sent-messages/")]
        public async Task<ActionResult<IEnumerable<Message>>> GetSentMessages(int id){
            IEnumerable<Message> messages = await _userRepo.GetUsersSentMessages(id);

            return messages.ToArray();
        }

        [HttpGet("{id}/recieved-messages/")]
        public async Task<ActionResult<IEnumerable<Message>>> GetRecievedMessages(int id)
        {
            IEnumerable<Message> messages = await _userRepo.GetUsersRecievedMessages(id);

            return messages.ToArray();
        }

        private bool UserExists(int id)
        {
            return _userRepo.CheckUserExists(id);
        }

        private bool ValidateUser(int userId)
        {
            return GetUserId() is not null && GetUserId() == userId;
        }
    }
}
