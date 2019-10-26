using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Desktop.WPF.Models {
    public class WpfHorarioConsultaFechadoModel {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public HCFStates? State { get; set; }
        public string Observation { get; set; }
        public System.DateTime? PostponementDate { get; set; }

        public int HorarioConsultaId { get; set; }
        public virtual WpfHorarioConsultaModel HorarioConsulta { get; set; }

        public string ReturnDateForDisplay {
            get {
                return this.Date.ToString("dd/MM/yyyy");
            }
        }

        // HorarioConsultaFechado's Enumeration of States
        public enum HCFStates {
            active,
            postponed,
            canceled,
            finalized
        }
    }
}
