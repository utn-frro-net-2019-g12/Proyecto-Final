using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Presentation.Web.MVC.Models;
using static Presentation.Library.Models.HorarioConsultaFechado;

namespace Presentation.Web.MVC.ViewModels {
    public class CreateHorarioConsultaFechadoViewModel {
        public CreateHorarioConsultaFechadoViewModel() { }

        public CreateHorarioConsultaFechadoViewModel(IEnumerable<MvcHorarioConsultaModel> horariosConsulta ) {
            this.SetHorariosConsultaAsSelectList(horariosConsulta);
            this.SetEstadosAsSelectList();
        }

        public MvcHorarioConsultaFechadoModel HorarioConsultaFechado { get; set; }
        public IEnumerable<SelectListItem> HorariosConsultaList { get; set; }
        public IEnumerable<SelectListItem> EstadosList { get; set; }

        public void SetHorariosConsultaAsSelectList(IEnumerable<MvcHorarioConsultaModel> horariosConsulta)  {
            HorariosConsultaList = horariosConsulta.Select(e => new SelectListItem() {
                Value = e.Id.ToString(),
                Text = e.Materia + " /// " + e.Profesor.Surname + " " + e.Profesor.Firstname
            }) as IEnumerable<SelectListItem>;
        }

        public void SetEstadosAsSelectList() {
            HCFStates[] estadosValues = (HCFStates[])Enum.GetValues(typeof(HCFStates));
            var estadosDict = from value in estadosValues select new { Value = (int)value, Text = value.ToString() };

            EstadosList = estadosDict.Select(e => new SelectListItem() {
                Value = e.Value.ToString(), // Lo dejo como String porque sino no funciona
                Text = e.Text
            }) as IEnumerable<SelectListItem>;
        }
    }
}
