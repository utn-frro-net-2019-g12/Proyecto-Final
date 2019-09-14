using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebPresentationMVC.Models;
using WebPresentationMVC.Api;

namespace WebPresentationMVC.Controllers {

    [Authorize]
    public class DepartamentoController : Controller {

        // Index - GET Departamento
        public ActionResult Index() {
            var response = GlobalApi.WebApiClient.GetAsync("departamentos/").Result;

            IEnumerable<MvcDepartamentoModel> departamentos = response.Content.ReadAsAsync<IEnumerable<MvcDepartamentoModel>>().Result;

            return View(departamentos);
        }

        // Details - GET Departamento/ID
        public ActionResult Details(int id) {
            var response = GlobalApi.WebApiClient.GetAsync("departamentos/" + id.ToString()).Result;

            if (!response.IsSuccessStatusCode) {
                return View(response.Content.ReadAsAsync<ModelState>().Result);
            }

            var departamento = response.Content.ReadAsAsync<MvcDepartamentoModel>().Result;
    
            return View(departamento);
        }

        // Delete - DELETE Departamento/ID
        [HttpPost]
        public ActionResult Delete(int id) {
            var response = GlobalApi.WebApiClient.DeleteAsync("departamentos/" + id.ToString()).Result;

            // Search what is TempData!
            TempData["SuccessMessage"] = "Deleted Sucessfully";

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // Create (Default)
        [HttpGet]
        public ActionResult Create() {
            return PartialView("_Create");
        }

        // Create - POST Departamento
        [HttpPost]
        public ActionResult Create(MvcDepartamentoModel departamentos) {
            var response = GlobalApi.WebApiClient.PostAsJsonAsync("departamentos", departamentos).Result;

            // Move this to an action filter
            if (!response.IsSuccessStatusCode) {
                ModelState.AddModelErrorsFromResponse(response);

                return PartialView("_Create");
            }

            return Content("OK");
        }

        // Edit - GET Departamento/ID
        [HttpGet]
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var response = GlobalApi.WebApiClient.GetAsync("departamentos/" + id).Result;

            if (!response.IsSuccessStatusCode) {
                return HttpNotFound();
            }

            MvcDepartamentoModel departamento = response.Content.ReadAsAsync<MvcDepartamentoModel>().Result;

            return PartialView("_Edit", departamento);
        }

        // Edit - PUT Departamento/ID (Secured)
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind(Include = "...") is used to avoid overposting attacks
        public ActionResult Edit([Bind(Include = "Id, Name")]MvcDepartamentoModel departamento) {
            var response = GlobalApi.WebApiClient.PutAsJsonAsync("departamentos/" + departamento.Id, departamento).Result;

            if (!response.IsSuccessStatusCode) {
                ModelState.AddModelErrorsFromResponse(response);

                return PartialView("_Edit", departamento);
            }

            return Content("OK");
        }
    }
}
