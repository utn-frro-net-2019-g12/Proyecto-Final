using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesktopPresentationWPF.Models {
    public class WpfMateriaModel {
        public int Id { get; set; }

        public int Year { get; set; }
        public string Name { get; set; }
        public bool IsElectiva { get; set; }
    }
}
