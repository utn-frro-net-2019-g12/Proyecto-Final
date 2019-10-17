using Presentation.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Web.MVC.ViewModels
{
    public class CreateOwnHorarioConsultaViewModel
    {
        public CreateOwnHorarioConsultaViewModel() { }

        public CreateOwnHorarioConsultaViewModel(IEnumerable<MvcMateriaModel> materias)
        {
            this.SetMateriasAsSelectList(materias);
        }

        public MvcHorarioConsultaModel HorarioConsulta { get; set; }
        public IEnumerable<SelectListItem> MateriasList { get; set; }

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
    }
}