using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer {
    public class Materia {
        public int Id { get; set; }

        [Required]
        public int? Year { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public bool? IsElectiva { get; set; }

        [Required]
        [ForeignKey("Departamento")]
        public int? DepartamentoId { get; set; }
        public virtual Departamento Departamento { get; set; }
    }
}
