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

namespace MessageAppServer.FIlters
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
                string authHeader = context.Request.Headers["Authorization"];
                BasicAuthenticationParser parser = new BasicAuthenticationParser(context);

                // put in the authHeader and get the username and password from the parser
                parser.ParseHeader(authHeader);
                string username = parser.Username;
                string password = parser.Password;

                // get the username and password from the headers
                if (username.Equals("username") && password.Equals("password"))
                {
                    AssignUserToContext(context);
                    await next(context);
                    return;
                }
                context.Response.StatusCode = 401;
            }
            catch (FormatException)
            {
                context.Response.StatusCode = 401;
            }
        }

        private void AssignUserToContext(HttpContext context)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "username", ClaimValueTypes.String, null),
                new Claim(ClaimTypes.NameIdentifier, "123", ClaimValueTypes.Integer, null)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "identity");
            ClaimsPrincipal user = new ClaimsPrincipal(identity);

            context.User = user;
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
