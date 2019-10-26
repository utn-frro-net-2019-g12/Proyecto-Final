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
        public HCFStates? State { get; set; }
        public string Observation { get; set; }
        public System.DateTime? PostponementDate { get; set; }

        [ForeignKey("HorarioConsulta")]
        public int? HorarioConsultaId { get; set; }
        public virtual MvcHorarioConsultaModel HorarioConsulta { get; set; }

        // HorarioConsultaFechado's Enumeration of States
        public enum HCFStates {
            active,
            postponed,
            canceled,
            finalized
        }
    }
}
