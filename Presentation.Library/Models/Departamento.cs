using System.Collections.Generic;

namespace Presentation.Library.Models
{
    public class Departamento {
        public Departamento() {
            Materias = new HashSet<Materia>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Materia> Materias { get; set; }
    }
}
