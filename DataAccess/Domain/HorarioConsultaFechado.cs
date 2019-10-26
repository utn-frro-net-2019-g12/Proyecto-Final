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

        // HorarioConsultaFechado's Enumeration of States
        public enum HCFStates {
            active,
            postponed,
            canceled,
            finalized
        }
    }
}
