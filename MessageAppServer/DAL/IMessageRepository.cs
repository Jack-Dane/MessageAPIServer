using MessageAppServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageAppServer.DAL
{
    public interface IMessageRepository
    {
        public Task<List<Message>> GetMessageBasedOnUser(int? userId);
        public Task<Message> FindMessageAsync(int messageId);
        public void MarkAsModified(Message message);
        public Task<int> SaveChangesAsync();
        public void AddMessage(Message message);
        public void RemoveMessage(Message message);
        public bool MessageExists(int messageId);
    }
}
