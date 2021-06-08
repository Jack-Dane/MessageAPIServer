using MessageAppServer.DAL;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MessageAppServer.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MessageAppServer.Repository
{
    public class UserRepository : IUserRepository
    {
        private MessageContext _context;
        public UserRepository(MessageContext context)
        {
            _context = context;
        }

        public async Task<User> FindUserAsync(string username)
        {
            return await _context.Users.Where(user => user.Username == username)
                .FirstOrDefaultAsync();
        }

        public Task<List<User>> GetUsers()
        {
            return _context.Users.ToListAsync();
        }

        public void MarkAsModified(User user)
        {
            _context.MarkAsModified(user);
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public void RemoveUser(User user)
        {
            _context.Users.Remove(user);
        }

        public bool CheckUserExists(string username)
        {
            return _context.Users.Any(e => e.Username == username);
        }

        public async Task<List<Message>> GetUsersSentMessages(string username)
        {
            User user = await FindUserAsync(username);
            return await _context.Messages.Where(message => message.SenderId == user.UserId)
                .ToListAsync();
        }

        public async Task<List<Message>> GetUsersRecievedMessages(string username)
        {
            User user = await FindUserAsync(username);
            return await _context.Messages.Where(message => message.RecieverId == user.UserId)
                .ToListAsync();
        }

        public async Task<User> GetUserBasedOnUsername(string username)
        {
            return await _context.Users.Where(user => user.Username == username).FirstOrDefaultAsync();
        }
    }
}
