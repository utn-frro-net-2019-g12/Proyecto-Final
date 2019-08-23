using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DesktopPresentationWPF.Models {
    public class WpfMateriaModel : INotifyPropertyChanged {
        private string _name;

        public int Id { get; set; }

        public int Year { get; set; }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                CallPropertyChanged(nameof(Name));
            }
        }

        public bool IsElectiva { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void CallPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
