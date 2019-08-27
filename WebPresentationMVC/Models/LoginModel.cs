using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPresentationMVC.Models
{
    public class LoginModel
    {
        public string EmailAddress {
            get
            {
                return "ale@example.com";
            }
        }
        public string Password {
            get
            {
                return "Example1?";
            }
        }
    }
}