using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebPresentationMVC.Api;
using WebPresentationMVC.Models;
using WebPresentationMVC.ViewModels;

namespace WebPresentationMVC.Controllers {

    // Note: This Controller Communicates with ViewModels (CreateHorarioConsultaViewModel and EditHorarioConsultaViewModel)
    [Authorize]
    public class HorarioConsultaController : Controller {
        private IMateriaEndpoint _materiaEndpoint;

        public HorarioConsultaController(IMateriaEndpoint materiaEndpoint)
        {
            _materiaEndpoint = materiaEndpoint;
        }

        // Index - GET HorarioConsulta
        public ActionResult Index() {
            var response = GlobalApi.WebApiClient.GetAsync("horariosConsulta").Result;

            IEnumerable<MvcHorarioConsultaModel> horariosConsulta = response.Content.ReadAsAsync<IEnumerable<MvcHorarioConsultaModel>>().Result;

            return View(horariosConsulta);
        }

        // Details - GET HorarioConsulta/ID
        public ActionResult Details(int id) {
            var response = GlobalApi.WebApiClient.GetAsync("horariosConsulta/" + id.ToString()).Result;

            if (!response.IsSuccessStatusCode) {
                return View(response.Content.ReadAsAsync<ModelState>().Result);
            }

            var horarioConsulta = response.Content.ReadAsAsync<MvcHorarioConsultaModel>().Result;

            return View(horarioConsulta);
        }

        // Delete - DELETE HorarioConsulta/ID
        public ActionResult Delete(int id) {
            var response = GlobalApi.WebApiClient.DeleteAsync("horariosConsulta/" + id.ToString()).Result;

            // Search what is TempData!
            TempData["SuccessMessage"] = "Deleted Sucessfully";

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // Create (Default)
        [HttpGet]
        public async Task<ActionResult> Create() {
            var profesores = GetProfesores();
            var materias = await _materiaEndpoint.GetAll(); // May throw an exception, so that is why the modal is not showing in nico user

            var viewModel = new CreateHorarioConsultaViewModel(profesores, materias);

            return PartialView("_Create", viewModel);
        }

        // Create - Post HorarioConsulta
        [HttpPost]
        public async Task<ActionResult> Create(CreateHorarioConsultaViewModel viewModel) {
            var response = GlobalApi.WebApiClient.PostAsJsonAsync("horariosConsulta", viewModel.HorarioConsulta).Result;

            // Move this to an action filter
            if (!response.IsSuccessStatusCode) {
                var profesores = GetProfesores();
                var materias = await _materiaEndpoint.GetAll(); // May throw an exception, so that is why the modal is not showing in nico user

                viewModel.SetProfesoresAsSelectList(profesores);
                viewModel.SetMateriasAsSelectList(materias);

                ModelState.AddModelErrorsFromResponse(response);

                return PartialView("_Create", viewModel);
            }

            return Content("OK");
        }

        // Edit - GET HorarioConsulta/ID
        [HttpGet]
        public async Task<ActionResult> Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var response = GlobalApi.WebApiClient.GetAsync("horariosConsulta/" + id).Result;

            if (!response.IsSuccessStatusCode) {
                return HttpNotFound();
            }

            MvcHorarioConsultaModel horarioConsulta = response.Content.ReadAsAsync<MvcHorarioConsultaModel>().Result;

            var profesores = GetProfesores();
            var materias = await _materiaEndpoint.GetAll(); // May throw an exception, so that is why the modal is not showing in nico user

            var viewModel = new EditHorarioConsultaViewModel(profesores, materias, horarioConsulta);

            return PartialView("_Edit", viewModel);

        }

        // Edit - PUT HorarioConsulta/ID (Secured)
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind(Include = "...") is used to avoid overposting attacks
        public async Task<ActionResult> Edit(EditHorarioConsultaViewModel viewModel) {
            var response = GlobalApi.WebApiClient.PutAsJsonAsync("horariosConsulta/" + viewModel.HorarioConsulta.Id, viewModel.HorarioConsulta).Result;

            if (!response.IsSuccessStatusCode) {
                var profesores = GetProfesores();
                var materias = await _materiaEndpoint.GetAll(); // May throw an exception, so that is why the modal is not showing in nico user
                viewModel.SetProfesoresAsSelectList(profesores);
                viewModel.SetMateriasAsSelectList(materias);

                ModelState.AddModelErrorsFromResponse(response);

                return PartialView("_Edit", viewModel);
            }

            return Content("OK");

        }

        // List of Profesores - GET Profesores
        // NOTA: Testear si funciona el LinQ para filtrar solo Profesores
        public IEnumerable<MvcUsuarioModel> GetProfesores() {
            var response = GlobalApi.WebApiClient.GetAsync("usuarios").Result;

            return response.Content.ReadAsAsync<IEnumerable<MvcUsuarioModel>>().Result;
        }

        // List of Materias - GET Materias
        public IEnumerable<MvcMateriaModel> GetMaterias() {
            var response = GlobalApi.WebApiClient.GetAsync("materias").Result;

            return response.Content.ReadAsAsync<IEnumerable<MvcMateriaModel>>().Result;
        }
    }
}
