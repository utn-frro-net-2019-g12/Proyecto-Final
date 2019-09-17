using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebPresentationMVC.Models;
using WebPresentationMVC.ViewModels;
using WebPresentationMVC.Api;
using System.Threading.Tasks;

namespace WebPresentationMVC.Controllers {

    // Note: This Controller Communicates with ViewModels (CreateMateriaViewModel and EditMateriaViewModel)
    [Authorize]
    public class MateriaController : Controller {

        private IMateriaEndpoint _materiaEndpoint;
        private IDepartamentoEndpoint _departamentoEndpoint;

        public MateriaController(IMateriaEndpoint materiaEndpoint, IDepartamentoEndpoint departamentoEndpoint)
        {
            _materiaEndpoint = materiaEndpoint;
            _departamentoEndpoint = departamentoEndpoint;
        }

        // Index - GET Materia
        public async Task<ActionResult> Index() {
            try
            {
                var materias = await _materiaEndpoint.GetAll();

                return View(materias);
            }
            catch(ApiErrorsException ex)
            {
                if(ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Content("No tiene acceso");
                }
                else
                {
                    return Content($"{ex.StatusCode.ToString()} Ha ocurrido un error. Por favor contacte a soporte");
                }
            }
        }

        // Details - GET Materia/ID
        public async Task<ActionResult> Details(int id) {
            try
            {
                var materia = await _materiaEndpoint.Get(id);

                return View(materia);
            }
            catch (ApiErrorsException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Content("No tiene acceso");
                }
                else
                {
                    return Content($"{ex.StatusCode.ToString()} Ha ocurrido un error. Por favor contacte a soporte");
                }
            }
        }

        // Delete - DELETE Materia/ID
        public async Task<ActionResult> Delete(int id) {
            try
            {
                await _materiaEndpoint.Delete(id);
            }
            catch (ApiErrorsException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Content("No tiene acceso");
                }
                else
                {
                    return Content($"{ex.StatusCode.ToString()} Ha ocurrido un error. Por favor contacte a soporte");
                }
            }

            TempData["SuccessMessage"] = "Deleted Sucessfully";

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // Create (Default)
        [HttpGet]
        public async Task<ActionResult> Create() {
            var departamentos = await _departamentoEndpoint.GetAll();

            var viewModel = new CreateMateriaViewModel(departamentos);

            return PartialView("_Create", viewModel);
        }

        // Create - Post Materia
        [HttpPost]
        public async Task<ActionResult> Create(CreateMateriaViewModel viewModel) {
            try
            {
                await _materiaEndpoint.Post(viewModel.Materia);
            }
            catch (ApiErrorsException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Content("No tiene acceso");
                }
                else if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    var departamentos = await _departamentoEndpoint.GetAll();
                    viewModel.SetDepartamentosAsSelectList(departamentos);

                    ModelState.AddModelErrors(ex.Errors);

                    return PartialView("_Create", viewModel);
                }
                else
                {
                    return Content($"{ex.StatusCode.ToString()} Ha ocurrido un error. Por favor contacte a soporte");
                }
            }

            return Content("OK");
        }

        // Edit - GET Materia/ID
        [HttpGet]
        public async Task<ActionResult> Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var materia = await _materiaEndpoint.Get(id);

                var departamentos = await _departamentoEndpoint.GetAll();

                var viewModel = new EditMateriaViewModel(departamentos, materia);

                return PartialView("_Edit", viewModel);
            }
            catch (ApiErrorsException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Content("No tiene acceso");
                }
                else
                {
                    return Content($"{ex.StatusCode.ToString()} Ha ocurrido un error. Por favor contacte a soporte");
                }
            }
        }

        // Edit - PUT Materia/ID (Secured)
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind(Include = "...") is used to avoid overposting attacks
        public async Task<ActionResult> Edit(EditMateriaViewModel viewModel) {
            try
            {
                await _materiaEndpoint.Put(viewModel.Materia);
            }
            catch (ApiErrorsException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Content("No tiene acceso");
                }
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    var departamentos = await _departamentoEndpoint.GetAll();
                    viewModel.SetDepartamentosAsSelectList(departamentos);

                    ModelState.AddModelErrors(ex.Errors);

                    return PartialView("_Edit", viewModel);
                }
                else
                {
                    return Content($"{ex.StatusCode.ToString()} Ha ocurrido un error. Por favor contacte a soporte");
                }
            }

            return Content("OK");
        }
    }
}
