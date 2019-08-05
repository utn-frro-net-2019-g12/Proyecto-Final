using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.DataTransferObjects
{
    public class ShowMateriaDTO
    {
        public int Id { get; set; }

        public int? Year { get; set; }
        public string Name { get; set; }
        public bool? IsElectiva { get; set; }

        public int? DepartamentoId { get; set; }
        public virtual Departamento Departamento { get; set; }
    }
}