using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Presentation.Web.MVC.Models;
using static Presentation.Library.Models.Inscripcion;

namespace Presentation.Web.MVC.ViewModels {
    public class EditInscripcionViewModel {
        public EditInscripcionViewModel() { }

        public EditInscripcionViewModel(IEnumerable<MvcUsuarioModel> alumnos, IEnumerable<MvcHorarioConsultaFechadoModel> horariosConsultaFechados, MvcInscripcionModel inscripcion) {
            this.SetAlumnosAsSelectList(alumnos);
            this.SetHorariosConsultaFechadosAsSelectList(horariosConsultaFechados);
            this.SetEstadosAsSelectList();
            this.Inscripcion = inscripcion;
        }

        public MvcInscripcionModel Inscripcion { get; set; }
        public IEnumerable<SelectListItem> AlumnosList { get; set; }
        public IEnumerable<SelectListItem> HorariosConsultaFechadosList { get; set; }
        public IEnumerable<SelectListItem> EstadosList { get; set; }

        public void SetAlumnosAsSelectList(IEnumerable<MvcUsuarioModel> profesores) {
            AlumnosList = profesores.Where(e => e.Legajo != null).Select(e => new SelectListItem() {
                Value = e.Id.ToString(),
                Text = e.Surname + " " + e.Firstname
            }) as IEnumerable<SelectListItem>;
        }

        public void SetHorariosConsultaFechadosAsSelectList(IEnumerable<MvcHorarioConsultaFechadoModel> horariosConsultaFechados) {
            HorariosConsultaFechadosList = horariosConsultaFechados.Select(e => new SelectListItem() {
                Value = e.Id.ToString(),
                Text = e.HorarioConsulta.Profesor.Surname + " " + e.HorarioConsulta.Profesor.Firstname + " /// " +
                       e.HorarioConsulta.Materia.Name + " /// " + e.HorarioConsulta.Weekday + " " +e.DateForDisplay + " " +
                       e.HorarioConsulta.StartHour + " - " + e.HorarioConsulta.EndHour
            }) as IEnumerable<SelectListItem>;
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
