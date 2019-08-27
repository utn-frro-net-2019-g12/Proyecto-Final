using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace WebPresentationMVC.Models
{
    public class UserSession : IUserSession
    {
        public string Username
        {
            get
            {
                return ((ClaimsPrincipal)HttpContext.Current.User).FindFirst(ClaimTypes.Name).Value;
            }
        }

        public string BearerToken
        {
            get
            {
                return ((ClaimsPrincipal)HttpContext.Current.User).FindFirst("AcessToken").Value;
            }
        }
    }
}