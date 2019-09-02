using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebPresentationMVC.Models;
using WebPresentationMVC.ViewModels;

namespace WebPresentationMVC.Controllers {

    // Note: This Controller Communicates with ViewModels (CreateMateriaViewModel and EditMateriaViewModel)
    [Authorize]
    public class MateriaController : Controller {
        // Index - GET Materia
        public ActionResult Index() {
            var response = GlobalApi.WebApiClient.GetAsync("materias/departamento").Result;

            IEnumerable<MvcMateriaModel> materias = response.Content.ReadAsAsync<IEnumerable<MvcMateriaModel>>().Result;

            return View(materias);
        }

        // Details - GET Materia/ID
        public ActionResult Details(int id) {
            var response = GlobalApi.WebApiClient.GetAsync("materias/" + id.ToString() + "/departamento").Result;

            if (!response.IsSuccessStatusCode) {
                return View(response.Content.ReadAsAsync<ModelState>().Result);
            }

            var materia = response.Content.ReadAsAsync<MvcMateriaModel>().Result;
    
            return View(materia);
        }

        // Delete - DELETE Materia/ID
        public ActionResult Delete(int id) {
            var response = GlobalApi.WebApiClient.DeleteAsync("materias/" + id.ToString()).Result;

            // Search what is TempData!
            TempData["SuccessMessage"] = "Deleted Sucessfully";

            return RedirectToAction("Index");
        }

        // Create (Default)
        [HttpGet]
        public ActionResult Create() {
            var departamentos = GetDepartamentos();

            var viewModel = new CreateMateriaViewModel(departamentos);

            return View(viewModel);
        }

        // Create - Post Materia
        [HttpPost]
        public ActionResult Create(CreateMateriaViewModel viewModel) {
            var response = GlobalApi.WebApiClient.PostAsJsonAsync("materias", viewModel.Materia).Result;

            // Move this to an action filter
            if (!response.IsSuccessStatusCode) {
                var departamentos = GetDepartamentos();

                viewModel.SetDepartamentosAsSelectList(departamentos);

                ModelState.AddModelErrorsFromResponse(response);

                return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        // Edit - GET Materia/ID
        [HttpGet]
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var response = GlobalApi.WebApiClient.GetAsync("materias/" + id).Result;

            if (!response.IsSuccessStatusCode) {
                return HttpNotFound();
            }

            MvcMateriaModel materia = response.Content.ReadAsAsync<MvcMateriaModel>().Result;

            var departamentos = GetDepartamentos();

            var viewModel = new EditMateriaViewModel(departamentos, materia);

            return View(viewModel);
            // Edit for Partial View!
        }

        // Edit - PUT Materia/ID (Secured)
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind(Include = "...") is used to avoid overposting attacks
        public ActionResult Edit(EditMateriaViewModel viewModel) {
            var response = GlobalApi.WebApiClient.PutAsJsonAsync("materias/" + viewModel.Materia.Id, viewModel.Materia).Result;

            if (!response.IsSuccessStatusCode) {
                var departamentos = GetDepartamentos();
                viewModel.SetDepartamentosAsSelectList(departamentos);

                ModelState.AddModelErrorsFromResponse(response);

                return View (viewModel);
            }

            return RedirectToAction("Index");
            // Edit for Partial View!
        }

        // List of Departamentos - GET Departamentos
        public IEnumerable<MvcDepartamentoModel> GetDepartamentos() {
            var response = GlobalApi.WebApiClient.GetAsync("departamentos").Result;

            return response.Content.ReadAsAsync<IEnumerable<MvcDepartamentoModel>>().Result;
        }
    }
}
