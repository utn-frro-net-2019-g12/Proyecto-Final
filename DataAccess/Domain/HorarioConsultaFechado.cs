using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess {
    public class HorarioConsultaFechado {
        public int Id { get; set; }
        
        [Required]
        public System.DateTime Date { get; set; }
        public HCFStates? State { get; set; }
        public string Observation { get; set; }
        public System.DateTime? PostponementDate { get; set; }

        [Required]
        [ForeignKey("HorarioConsulta")]
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
