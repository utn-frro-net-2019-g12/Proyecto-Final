namespace Presentation.Library.Models
{
    public class Inscripcion {
        public int Id { get; set; }

        public string Topic { get; set; }
        public InscripcionStates? State { get; set; }
        public string Answer { get; set; }
        public string Observation { get; set; }   
      
        public int? AlumnoId { get; set; }
        public virtual Usuario Alumno { get; set; }

        public int? HorarioConsultaFechadoId { get; set; }
        public virtual HorarioConsultaFechado HorarioConsultaFechado { get; set; }

        // Inscripcion's Enumeration of States
        public enum InscripcionStates {
            Active,
            Canceled,
            Finalized
        }
    }
}
