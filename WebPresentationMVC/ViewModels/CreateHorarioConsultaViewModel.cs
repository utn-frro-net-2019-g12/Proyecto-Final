using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Presentation.Web.MVC.Models;

namespace Presentation.Web.MVC.ViewModels {
    public class CreateHorarioConsultaViewModel {
        public CreateHorarioConsultaViewModel() { }

        public CreateHorarioConsultaViewModel(IEnumerable<MvcUsuarioModel> profesores, IEnumerable<MvcMateriaModel> materias) {
            this.SetProfesoresAsSelectList(profesores);
            this.SetMateriasAsSelectList(materias);
        }

        public MvcHorarioConsultaModel HorarioConsulta { get; set; }
        public IEnumerable<SelectListItem> ProfesoresList { get; set; }
        public IEnumerable<SelectListItem> MateriasList { get; set; }

        public void SetProfesoresAsSelectList(IEnumerable<MvcUsuarioModel> profesores)  {
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
