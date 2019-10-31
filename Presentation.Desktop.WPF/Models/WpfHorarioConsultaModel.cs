using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Presentation.Desktop.WPF.Models {
    public class WpfHorarioConsultaModel : INotifyPropertyChanged {
        public int Id { get; set; }
        public string Weekday { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string Place { get; set; }
        public DateTime? EliminationDate { get; set; }

        // Usuarios with Matricula != Null Only
        public int ProfesorId { get; set; }
        public virtual WpfUsuarioModel Profesor { get; set; }

        public int MateriaId { get; set; }
        public virtual WpfMateriaModel Materia { get; set; }

        public string EliminationDateForDisplay {
            get {
                return this.EliminationDate.HasValue ? this.EliminationDate.Value.ToString("dd/MM/yyyy") : null;
            }
        }

        public override bool Equals(object o) {
            var item = o as WpfHorarioConsultaModel;

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
