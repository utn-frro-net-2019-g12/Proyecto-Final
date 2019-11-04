using Presentation.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Web.MVC.ViewModels
{
    public class ShowDepartamentosViewModel
    {
        public ShowDepartamentosViewModel(
            IEnumerable<MvcDepartamentoModel> departamentos,
            string parcialDesc = null)
        {
            Departamentos = departamentos;
            ParcialDesc = parcialDesc;
        }

        public IEnumerable<MvcDepartamentoModel> Departamentos { get; set; }
        public string ParcialDesc { get; set; }
    }
}