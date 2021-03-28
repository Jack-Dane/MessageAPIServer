using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageAppServer.Models
{
    public class LiveUser
    {
        public int LiveUserId { get; set; }

        public virtual User User { get; set; }
        public int UserId { get; set; }

        public string connectionString { get; set; }
    }
}
