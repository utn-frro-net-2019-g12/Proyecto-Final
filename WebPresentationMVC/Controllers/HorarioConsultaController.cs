using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebPresentationMVC.Api.Endpoints.Interfaces;
using WebPresentationMVC.Api.Exceptions;
using WebPresentationMVC.Models;
using WebPresentationMVC.ViewModels;

namespace WebPresentationMVC.Controllers {

    // Note: This Controller Communicates with ViewModels (CreateHorarioConsultaViewModel and EditHorarioConsultaViewModel)
    [Authorize]
    public class HorarioConsultaController : Controller {
        private IMateriaEndpoint _materiaEndpoint;
        private IUsuarioEndpoint _usuarioEndpoint;
        private IHorarioConsultaEndpoint _horarioConsultaEndpoint;

        public HorarioConsultaController(IMateriaEndpoint materiaEndpoint, IUsuarioEndpoint usuarioEndpoint, IHorarioConsultaEndpoint horarioConsultaEndpoint)
        {
            _materiaEndpoint = materiaEndpoint;
            _usuarioEndpoint = usuarioEndpoint;
            _horarioConsultaEndpoint = horarioConsultaEndpoint;

        }

        // Index - GET HorarioConsulta
        public async Task<ActionResult> Index()
        {
            try
            {
                IEnumerable<MvcHorarioConsultaModel> entities = await _horarioConsultaEndpoint.GetAll();

                return View(entities);
            }
            catch (UnauthorizedRequestException)
            {
                return Content("No tiene acceso");
            }
            catch (Exception ex)
            {
                return Content($"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte");
            }
        }

        // Details - GET HorarioConsulta/ID
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                MvcHorarioConsultaModel entity = await _horarioConsultaEndpoint.Get(id);

                return View(entity);
            }
            catch (UnauthorizedRequestException)
            {
                return Content("No tiene acceso");
            }
            catch (NotFoundRequestException ex)
            {
                return Content($"{ex.StatusCode}: Elemento no encontrado");
            }
            catch (Exception ex)
            {
                return Content($"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte");
            }
        }

        // Delete - DELETE HorarioConsulta/ID
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _horarioConsultaEndpoint.Delete(id);
            }
            catch (UnauthorizedRequestException)
            {
                return Content("No tiene acceso");
            }
            catch (NotFoundRequestException ex)
            {
                return Content($"{ex.StatusCode}: Elemento no encontrado");
            }
            catch (Exception ex)
            {
                return Content($"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte");
            }

            // TempData may be used to check in the view whether the deletion was successful or not
            TempData["SuccessMessage"] = "Deleted Sucessfully";

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // Create (Default)
        [HttpGet]
        public async Task<ActionResult> Create() {
            try
            {
                // These tasks run in parallel until they are awaited by Task.WhenAll method
                var profesoresTask = _usuarioEndpoint.GetAll();
                var materiasTask = _materiaEndpoint.GetAll();

                await Task.WhenAll(profesoresTask, materiasTask);

                var viewModel = new CreateHorarioConsultaViewModel(profesoresTask.Result, materiasTask.Result);

                return PartialView("_Create", viewModel);
            }
            catch (UnauthorizedRequestException)
            {
                return Content("No tiene acceso");
            }
            catch (Exception ex)
            {
                return Content($"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte");
            }
        }

        // Create - Post HorarioConsulta
        [HttpPost]
        public async Task<ActionResult> Create(CreateHorarioConsultaViewModel viewModel) {
            try
            {
                await _horarioConsultaEndpoint.Post(viewModel.HorarioConsulta);
            }
            catch (UnauthorizedRequestException)
            {
                return Content("No tiene acceso");
            }
            catch (BadRequestException ex)
            {
                var profesoresTask = _usuarioEndpoint.GetAll();
                var materiasTask = _materiaEndpoint.GetAll(); 

                await Task.WhenAll(profesoresTask, materiasTask);

                viewModel.SetProfesoresAsSelectList(profesoresTask.Result);
                viewModel.SetMateriasAsSelectList(materiasTask.Result);

                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_Create", viewModel);
            }
            catch (Exception ex)
            {
                return Content($"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte");
            }

            return Content("OK");
        }

        // Edit - GET HorarioConsulta/ID
        [HttpGet]
        public async Task<ActionResult> Edit(int? id) {
            if (id == null)
            {
                return Content("Debe incluir el id");
            }

            try
            {
                var horarioConsultaTask = _horarioConsultaEndpoint.Get(id);
                var profesoresTask = _usuarioEndpoint.GetAll();
                var materiasTask = _materiaEndpoint.GetAll();

                await Task.WhenAll(horarioConsultaTask, profesoresTask, materiasTask);

                var viewModel = new EditHorarioConsultaViewModel(profesoresTask.Result, materiasTask.Result, horarioConsultaTask.Result);

                return PartialView("_Edit", viewModel);
            }
            catch (UnauthorizedRequestException)
            {
                return Content("No tiene acceso");
            }
            catch (NotFoundRequestException ex)
            {
                return Content($"{ex.StatusCode}: Elemento no encontrado");
            }
            catch (Exception ex)
            {
                return Content($"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte");
            }
        }

        // Edit - PUT HorarioConsulta/ID (Secured)
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind(Include = "...") is used to avoid overposting attacks
        public async Task<ActionResult> Edit(EditHorarioConsultaViewModel viewModel) {
            try
            {
                await _horarioConsultaEndpoint.Put(viewModel.HorarioConsulta);
            }
            catch (UnauthorizedRequestException)
            {
                return Content("No tiene acceso");
            }
            catch (BadRequestException ex)
            {
                var profesoresTask = _usuarioEndpoint.GetAll();
                var materiasTask = _materiaEndpoint.GetAll(); // May throw an exception, so that is why the modal is not showing in nico user

                await Task.WhenAll(profesoresTask, materiasTask);

                viewModel.SetProfesoresAsSelectList(profesoresTask.Result);
                viewModel.SetMateriasAsSelectList(materiasTask.Result);

                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_Edit", viewModel);
            }
            catch (Exception ex)
            {
                return Content($"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte");
            }

            return Content("OK");
        }
    }
}
