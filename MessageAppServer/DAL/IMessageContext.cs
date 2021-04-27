using MessageAppServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageAppServer.DAL
{
    public interface IMessageContext : IDisposable
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public void MarkAsModified(Message item);
        public void MarkAsModified(User user);
        public Task<int> SaveChangesAsync();
    }
}
