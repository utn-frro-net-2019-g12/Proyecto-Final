using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPresentationMVC.Models {
    public class MvcHorarioConsultaFechadoModel {

        public int Id { get; set; }

        [DisplayName("Fecha")]
        public System.DateTime Date { get; set; }

        [ForeignKey("HorarioConsulta")]
        public int? HorarioConsultaId { get; set; }
        public virtual MvcHorarioConsultaModel HorarioConsulta { get; set; }
    }
}
