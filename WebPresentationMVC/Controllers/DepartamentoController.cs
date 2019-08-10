using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebPresentationMVC.Models;

namespace WebPresentationMVC.Controllers {
    public class DepartamentoController : Controller {
        // GET: Departamento
        public ActionResult Index() {
            var response = GlobalApi.WebApiClient.GetAsync("departamentos/").Result;

            IEnumerable<MvcDepartamentoModel> departamentos = response.Content.ReadAsAsync<IEnumerable<MvcDepartamentoModel>>().Result;

            return View(departamentos);
        }

        // DETAILS
        public ActionResult Details(int id) {
            var response = GlobalApi.WebApiClient.GetAsync("departamentos/" + id.ToString()).Result;

            if (!response.IsSuccessStatusCode) {
                return View(response.Content.ReadAsAsync<ModelState>().Result);
            }

            var departamento = response.Content.ReadAsAsync<MvcDepartamentoModel>().Result;
    
            return View(departamento);
        }

        // DELETE Departamento/5
        public ActionResult Delete(int id) {
            var response = GlobalApi.WebApiClient.DeleteAsync("departamentos/" + id.ToString()).Result;

            // Search what is TempData!
            TempData["SuccessMessage"] = "Deleted Sucessfully";

            return RedirectToAction("Index");
        }

        // CREATE (Default)
        [HttpGet]
        public ActionResult Create() {
            return View();
        }

        // CREATE
        [HttpPost]
        public ActionResult Create(MvcDepartamentoModel departamentos) {
            var response = GlobalApi.WebApiClient.PostAsJsonAsync("departamentos", departamentos).Result;

            // Move this to an action filter
            if (!response.IsSuccessStatusCode) {
                ModelState.AddModelErrorsFromResponse(response);

                return View(departamentos);
            }

            return RedirectToAction("Index");
        }

        // EDIT
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

            return View(departamento);
        }

        // SECURE EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind(Include = "...") is used to avoid overposting attacks
        public ActionResult Edit([Bind(Include = "Id, Name")]MvcDepartamentoModel departamento) {
            var response = GlobalApi.WebApiClient.PutAsJsonAsync("departamentos/" + departamento.Id, departamento).Result;

            if (!response.IsSuccessStatusCode) {
                ModelState.AddModelErrorsFromResponse(response);

                return View(departamento);
            }

            return RedirectToAction("Index");
        }
    }
}
