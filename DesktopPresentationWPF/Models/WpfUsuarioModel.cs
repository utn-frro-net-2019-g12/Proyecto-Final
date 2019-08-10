using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesktopPresentationWPF.Models {
    public class WpfUsuarioModel {
        public int id { get; set; }

        public string username { get; set; } // Remember: Add FK from Identity Framework

        // Legajo != Null --> Es Alumno
        public int legajo { get; set; }
        // Matrícula != Null --> Es Profesor
        public string matricula { get; set; }
        public bool isAdmin { get; set; }
        public string firstname { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public int phone { get; set; }
        // public blob photo { get; set; } --> Consultar en el Grupo el formato de imagen a usar
    }
}
