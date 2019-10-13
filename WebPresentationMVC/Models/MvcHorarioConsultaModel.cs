using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPresentationMVC.Models {
    public class MvcHorarioConsultaModel {
        public int Id { get; set; }

        [DisplayName("Día de la Semana")]
        public string Weekday { get; set; }

        [DisplayName("Hora Inicio")]
        public string StartHour { get; set; }

        [DisplayName("Hora Finalización")]
        public string EndHour { get; set; }

        [DisplayName("Lugar")]
        public string Place { get; set; }

        [DisplayName("Fecha Eliminación")]
        public string EliminationDate { get; set; }


        // Usuarios with Matricula != Null Only
        [ForeignKey("Profesor")]
        public int? ProfesorId { get; set; }
        public virtual MvcUsuarioModel Profesor { get; set; }

        [ForeignKey("Materia")]
        public int? MateriaId { get; set; }
        public virtual MvcMateriaModel Materia { get; set; }
    }
}
