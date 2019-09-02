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

    [Authorize]
    public class UsuarioController : Controller {
        // Index - GET Usuario
        public ActionResult Index() {
            var response = GlobalApi.WebApiClient.GetAsync("usuarios").Result;

            IEnumerable<MvcUsuarioModel> usuarios = response.Content.ReadAsAsync<IEnumerable<MvcUsuarioModel>>().Result;

            return View(usuarios);
        }

        // Details - GET Usuario/ID
        public ActionResult Details(int id) {
            var response = GlobalApi.WebApiClient.GetAsync("usuarios/" + id.ToString()).Result;

            if (!response.IsSuccessStatusCode) {
                return View(response.Content.ReadAsAsync<ModelState>().Result);
            }

            var usuario = response.Content.ReadAsAsync<MvcUsuarioModel>().Result;
    
            return View(usuario);
        }

        // Delete - DELETE Usuario/ID
        [HttpPost]
        public ActionResult Delete(int id) {
            var response = GlobalApi.WebApiClient.DeleteAsync("usuarios/" + id.ToString()).Result;

            // Search what is TempData!
            TempData["SuccessMessage"] = "Deleted Sucessfully";

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // Create (Default)
        [HttpGet]
        public ActionResult Create() {
            return PartialView("_Create");
        }


        // Create - POST Usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MvcUsuarioModel usuario) {

            var account = new RegisterModel();
            account.Email = usuario.Email;
            account.Password = "defaultpassword";
            account.ConfirmPassword = "defaultpassword";

            var responseCreateAccount = GlobalApi.WebApiClient.PostAsJsonAsync("account/register", account).Result;

            if (responseCreateAccount.IsSuccessStatusCode) {
                var responseSaveUserData = GlobalApi.WebApiClient.PostAsJsonAsync("usuarios", usuario).Result;

                if (!responseSaveUserData.IsSuccessStatusCode) {
                    ModelState.AddModelErrorsFromResponse(responseSaveUserData);
                    return PartialView("_Create", usuario);
                }

            } else {
                // Move this to an action filter
                ModelState.AddModelErrorsFromResponse(responseCreateAccount);
                return PartialView("_Create", usuario);
            }

            return Content("OK");
        }

        // Edit - GET Usuario/ID
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

            return PartialView("_Edit", usuario);
        }

        // Edit - PUT Usuario/ID (Secured)
        [HttpPost]
        // Bind(Include = "...") is used to avoid overposting attacks
        public ActionResult Edit([Bind(Include = "Id, Username, Legajo, Matricula, IsAdmin, Firstname, Surname, Email, Phone1, Phone2")]MvcUsuarioModel usuario) {
            var response = GlobalApi.WebApiClient.PutAsJsonAsync("usuarios/" + usuario.Id, usuario).Result;

            if (!response.IsSuccessStatusCode) {
                ModelState.AddModelErrorsFromResponse(response);

                return PartialView("_Edit", usuario);
            }

            return Content("OK");
        }
        
    }
}
