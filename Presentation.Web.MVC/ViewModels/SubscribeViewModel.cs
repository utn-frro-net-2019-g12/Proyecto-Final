using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Presentation.Web.MVC.Models;
using static Presentation.Library.Models.Inscripcion;

namespace Presentation.Web.MVC.ViewModels {
    public class SubscribeViewModel {
        public string Topic { get; set; }
        public InscripcionStates? State {
            get
            {
                return InscripcionStates.Active;
            }
        }
        public string Answer { get; set; }
        public string Observation { get; set; }

        public int? AlumnoId { get; set; }

        public int? HorarioConsultaFechadoId { get; set; }

        public enum InscripcionStates
        {
            Active,
            Canceled,
            Finalized
        }
    }
}
