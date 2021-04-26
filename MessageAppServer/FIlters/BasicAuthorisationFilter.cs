using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MessageAppServer.Filters
{
    public class BasicAuthorisationFilter : Attribute, IAuthorizationFilter
    {
        public string Role { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            switch (Role)
            {
                default:
                    // when role is null, just check to make sure the user has been authenticated
                    if (!context.HttpContext.User.Identity.IsAuthenticated)
                    {
                        SetUnauthorised(context);
                        return;
                    }
                    break;
            }
        }

        private void SetUnauthorised(AuthorizationFilterContext context)
        {
            context.ModelState.AddModelError("Unauthorised", "You are not authorised");
            context.Result = new UnauthorizedObjectResult(context.ModelState);
        }
    }
}
