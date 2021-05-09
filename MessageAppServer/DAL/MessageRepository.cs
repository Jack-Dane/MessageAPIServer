using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageAppServer.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageAppServer.DAL
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMessageContext _context;

        public MessageRepository(IMessageContext messageContext)
        {
            _context = messageContext;
        }

        public async Task<Message> FindMessageAsync(int messageId)
        {
            return await _context.Messages.FindAsync(messageId);
        }

        public void MarkAsModified(Message message)
        {
            _context.MarkAsModified(message);
        }

        public async Task<List<Message>> GetMessageBasedOnUser(int? userId)
        {
            List<Message> messages = await _context.Messages.Where(
                message =>
                message.RecieverId == userId
                ||
                message.SenderId == userId
            ).ToListAsync();
            return messages;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void RemoveMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public bool MessageExists(int messageId)
        {
            return _context.Messages.Any(e => e.MessageId == messageId);
        }
    }
}
