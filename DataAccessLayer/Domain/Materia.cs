using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer
{
    public class Materia
    {
        public int Id { get; set; }

        [Required]
        public int? Year { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public bool? IsElectiva { get; set; }
    }
}
