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

        public string DateForDisplay {
            get {
                return this.Date.ToString("dd/MM/yyyy");
            }
        }

        public string PostponementDateForDisplay {
            get {
                return this.PostponementDate.HasValue ? this.PostponementDate.Value.ToString("dd/MM/yyyy") : null;
            }
        }

        // HorarioConsultaFechado's Enumeration of States
        public enum HCFStates {
            Active,
            Postponed,
            Canceled,
            Finalized
        }
    }
}
