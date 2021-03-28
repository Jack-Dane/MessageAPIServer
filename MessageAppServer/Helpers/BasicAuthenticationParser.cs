using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.HttpSys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MessageAppServer.Helpers
{
    public class BasicAuthenticationParser
    {
        private HttpContext _context;

        public BasicAuthenticationParser(HttpContext context)
        {
            _context = context;
        }

        public void ParseHeader(string authHeader)
        {
            if (authHeader != null)
            {
                var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
                if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    var credentials = Encoding.UTF8
                                        .GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty))
                                        .Split(":", 2);
                    if (credentials.Length == 2)
                    {
                        Username = credentials[0];
                        Password = credentials[1];
                    }
                }
            }
        }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
