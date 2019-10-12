using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer {
    public class HorarioConsultaFechado {
        public int Id { get; set; }
        
        [Required]
        public DateTime Date { get; set; }

        [ForeignKey("HorarioConsulta")]
        public int HorarioConsultaId { get; set; }
        public virtual HorarioConsulta HorarioConsulta { get; set; }
    }
}
