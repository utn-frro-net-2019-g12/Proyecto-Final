﻿namespace Presentation.Library.Models
{
    public class HorarioConsulta {
        public int Id { get; set; }

        public string Weekday { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string Place { get; set; }
        public System.DateTime? EliminationDate { get; set; }

        public int? ProfesorId { get; set; }
        public virtual Usuario Profesor { get; set; }

        public int? MateriaId { get; set; }
        public virtual Materia Materia { get; set; }

        public string EliminationDateForDisplay {
            get {
                return this.EliminationDate.HasValue ? this.EliminationDate.Value.ToString("dd/MM/yyyy") : null;
            }
        }
    }
}
