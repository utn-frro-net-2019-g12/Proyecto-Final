using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPresentationMVC.Models;

namespace WebPresentationMVC.ViewModels {
    public class CreateHorarioConsultaFechadoViewModel {
        public CreateHorarioConsultaFechadoViewModel() { }

        public CreateHorarioConsultaFechadoViewModel(IEnumerable<MvcHorarioConsultaModel> horariosConsulta ) {
            this.SetHorariosConsultaAsSelectList(horariosConsulta);
        }

        public MvcHorarioConsultaFechadoModel HorarioConsultaFechado { get; set; }
        public IEnumerable<SelectListItem> HorariosConsultaList { get; set; }

        public void SetHorariosConsultaAsSelectList(IEnumerable<MvcHorarioConsultaModel> horariosConsulta)  {
            HorariosConsultaList = horariosConsulta.Select(e => new SelectListItem() {
                Value = e.Id.ToString(),
                Text = e.Materia + " --- " + e.Profesor.Surname + " " + e.Profesor.Firstname
            }) as IEnumerable<SelectListItem>;
        }
    }
}
