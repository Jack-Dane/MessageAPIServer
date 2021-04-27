using MessageAppServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageAppServerTests.MockData
{
    class TestMessageDbSet : TestDbSet<Message>
    {
        public override Message Find(params object[] keyValues)
        {
            return this.SingleOrDefault(Message => Message.MessageId == (int)keyValues.Single());
        }
    }
}
