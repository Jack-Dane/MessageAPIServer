using Microsoft.EntityFrameworkCore;
using MessageAppServer.Models;

namespace MessageAppServer.DAL
{
    public class MessageContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }

        public MessageContext(DbContextOptions<MessageContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=C:\Users\44785\source\repos\MessageAppServer\MessageAppServer\MessageApp.db")
            .UseLazyLoadingProxies();
    }
}
