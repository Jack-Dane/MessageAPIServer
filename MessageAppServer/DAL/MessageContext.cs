using Microsoft.EntityFrameworkCore;
using MessageAppServer.Models;

namespace MessageAppServer.DAL
{
    public class MessageContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }

        public MessageContext(DbContextOptions<MessageContext> options) : base(options) { }

        public MessageContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=C:\Users\jackd\source\repos\MessageAPIServer\MessageAppServer\MessageApp.db")
            .UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(user => user.RecievedMessages)
                .WithOne(message => message.Reciever);

            modelBuilder.Entity<User>()
                .HasMany(user => user.SentMessages)
                .WithOne(Message => Message.Sender);
        }
    }
}
