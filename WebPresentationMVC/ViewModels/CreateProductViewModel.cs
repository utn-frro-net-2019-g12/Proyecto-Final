using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPresentationMVC.Models;

namespace WebPresentationMVC.ViewModels
{
    public class CreateMateriaViewModel
    {
        public CreateMateriaViewModel() { }

        public CreateMateriaViewModel(IEnumerable<MvcDepartamentoModel> departamentos)
        {
            this.SetDepartamentosAsSelectList(departamentos);
        }

        public MvcMateriaModel Materia { get; set; }
        public int? VendorId { get; set; }
        public IEnumerable<SelectListItem> DepartamentosList { get; set; }
        public void SetDepartamentosAsSelectList(IEnumerable<MvcDepartamentoModel> departamentos)
        {
            DepartamentosList = departamentos.Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.Name
            }) as IEnumerable<SelectListItem>;
        }
    }
}