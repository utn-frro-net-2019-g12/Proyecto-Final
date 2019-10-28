using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Presentation.Web.MVC.Models {
    public class MvcHorarioConsultaFechadoModel {

        public int Id { get; set; }

        [DisplayName("Fecha")]
        public System.DateTime Date { get; set; }
        [DisplayName("Estado")]
        public HCFStates? State { get; set; }
        [DisplayName("Observación")]
        public string Observation { get; set; }
        [DisplayName("Fecha Pospuesta")]
        public System.DateTime? PostponementDate { get; set; }

        [ForeignKey("HorarioConsulta")]
        public int? HorarioConsultaId { get; set; }
        public virtual MvcHorarioConsultaModel HorarioConsulta { get; set; }

        public string DateForDisplay {
            get {
                return this.Date.ToString("dd/MM/yyyy");
            }
        }

        public string PostponementDateForDisplay {
            get {
                return this.PostponementDate.HasValue ? this.PostponementDate.Value.ToString("dd/MM/yyyy") : null;
            }
        }

        // HorarioConsultaFechado's Enumeration of States
        public enum HCFStates {
            Active,
            Postponed,
            Canceled,
            Finalized
        }
    }
}
