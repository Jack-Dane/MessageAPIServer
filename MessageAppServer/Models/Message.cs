using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageAppServer.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        public virtual User Sender { get; set; }
        public int SenderId { get; set; }

        public virtual User Reciever { get; set; }
        public int RecieverId { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
    }
}
