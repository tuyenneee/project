using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SignalRAssignment.Models;

namespace SignalRAssignment.Common
{
    public class Authorization { }

    public class StaffPermissionAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var account = VaSession.Get<Account>(context.HttpContext.Session, "Account");
            if (account == null || account.Type != 1)
            {
                // get query string from url
                var query = context.HttpContext.Request.QueryString;
                context.Result = new RedirectToPageResult("/Login", new { returnUrl = context.HttpContext.Request.Path + query });
                
            }
        }
    }

    public class MemberPermissionAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var account = VaSession.Get<Account>(context.HttpContext.Session, "Account");
            if (account == null || account.Type != 0)
            {
                var query = context.HttpContext.Request.QueryString;
                context.Result = new RedirectToPageResult("/Login", new { returnUrl = context.HttpContext.Request.Path + query });
            }
        }
    }
}
