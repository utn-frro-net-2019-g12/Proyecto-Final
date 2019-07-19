using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer
{
    public class Departamento
    {
        public Departamento()
        {
            Materias = new HashSet<Materia>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Materia> Materias { get; set; }
    }
}
