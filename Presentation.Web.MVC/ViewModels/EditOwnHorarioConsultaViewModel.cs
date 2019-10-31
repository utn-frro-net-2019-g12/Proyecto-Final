using Presentation.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Web.MVC.ViewModels
{
    public class EditOwnHorarioConsultaViewModel
    {
        public EditOwnHorarioConsultaViewModel() { }

        public EditOwnHorarioConsultaViewModel(IEnumerable<MvcMateriaModel> materias, MvcHorarioConsultaModel horarioConsulta)
        {
            this.SetMateriasAsSelectList(materias);
            this.SetDiasSemanaAsSelectList();
            this.HorarioConsulta = horarioConsulta;
        }

        public MvcHorarioConsultaModel HorarioConsulta { get; set; }
        public IEnumerable<SelectListItem> MateriasList { get; set; }
        public IEnumerable<SelectListItem> DiasList { get; set; }

        public void SetMateriasAsSelectList(IEnumerable<MvcMateriaModel> materias)
        {
            MateriasList = materias.Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.Name
            }) as IEnumerable<SelectListItem>;
        }

        public void SetProfesor(int id_profesor)
        {
            HorarioConsulta.ProfesorId = id_profesor;
        }

        public void SetDiasSemanaAsSelectList() {
            List<string> dias = new List<string> { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };
            DiasList = dias.Select(e => new SelectListItem() { Value = e, Text = e }) as IEnumerable<SelectListItem>;
        }
    }
}
