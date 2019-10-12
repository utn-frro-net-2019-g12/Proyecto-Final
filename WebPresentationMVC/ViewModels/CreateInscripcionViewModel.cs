using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPresentationMVC.Models;

namespace WebPresentationMVC.ViewModels {
    public class CreateInscripcionViewModel {
        public CreateInscripcionViewModel() { }

        public CreateInscripcionViewModel(IEnumerable<MvcUsuarioModel> alumnos, IEnumerable<MvcHorarioConsultaFechadoModel> horariosConsultaFechados) {
            this.SetAlumnosAsSelectList(alumnos);
            this.SetHorariosConsultaFechadosAsSelectList(horariosConsultaFechados);
        }

        public MvcInscripcionModel Inscripcion { get; set; }
        public IEnumerable<SelectListItem> AlumnosList { get; set; }
        public IEnumerable<SelectListItem> HorariosConsultaFechadosList { get; set; }

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
                       e.HorarioConsulta.Materia.Name + " Fecha: " + e.Date
            }) as IEnumerable<SelectListItem>;
        }
    }
}
