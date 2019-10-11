namespace Presentation.Library.Models
{
    public class Materia {
        public int Id { get; set; }

        public int? Year { get; set; }
        public string Name { get; set; }
        public bool? IsElectiva { get; set; }
        public int? DepartamentoId { get; set; }
        public virtual Departamento Departamento { get; set; }
    }
}
