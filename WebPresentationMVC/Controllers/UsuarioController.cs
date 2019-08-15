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
    public class UsuarioController : Controller {
        // GET: Usuario
        public ActionResult Index() {
            var response = GlobalApi.WebApiClient.GetAsync("usuarios").Result;

            IEnumerable<MvcUsuarioModel> usuarios = response.Content.ReadAsAsync<IEnumerable<MvcUsuarioModel>>().Result;

            return View(usuarios);
        }

        // DETAILS
        public ActionResult Details(int id) {
            var response = GlobalApi.WebApiClient.GetAsync("usuarios/" + id.ToString()).Result;

            if (!response.IsSuccessStatusCode) {
                return View(response.Content.ReadAsAsync<ModelState>().Result);
            }

            var usuario = response.Content.ReadAsAsync<MvcUsuarioModel>().Result;
    
            return View(usuario);
        }

        // DELETE Usuario/5
        public ActionResult Delete(int id) {
            var response = GlobalApi.WebApiClient.DeleteAsync("usuarios/" + id.ToString()).Result;

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
        public ActionResult Create(MvcUsuarioModel usuarios) {
            var response = GlobalApi.WebApiClient.PostAsJsonAsync("usuarios", usuarios).Result;

            // Move this to an action filter
            if (!response.IsSuccessStatusCode) {
                ModelState.AddModelErrorsFromResponse(response);

                return View(usuarios);
            }

            return RedirectToAction("Index");
        }

        // EDIT
        [HttpGet]
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var response = GlobalApi.WebApiClient.GetAsync("usuarios/" + id).Result;

            if (!response.IsSuccessStatusCode) {
                return HttpNotFound();
            }

            MvcUsuarioModel usuario = response.Content.ReadAsAsync<MvcUsuarioModel>().Result;

            return View(usuario);
        }

        // MODAL EDIT
        [HttpGet]
        public ActionResult Modify() {
            return PartialView("_Edit");
        }

        // SECURE EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind(Include = "...") is used to avoid overposting attacks
        public ActionResult Edit([Bind(Include = "Id, Username, Legajo, Matricula, IsAdmin, Firstname, Surname, Email, Phone")]MvcUsuarioModel usuario) {
            var response = GlobalApi.WebApiClient.PutAsJsonAsync("usuarios/" + usuario.Id, usuario).Result;

            if (!response.IsSuccessStatusCode) {
                ModelState.AddModelErrorsFromResponse(response);

                return View(usuario);
            }

            return RedirectToAction("Index");
        }
        
    }
}
