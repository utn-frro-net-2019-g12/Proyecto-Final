using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPresentationMVC.Models;

namespace WebPresentationMVC.ViewModels {
    public class EditMateriaViewModel {
        public EditMateriaViewModel() { }

        public EditMateriaViewModel(IEnumerable<MvcDepartamentoModel> departamentos, MvcMateriaModel materia) {
            this.SetDepartamentosAsSelectList(departamentos);
            this.Materia = materia;
        }

        public MvcMateriaModel Materia { get; set; }
        public int? VendorId { get; set; }
        public IEnumerable<SelectListItem> DepartamentosList { get; set; }
        public void SetDepartamentosAsSelectList(IEnumerable<MvcDepartamentoModel> departamentos) {
            DepartamentosList = departamentos.Select(e => new SelectListItem() {
                Value = e.Id.ToString(),
                Text = e.Name
            }) as IEnumerable<SelectListItem>;
        }
    }
}
