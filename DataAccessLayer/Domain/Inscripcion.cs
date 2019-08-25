using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer {
    public class Inscripcion {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Topic { get; set; }
        public bool? State { get; set; }
        public string Answer { get; set; }
        public string Observation { get; set; }   
      
        // Usuarios with Legajo != Null Only
        [ForeignKey("Alumno")]
        public int AlumnoId { get; set; }
        public virtual Usuario Alumno { get; set; }

        [ForeignKey("HorarioConsulta")]
        public int HorarioConsultaId { get; set; }
        public virtual HorarioConsulta HorarioConsulta { get; set; }
    }
}
