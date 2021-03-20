using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MessageAppServer.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Username { get; set; }
        public string Name { get; set; }
    }
}
