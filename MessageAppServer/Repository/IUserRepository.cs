using System.Collections.Generic;
using System.Threading.Tasks;
using MessageAppServer.Models;

namespace MessageAppServer.Repository
{
    public interface IUserRepository
    {
        public void MarkAsModified(User user);
        public Task<int> SaveChangesAsync();
        public Task<List<User>> GetUsers();
        public Task<User> FindUserAsync(string username);
        public void AddUser(User user);
        public void RemoveUser(User user);
        public bool CheckUserExists(string username);
        public Task<List<Message>> GetUsersSentMessages(string username);
        public Task<List<Message>> GetUsersRecievedMessages(string username);
        public Task<User> GetUserBasedOnUsername(string username);
    }
}
