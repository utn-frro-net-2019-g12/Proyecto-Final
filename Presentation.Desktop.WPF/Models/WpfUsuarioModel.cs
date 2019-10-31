using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Presentation.Desktop.WPF.Models {
    public class WpfUsuarioModel : INotifyPropertyChanged {
        public int Id { get; set; }

        public string Username { get; set; }

        public int? Legajo { get; set; }

        public string Matricula { get; set; }

        public bool IsAdmin { get; set; }

        public string Firstname { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public int? Phone { get; set; }

        public override bool Equals(object o) {
            var item = o as WpfUsuarioModel;

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
