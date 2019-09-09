using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPresentationMVC.Models {
    public class MvcMateriaModel {
        public int Id { get; set; }
        [DisplayName("Año")]
        public int? Year { get; set; }
        [DisplayName("Nombre")]
        public string Name { get; set; }
        [DisplayName("¿Es electiva?")]
        public bool IsElectiva { get; set; }

        [ForeignKey("Departamento")]
        public int? DepartamentoId { get; set; }
        public virtual MvcDepartamentoModel Departamento { get; set; }
    }
}
