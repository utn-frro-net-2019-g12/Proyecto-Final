using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Desktop.WPF.Models {
    public class WpfHorarioConsultaModel {
        public int Id { get; set; }
        public string Weekday { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string Place { get; set; }
        public string EliminationDate { get; set; }

        // Usuarios with Matricula != Null Only
        public int ProfesorId { get; set; }
        public virtual WpfUsuarioModel Profesor { get; set; }

        public int MateriaId { get; set; }
        public virtual WpfMateriaModel Materia { get; set; }
    }
}
