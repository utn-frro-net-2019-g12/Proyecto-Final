using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPresentationMVC.Models;

namespace WebPresentationMVC.ViewModels {
    public class EditHorarioConsultaViewModel {
        public EditHorarioConsultaViewModel() { }

        public EditHorarioConsultaViewModel(IEnumerable<MvcUsuarioModel> profesores, IEnumerable<MvcMateriaModel> materias, MvcHorarioConsultaModel horarioConsulta) {
            this.SetProfesoresAsSelectList(profesores);
            this.SetMateriasAsSelectList(materias);
            this.HorarioConsulta = horarioConsulta;
        }

        public MvcHorarioConsultaModel HorarioConsulta { get; set; }
        public IEnumerable<SelectListItem> ProfesoresList { get; set; }
        public IEnumerable<SelectListItem> MateriasList { get; set; }

        public void SetProfesoresAsSelectList(IEnumerable<MvcUsuarioModel> profesores) {
            ProfesoresList = profesores.Where(e => e.Matricula != null).Select(e => new SelectListItem() {
                Value = e.Id.ToString(),
                Text = e.Surname + " " + e.Firstname
            }) as IEnumerable<SelectListItem>;
        }

        public void SetMateriasAsSelectList(IEnumerable<MvcMateriaModel> materias) {
            MateriasList = materias.Select(e => new SelectListItem() {
                Value = e.Id.ToString(),
                Text = e.Name
            }) as IEnumerable<SelectListItem>;
        }
    }
}
