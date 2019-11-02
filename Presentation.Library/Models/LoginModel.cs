using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Library.Models
{
    public class LoginModel
    {
        [DisplayName("Email")]
        public string EmailAddress { get; set; }

        [DisplayName("Contraseña")]
        public string Password { get; set; }
        
        [DisplayName("Recuérdame")]
        public bool RememberMe { get; set; }
    }
}