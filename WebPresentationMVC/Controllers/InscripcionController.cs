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

    // Note: This Controller Communicates with ViewModels (CreateInscripcionViewModel and EditInscripcionViewModel)
    [Authorize]
    public class InscripcionController : Controller {
        // Index - GET Inscripcion
        public ActionResult Index() {
            var response = GlobalApi.WebApiClient.GetAsync("inscripciones").Result;

            IEnumerable<MvcInscripcionModel> inscripciones = response.Content.ReadAsAsync<IEnumerable<MvcInscripcionModel>>().Result;

            return View(inscripciones);
        }

        // Details - GET Inscripcion/ID
        public ActionResult Details(int id) {
            var response = GlobalApi.WebApiClient.GetAsync("inscripciones/" + id.ToString()).Result;

            if (!response.IsSuccessStatusCode) {
                return View(response.Content.ReadAsAsync<ModelState>().Result);
            }

            var inscripcion = response.Content.ReadAsAsync<MvcInscripcionModel>().Result;

            return View(inscripcion);
        }

        // Delete - DELETE Inscripcion/ID
        public ActionResult Delete(int id) {
            var response = GlobalApi.WebApiClient.DeleteAsync("inscripciones/" + id.ToString()).Result;

            // Search what is TempData!
            TempData["SuccessMessage"] = "Deleted Sucessfully";

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // Create (Default)
        [HttpGet]
        public ActionResult Create() {
            var alumnos = GetAlumnos();
            var horariosConsulta = GetHorariosConsulta();

            var viewModel = new CreateInscripcionViewModel(alumnos, horariosConsulta);

            return PartialView("_Create", viewModel);
        }

        // Create - Post Inscripcion
        [HttpPost]
        public ActionResult Create(CreateInscripcionViewModel viewModel) {
            var response = GlobalApi.WebApiClient.PostAsJsonAsync("inscripciones", viewModel.Inscripcion).Result;

            // Move this to an action filter
            if (!response.IsSuccessStatusCode) {
                var alumnos = GetAlumnos();
                var horariosConsulta = GetHorariosConsulta();

                viewModel.SetAlumnosAsSelectList(alumnos);
                viewModel.SetHorariosConsultaAsSelectList(horariosConsulta);

                ModelState.AddModelErrorsFromResponse(response);

                return PartialView("_Create", viewModel);
            }

            return Content("OK");
        }

        // Edit - GET Inscripcion/ID
        [HttpGet]
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var response = GlobalApi.WebApiClient.GetAsync("inscripciones/" + id).Result;

            if (!response.IsSuccessStatusCode) {
                return HttpNotFound();
            }

            MvcInscripcionModel inscripcion = response.Content.ReadAsAsync<MvcInscripcionModel>().Result;

            var alumnos = GetAlumnos();
            var horariosConsulta = GetHorariosConsulta();

            var viewModel = new EditInscripcionViewModel(alumnos, horariosConsulta, inscripcion);

            return PartialView("_Edit", viewModel);

        }

        // Edit - PUT Inscripcion/ID (Secured)
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind(Include = "...") is used to avoid overposting attacks
        public ActionResult Edit(EditInscripcionViewModel viewModel) {
            var response = GlobalApi.WebApiClient.PutAsJsonAsync("inscripciones/" + viewModel.Inscripcion.Id, viewModel.Inscripcion).Result;

            if (!response.IsSuccessStatusCode) {
                var alumnos = GetAlumnos();
                var horariosConsulta = GetHorariosConsulta();
                viewModel.SetAlumnosAsSelectList(alumnos);
                viewModel.SetHorariosConsultaAsSelectList(horariosConsulta);

                ModelState.AddModelErrorsFromResponse(response);

                return PartialView("_Edit", viewModel);
            }

            return Content("OK");

        }

        // List of Alumnos - GET Alumnos
        // NOTA: Testear si funciona el LinQ para filtrar solo Alumnos
        public IEnumerable<MvcUsuarioModel> GetAlumnos() {
            var response = GlobalApi.WebApiClient.GetAsync("usuarios").Result;

            return response.Content.ReadAsAsync<IEnumerable<MvcUsuarioModel>>().Result;
        }

        // List of HorariosConsulta - GET HorariosConsulta
        public IEnumerable<MvcHorarioConsultaModel> GetHorariosConsulta() {
            var response = GlobalApi.WebApiClient.GetAsync("horariosConsulta").Result;

            return response.Content.ReadAsAsync<IEnumerable<MvcHorarioConsultaModel>>().Result;
        }
    }
}
