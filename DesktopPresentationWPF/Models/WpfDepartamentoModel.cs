using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesktopPresentationWPF.Models {
    public class WpfDepartamentoModel {
        public WpfDepartamentoModel() {
            Materias = new HashSet<WpfMateriaModel>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<WpfMateriaModel> Materias { get; set; }
        // Testear si anda todo esto...
    }
}
