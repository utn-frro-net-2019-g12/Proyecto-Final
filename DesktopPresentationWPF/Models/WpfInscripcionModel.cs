using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesktopPresentationWPF.Models {
    public class WpfInscripcionModel {
        public int Id { get; set; }
        public string Topic { get; set; }
        public bool? State { get; set; }
        public string Answer { get; set; }
        public string Observation { get; set; }

        // Usuarios with Legajo != Null Only
        public int AlumnoId { get; set; }
        public virtual WpfUsuarioModel Alumno { get; set; }

        public int HorarioConsultaFechadoId { get; set; }
        public virtual WpfHorarioConsultaFechadoModel HorarioConsultaFechado { get; set; }
    }
}
