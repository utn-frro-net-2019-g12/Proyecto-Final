namespace Presentation.Library.Models
{
    public class HorarioConsultaFechado {
        public int Id { get; set; }

        public System.DateTime Date { get; set; }

        public int HorarioConsultaId { get; set; }
        public virtual HorarioConsulta HorarioConsulta { get; set; }
    }
}
