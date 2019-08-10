using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPresentationMVC.Models {
    public class MvcUsuarioModel {
        public int Id { get; set; }

        public string Username { get; set; }
        /*
        // Remember: Add FK from Identity Framework
        [ForeignKey("IdentityFramework")]
        public int? UsuarioId { get; set; }
        public virtual MvcDepartamentoModel Departamento { get; set; }
        */

        // Legajo != Null --> Es Alumno
        public int Legajo { get; set; }
        // Matrícula != Null --> Es Profesor
        public string Matricula { get; set; }
        public bool IsAdmin { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        // public blob Photo { get; set; } --> Consultar en el Grupo el formato de imagen a usar

    }
}
