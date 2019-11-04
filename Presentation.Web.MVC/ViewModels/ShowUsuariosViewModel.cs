using Presentation.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Web.MVC.ViewModels
{
    public class ShowUsuariosViewModel
    {
        public ShowUsuariosViewModel(
            IEnumerable<MvcUsuarioModel> usuarios,
            string parcialDesc = null)
        {
            Usuarios = usuarios;
            ParcialDesc = parcialDesc;
        }

        public IEnumerable<MvcUsuarioModel> Usuarios { get; set; }
        public string ParcialDesc { get; set; }
    }
}