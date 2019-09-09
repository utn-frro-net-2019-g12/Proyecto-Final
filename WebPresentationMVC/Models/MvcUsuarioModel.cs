using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPresentationMVC.Models {
    public class MvcUsuarioModel {
        public int Id { get; set; }

        [DisplayName("Usuario")]
        public string Username { get; set; }

        // Legajo != Null --> Es Alumno
        [DisplayName("Legajo")]
        public int? Legajo { get; set; }
        // Matrícula != Null --> Es Profesor

        [DisplayName("Matrícula")]
        public string Matricula { get; set; }

        public bool IsAdmin { get; set; }

        [DisplayName("Nombre")]
        public string Firstname { get; set; }

        [DisplayName("Apellido")]
        public string Surname { get; set; }

        public string Email { get; set; }

        [DisplayName("Teléfono 1")]
        public int? Phone1 { get; set; }

        [DisplayName("Teléfono 2")]
        public int? Phone2 { get; set; }

        // public Bitmap Photo { get; set; } --> Consultar
    }
}
