using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageAppServer.DAL;
using MessageAppServer.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageAppServerTests.MockData
{
    public class TestMessageContext : IMessageContext
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }

        public TestMessageContext() { }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
