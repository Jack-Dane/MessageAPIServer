using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MessageAppServer.Controllers
{
    public class ControllerBaseAuthMethods : ControllerBase
    {
        protected int? GetUserId()
        {
            int? userId = null;
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return userId;
        }
    }
}
