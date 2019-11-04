using Presentation.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Web.MVC.ViewModels
{
    public class ShowMateriasViewModel
    {
        public ShowMateriasViewModel(
            IEnumerable<MvcMateriaModel> materias,
            string parcialDesc = null)
        {
            Materias = materias;
            ParcialDesc = parcialDesc;
        }

        public IEnumerable<MvcMateriaModel> Materias { get; set; }
        public string ParcialDesc { get; set; }
    }
}