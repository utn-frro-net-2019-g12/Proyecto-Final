﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess {
    public class HorarioConsulta {
        public int Id { get; set; }

        [Required]
        public string Weekday { get; set; }
        [Required]
        public string StartHour { get; set; }
        [Required]
        public string EndHour { get; set; }
        [Required]
        [StringLength(50)]
        public string Place { get; set; }
        public DateTime? EliminationDate { get; set; }

        // Usuarios with Matricula != Null Only
        [Required]
        [ForeignKey("Profesor")]
        public int? ProfesorId { get; set; }
        public virtual Usuario Profesor { get; set; }

        [Required]
        [ForeignKey("Materia")]
        public int? MateriaId { get; set; }
        public virtual Materia Materia { get; set; }

        public string EliminationDateForDisplay {
            get {
                return this.EliminationDate.HasValue ? this.EliminationDate.Value.ToString("dd/MM/yyyy") : null;
            }
        }
    }
}
