using Microsoft.EntityFrameworkCore;
using MessageAppServer.Models;
using System.Threading.Tasks;

namespace MessageAppServer.DAL
{
    public class MessageContext : DbContext, IMessageContext
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

        public void MarkAsModified(Message item)
        {
            Entry(item).State = EntityState.Modified;
        }

        public void MarkAsModified(User item)
        {
            Entry(item).State = EntityState.Modified;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
