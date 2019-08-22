using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPresentationWPF.Models
{
    public interface IUsuarioLoggedModel
    {
        string Username { get; set; }

        string Firstname { get; set; }

        string Surname { get; set; }

        string Token { get; set; }

        void LogOffUser();
    }
}
