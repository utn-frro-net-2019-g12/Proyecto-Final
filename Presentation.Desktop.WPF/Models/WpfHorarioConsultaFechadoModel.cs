using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Desktop.WPF.Models {
    public class WpfHorarioConsultaFechadoModel {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }

        public int HorarioConsultaId { get; set; }
        public virtual WpfHorarioConsultaModel HorarioConsulta { get; set; }
    }
}
