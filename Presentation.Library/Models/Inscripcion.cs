namespace Presentation.Library.Models
{
    public class Inscripcion {
        public int Id { get; set; }

        public string Topic { get; set; }
        public bool? State { get; set; }
        public string Answer { get; set; }
        public string Observation { get; set; }   
      
        public int AlumnoId { get; set; }
        public virtual Usuario Alumno { get; set; }

        public int HorarioConsultaId { get; set; }
        public virtual HorarioConsulta HorarioConsulta { get; set; }
    }
}
