using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPresentationMVC.Models {
    public class MvcHorarioConsultaModel {
        public int Id { get; set; }

        public string Weekday { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string Place { get; set; }
        public string EliminationDate { get; set; }

        // Usuarios with Matricula != Null Only
        [ForeignKey("Profesor")]
        public int ProfesorId { get; set; }
        public virtual MvcUsuarioModel Profesor { get; set; }

        [ForeignKey("Materia")]
        public int MateriaId { get; set; }
        public virtual MvcMateriaModel Materia { get; set; }
    }
}
