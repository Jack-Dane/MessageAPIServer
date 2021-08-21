using MessageAppServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageAppServer.Repository
{
    public interface IMessageRepository
    {
        // TODO - will be standard in all repos - extract?
        public void MarkAsModified(Message message);
        public Task<int> SaveChangesAsync();
        public Task<List<Message>> GetMessageBasedOnUser(int? userId, int page, int limit);
        public Task<Message> FindMessageAsync(int messageId);
        public void AddMessage(Message message);
        public void RemoveMessage(Message message);
        public bool MessageExists(int messageId);
    }
}
