using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Presentation.Desktop.WPF.Models {
    public class WpfInscripcionModel : INotifyPropertyChanged {
        public int Id { get; set; }
        public string Topic { get; set; }
        public InscripcionStates? State { get; set; }
        public string Answer { get; set; }
        public string Observation { get; set; }

        // Usuarios with Legajo != Null Only
        public int AlumnoId { get; set; }
        public virtual WpfUsuarioModel Alumno { get; set; }

        public int HorarioConsultaFechadoId { get; set; }
        public virtual WpfHorarioConsultaFechadoModel HorarioConsultaFechado { get; set; }

        // Inscripcion's Enumeration of States
        public enum InscripcionStates {
            Active,
            Canceled,
            Finalized
        }

        public override bool Equals(object o) {
            var item = o as WpfInscripcionModel;

            if (item == null) {
                return false;
            }

            return this.Id.Equals(item.Id);
        }

        public override int GetHashCode() {
            return this.Id.GetHashCode();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void CallPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
