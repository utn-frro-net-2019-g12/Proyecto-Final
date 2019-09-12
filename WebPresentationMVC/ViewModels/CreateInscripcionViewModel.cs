using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPresentationMVC.Models;

namespace WebPresentationMVC.ViewModels {
    public class CreateInscripcionViewModel {
        public CreateInscripcionViewModel() { }

        public CreateInscripcionViewModel(IEnumerable<MvcUsuarioModel> alumnos, IEnumerable<MvcHorarioConsultaModel> horariosConsulta) {
            this.SetAlumnosAsSelectList(alumnos);
            this.SetHorariosConsultaAsSelectList(horariosConsulta);
        }

        public MvcInscripcionModel Inscripcion { get; set; }
        public IEnumerable<SelectListItem> AlumnosList { get; set; }
        public IEnumerable<SelectListItem> HorariosConsultaList { get; set; }

        public void SetAlumnosAsSelectList(IEnumerable<MvcUsuarioModel> profesores)  {
            AlumnosList = profesores.Where(e => e.Legajo != null).Select(e => new SelectListItem() {
                Value = e.Id.ToString(),
                Text = e.Surname + " " + e.Firstname
            }) as IEnumerable<SelectListItem>;
        }

        public void SetHorariosConsultaAsSelectList(IEnumerable<MvcHorarioConsultaModel> horariosConsulta) {
            HorariosConsultaList = horariosConsulta.Select(e => new SelectListItem() {
                Value = e.Id.ToString(),
                Text = e.Profesor.Surname + " " + e.Profesor.Firstname + " /// " + e.Materia.Name
            }) as IEnumerable<SelectListItem>;
        }
    }
}
