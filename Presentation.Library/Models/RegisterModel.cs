using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Presentation.Library.Models {
    public class RegisterModel {
        public string Email { get; set; }

        [DisplayName("Constraseña")]
        public string Password { get; set; }

        [DisplayName("Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }
    }
}
