using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPresentationWPF.Models
{
    class UsuarioLoggedInModel : IUsuarioLoggedModel
    {
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Token { get; set; }

        public void LogOffUser()
        {
            Username = "";
            Firstname = "";
            Surname = "";
            Token = "";
        }
    }
}
