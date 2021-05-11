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

        public async Task<User> FindUserAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
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

        public bool CheckUserExists(int userId)
        {
            return _context.Users.Any(e => e.UserId == userId);
        }

        public async Task<List<Message>> GetUsersSentMessages(int userId)
        {
            return await _context.Messages.Where(message => message.SenderId == userId)
                .ToListAsync();
        }

        public async Task<List<Message>> GetUsersRecievedMessages(int userId)
        {
            return await _context.Messages.Where(message => message.RecieverId == userId)
                .ToListAsync();
        }
    }
}
