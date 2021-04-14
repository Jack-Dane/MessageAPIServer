using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MessageAppServer.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual List<Message> RecievedMessages { get; set; } = new List<Message>();
        [JsonIgnore]
        public virtual List<Message> SentMessages { get; set; } = new List<Message>();
    }
}
