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
        public Task<User> FindUserAsync(int userId);
        public void AddUser(User user);
        public void RemoveUser(User user);
        public bool CheckUserExists(int userId);
        public Task<List<Message>> GetUsersSentMessages(int userId);
        public Task<List<Message>> GetUsersRecievedMessages(int userId);
        public Task<User> GetUserBasedOnUsername(string username);
    }
}
