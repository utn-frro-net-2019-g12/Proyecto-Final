using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPresentationWPF.Models
{
    public interface IUsuarioLogged
    {
        string Username { get; set; }
        string Token { get; set; }

        void Set(string userName, string token);
        void LogOffUser();
    }
}
