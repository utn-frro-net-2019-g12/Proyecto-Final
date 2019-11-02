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

        public string IsAlumnoDisplay
        {
            get
            {
                if(Legajo == null)
                {
                    return "---";
                }
                else
                {
                    return Legajo.ToString();
                }
            }
        }

        public string Matricula { get; set; }

        public string IsProfesorDisplay
        {
            get
            {
                if (Matricula == null)
                {
                    return "---";
                }
                else
                {
                    return Matricula.ToString();
                }
            }
        }

        public bool? IsAdmin { get; set; }

        public string IsAdminDisplay
        {
            get
            {
                if (IsAdmin != true || IsAdmin == null)
                {
                    return "No";
                }
                else
                {
                    return "Si";
                }
            }
        }

        public string Firstname { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public int? Phone1 { get; set; }

        public int? Phone2 { get; set; }

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
