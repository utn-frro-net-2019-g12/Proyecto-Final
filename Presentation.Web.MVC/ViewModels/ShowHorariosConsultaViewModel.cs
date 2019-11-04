using Presentation.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Web.MVC.ViewModels
{
    public class ShowHorariosConsultaViewModel
    {
        public ShowHorariosConsultaViewModel(
            IEnumerable<MvcHorarioConsultaModel> horariosConsulta,
            IEnumerable<MvcDepartamentoModel> departamentos,
            string parcialDesc = null, 
            int? departamentoId = null)
        {
            HorariosConsulta = horariosConsulta;
            ParcialDesc = parcialDesc;
            DepartamentoId = departamentoId;
            this.SetHorariosConsultaAsSelectList(departamentos);
        }

        public IEnumerable<MvcHorarioConsultaModel> HorariosConsulta { get; set; }
        public string ParcialDesc { get; set; }
        public int? DepartamentoId { get; set; }
        public IEnumerable<SelectListItem> DepartamentosList { get; set; }

        public void SetHorariosConsultaAsSelectList(IEnumerable<MvcDepartamentoModel> departamentos)
        {
            DepartamentosList = departamentos.Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.Name
            }) as IEnumerable<SelectListItem>;
        }
    }
}