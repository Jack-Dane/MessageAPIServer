using MessageAppServer.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.HttpSys;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MessageAppServer.DAL;
using MessageAppServer.Models;

namespace MessageAppServer.Filters
{
    public class BasicAuthenticationFilter
    {
        private readonly RequestDelegate next;

        public BasicAuthenticationFilter(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IAuthenticationService authenticationService)
        {
            try
            {
                BasicAuthenticationParser parser = new BasicAuthenticationParser(context);

                // put in the authHeader and get the username and password from the parser
                parser.ParseHeader();
                string username = parser.Username;
                string password = parser.Password;

                // find the usernamd and password in the database
                int? userId = CheckReturnUserId(username, password);
                if (userId != null)
                {
                    // add the user to the context
                    AssignUserToContext(context, userId);
                }
                await next(context);
            }
            catch (FormatException)
            {
                Debug.WriteLine("Format issues");
            }
        }

        private void AssignUserToContext(HttpContext context, int? userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString(), ClaimValueTypes.Integer, null)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "identity");
            ClaimsPrincipal user = new ClaimsPrincipal(identity);

            context.User = user;
        }

        private int? CheckReturnUserId(string username, string password)
        {
            using var db = new MessageContext();

            // get the id where username is the same
            User user = db.Users.Where(user => user.Username.Equals(username)).FirstOrDefault();
            if(CheckPassword(user, password))
                return user.UserId;
            return null;
        }

        private bool CheckPassword(User user, string password)
        {
            // check the hashed password with the one supplied in database with salt
            if (user is not null)
                return PasswordHasher.CheckPassword(password, user.Salt, user.Password);
            return false;
        }
    }

    public static class BasicAuthenticationExtensions
    {
        public static void UseBasicAuthentication(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<BasicAuthenticationFilter>();
        }
    }
}
