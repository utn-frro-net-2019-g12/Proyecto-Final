namespace Presentation.Library.Models
{
    public class Inscripcion {
        public int Id { get; set; }

        public string Topic { get; set; }
        public States? State { get; set; }
        public string Answer { get; set; }
        public string Observation { get; set; }   
      
        public int? AlumnoId { get; set; }
        public virtual Usuario Alumno { get; set; }

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
