using Presentation.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Web.MVC.ViewModels
{
    public class ShowInscripcionesViewModel
    {
        public ShowInscripcionesViewModel(
            IEnumerable<MvcInscripcionModel> inscripciones,
            string parcialDesc = null)
        {
            Inscripciones = inscripciones;
            ParcialDesc = parcialDesc;
        }

        public IEnumerable<MvcInscripcionModel> Inscripciones { get; set; }
        public string ParcialDesc { get; set; }
    }
}