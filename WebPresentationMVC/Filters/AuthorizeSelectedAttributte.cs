using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Presentation.Web.MVC.Filters
{
    public class AuthorizeSelected : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            // First checks if user is logged
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult
                (
                    new RouteValueDictionary
                    (
                        new
                        {
                            controller = "Account",
                            action = "Login",
                            returnUrl = filterContext.HttpContext.Request.Url
                        }
                    )
                );

                return;
            }

            // Then checks if exception was raised
            if(filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult
                (
                    new RouteValueDictionary
                    (
                        new
                        {
                            controller = "Account",
                            action = "Unauthorized"
                        }
                     )
                );
            }
        }
    }
}