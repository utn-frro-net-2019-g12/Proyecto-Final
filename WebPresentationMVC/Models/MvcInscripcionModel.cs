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

        public string Answer { get; set; }

        [DisplayName("Observación")]
        public string Observation { get; set; }


        // Usuarios with Legajo != Null Only
        [ForeignKey("Alumno")]
        public int AlumnoId { get; set; }
        public virtual MvcUsuarioModel Alumno { get; set; }

        [ForeignKey("HorarioConsulta")]
        public int HorarioConsultaId { get; set; }
        public virtual MvcHorarioConsultaModel HorarioConsulta { get; set; }
    }
}
