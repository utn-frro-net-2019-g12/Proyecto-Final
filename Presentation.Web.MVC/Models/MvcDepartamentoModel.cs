using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Presentation.Web.MVC.Models {
    public class MvcDepartamentoModel {
        public int Id { get; set; }

        [DisplayName("Nombre")]
        public string Name { get; set; }
    }
}
