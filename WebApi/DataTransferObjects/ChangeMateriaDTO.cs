using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApi.DataTransferObjects
{
    public class ChangeMateriaDTO
    {
        [Required]
        public int? Year { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public bool? IsElectiva { get; set; }

        public int? DepartamentoId { get; set; }
        public virtual Departamento Departamento { get; set; }
    }
}