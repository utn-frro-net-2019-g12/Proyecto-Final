using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebPresentationMVC.Models
{
    public class LoginModel
    {
        [DisplayName("Email")]
        public string EmailAddress { get; set; }

        [DisplayName("Contraseña")]
        public string Password { get; set; }
    }
}