using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Presentation.Web.MVC.Models;

namespace Presentation.Web.MVC.ViewModels {
    public class EditHorarioConsultaFechadoViewModel {
        public EditHorarioConsultaFechadoViewModel() { }

        public EditHorarioConsultaFechadoViewModel(IEnumerable<MvcHorarioConsultaModel> horariosConsulta, MvcHorarioConsultaFechadoModel horarioConsultaFechado) {
            this.SetHorariosConsultaAsSelectList(horariosConsulta);
            this.HorarioConsultaFechado = horarioConsultaFechado;
        }

        public MvcHorarioConsultaFechadoModel HorarioConsultaFechado { get; set; }
        public IEnumerable<SelectListItem> HorariosConsultaList { get; set; }

        public void SetHorariosConsultaAsSelectList(IEnumerable<MvcHorarioConsultaModel> horariosConsulta) {
            HorariosConsultaList = horariosConsulta.Select(e => new SelectListItem() {
                Value = e.Id.ToString(),
                Text = e.Materia + " --- " + e.Profesor.Surname + " " + e.Profesor.Firstname
            }) as IEnumerable<SelectListItem>;
        }
    }
}
