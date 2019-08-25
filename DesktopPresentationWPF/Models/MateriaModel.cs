using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPresentationWPF.Models
{
    public class MateriaModel
    {
        public int Id { get; set; }

        public int? Year { get; set; }
        public string Name { get; set; }
        public bool IsElectiva { get; set; }

        public int? DepartamentoId { get; set; }
    }
}
