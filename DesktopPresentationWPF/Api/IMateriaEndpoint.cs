using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopPresentationWPF.Models;

namespace DesktopPresentationWPF.Api
{
    public interface IMateriaEndpoint
    {
        Task<BindingList<WpfMateriaModel>> GetAll();
        Task Delete(object id);
        Task Post(WpfMateriaModel materia);
        Task Put(WpfMateriaModel materia);
    }
}
