namespace Presentation.Library.Models
{
    public class HorarioConsultaFechado {
        public int Id { get; set; }

        public System.DateTime Date { get; set; }
        public HCFStates? State { get; set; }
        public string Observation { get; set; }
        public System.DateTime? PostponementDate { get; set; }

        public int? HorarioConsultaId { get; set; }
        public virtual HorarioConsulta HorarioConsulta { get; set; }

        // HorarioConsultaFechado's Enumeration of States
        public enum HCFStates {
            active,
            postponed,
            canceled,
            finalized
        }
    }
}
