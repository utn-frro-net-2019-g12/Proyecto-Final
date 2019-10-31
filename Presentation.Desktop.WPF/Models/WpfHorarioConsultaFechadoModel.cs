using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Presentation.Desktop.WPF.Models {
    public class WpfHorarioConsultaFechadoModel : INotifyPropertyChanged {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public HCFStates? State { get; set; }
        public string Observation { get; set; }
        public System.DateTime? PostponementDate { get; set; }

        public int HorarioConsultaId { get; set; }
        public virtual WpfHorarioConsultaModel HorarioConsulta { get; set; }

        public string DateForDisplay {
            get {
                return this.Date.ToString("dd/MM/yyyy");
            }
        }

        public string PostponementDateForDisplay {
            get {
                return this.PostponementDate.HasValue ? this.PostponementDate.Value.ToString("dd/MM/yyyy") : null;
            }
        }

        // HorarioConsultaFechado's Enumeration of States
        public enum HCFStates {
            Active,
            Postponed,
            Canceled,
            Finalized
        }

        public override bool Equals(object o) {
            var item = o as WpfHorarioConsultaFechadoModel;

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
