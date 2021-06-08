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

        [HttpGet("{username}")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            var user = await _userRepo.FindUserAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateUser(string username, User user)
        {
            if (await ValidateUserAsync(username))
            {
                if (username != user.Username)
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
                    if (!UserExists(username))
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

        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            if (await ValidateUserAsync(username))
            {
                var user = await _userRepo.FindUserAsync(username);
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

        [HttpGet("{username}/sent-messages/")]
        public async Task<ActionResult<IEnumerable<Message>>> GetSentMessages(string username){
            IEnumerable<Message> messages = await _userRepo.GetUsersSentMessages(username);

            return messages.ToArray();
        }

        [HttpGet("{username}/received-messages/")]
        public async Task<ActionResult<IEnumerable<Message>>> GetRecievedMessages(string username)
        {
            IEnumerable<Message> messages = await _userRepo.GetUsersRecievedMessages(username);

            return messages.ToArray();
        }

        private bool UserExists(string username)
        {
            return _userRepo.CheckUserExists(username);
        }

        private async Task<bool> ValidateUserAsync(string username)
        {
            User user = await GetUserBasedOnUsername(username);
            return GetUserId() is not null && GetUserId() == user.UserId;
        }

        private async Task<User> GetUserBasedOnUsername(string username)
        {
            return await _userRepo.FindUserAsync(username);
        }
    }
}
