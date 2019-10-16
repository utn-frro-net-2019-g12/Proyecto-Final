using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess {
    public class Inscripcion {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Topic { get; set; }
        public States? State { get; set; }
        public string Answer { get; set; }
        public string Observation { get; set; }

        // Usuarios with Legajo != Null Only
        [Required]
        [ForeignKey("Alumno")]
        public int? AlumnoId { get; set; }
        public virtual Usuario Alumno { get; set; }

        [Required]
        [ForeignKey("HorarioConsultaFechado")]
        public int? HorarioConsultaFechadoId { get; set; }
        public virtual HorarioConsultaFechado HorarioConsultaFechado { get; set; }

        // States Enumeration
        public enum States {
            active,
            canceled,
            finalized
        }
    }
}
