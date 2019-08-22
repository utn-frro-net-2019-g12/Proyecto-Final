using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesktopPresentationWPF.Models {
    public class WpfUsuarioModel {
        public int Id { get; set; }

        public string Username { get; set; }

        public int? Legajo { get; set; }

        public string Matricula { get; set; }

        public bool IsAdmin { get; set; }

        public string Firstname { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public int? Phone { get; set; }
    }
}
