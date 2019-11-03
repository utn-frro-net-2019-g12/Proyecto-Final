using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Presentation.Web.MVC.Models;
using static Presentation.Library.Models.Inscripcion;

namespace Presentation.Web.MVC.ViewModels {
    public class SubscribeViewModel {
        public SubscribeViewModel() {
            this.SetEstadosAsSelectList();
        }

        public SubscribeViewModel(IEnumerable<MvcHorarioConsultaFechadoModel> horariosConsultaFechados) {
            this.SetHorariosConsultaFechadosAsSelectList(horariosConsultaFechados);
            this.SetEstadosAsSelectList();
        }

        public MvcInscripcionModel Inscripcion { get; set; }
        public IEnumerable<SelectListItem> HorariosConsultaFechadosList { get; set; }
        public IEnumerable<SelectListItem> EstadosList { get; set; }

        public void SetAlumno(int id_alumno) {
            Inscripcion.AlumnoId = id_alumno;
        }

        public void SetHorariosConsultaFechadosAsSelectList(IEnumerable<MvcHorarioConsultaFechadoModel> horariosConsultaFechados) {
            HorariosConsultaFechadosList = horariosConsultaFechados.Select(e => new SelectListItem() {
                Value = e.Id.ToString(),
                Text = e.HorarioConsulta.Weekday + " " + e.DateForDisplay + " " + e.HorarioConsulta.StartHour + " - " + e.HorarioConsulta.EndHour
            }) as IEnumerable<SelectListItem>;
        }

        public void SetHorarioConsultaFechado(int id_hcf) {
            Inscripcion.HorarioConsultaFechadoId = id_hcf;
        }

        public void SetEstadosAsSelectList() {
            InscripcionStates[] estadosValues = (InscripcionStates[])Enum.GetValues(typeof(InscripcionStates));
            
            EstadosList = estadosValues.Select(e => new SelectListItem() {
                Value = e.ToString(),
                Text = e.ToString()
            }) as IEnumerable<SelectListItem>;
        }
    }
}
