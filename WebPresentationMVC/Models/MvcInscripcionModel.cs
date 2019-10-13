using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPresentationMVC.Models {
    public class MvcInscripcionModel {

        public int Id { get; set; }

        [DisplayName("Asunto")]
        public string Topic { get; set; }

        [DisplayName("Estado")]
        public bool? State { get; set; }

        [DisplayName("Respuesta Corta")]
        public string Answer { get; set; }

        [DisplayName("Observación")]
        public string Observation { get; set; }


        // Usuarios with Legajo != Null Only
        [ForeignKey("Alumno")]
        public int? AlumnoId { get; set; }
        public virtual MvcUsuarioModel Alumno { get; set; }

        [ForeignKey("HorarioConsultaFechado")]
        public int? HorarioConsultaFechadoId { get; set; }
        public virtual MvcHorarioConsultaFechadoModel HorarioConsultaFechado { get; set; }
    }
}
