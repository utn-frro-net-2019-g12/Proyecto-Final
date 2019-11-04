using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Presentation.Web.MVC.Models;
using Presentation.Web.MVC.ViewModels;
using Presentation.Library.Models;
using Presentation.Library.Api.Endpoints.Interfaces;
using Presentation.Library.Api.Exceptions;
using System.Threading.Tasks;
using AutoMapper;
using Presentation.Web.MVC.Filters;

namespace Presentation.Web.MVC.Controllers {

    // Note: This Controller Communicates with ViewModels (CreateMateriaViewModel and EditMateriaViewModel)
    [AuthorizeSelected(Roles = "Admin")]
    public class MateriaController : Controller {

        private readonly IMateriaEndpoint _materiaEndpoint;
        private readonly IDepartamentoEndpoint _departamentoEndpoint;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public MateriaController(IMateriaEndpoint materiaEndpoint, IDepartamentoEndpoint departamentoEndpoint
            , IUserSession userSession, IMapper mapper)
        {
            _materiaEndpoint = materiaEndpoint;
            _departamentoEndpoint = departamentoEndpoint;
            _userSession = userSession;
            _mapper = mapper;
        }

        // Index - GET Materia
        public async Task<ActionResult> Index() {
            try
            {
                IEnumerable<Materia> entities = await _materiaEndpoint.GetAll(_userSession.BearerToken);

                var materias = _mapper.Map<IEnumerable<MvcMateriaModel>>(entities);

                var viewModel = new ShowMateriasViewModel(materias: materias);

                return View(viewModel);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }
        }

        // Search - GET Materia/PartialDescripcion
        public async Task<ActionResult> Search(string partialDesc) {
            try {
                IEnumerable<Materia> entities = await _materiaEndpoint.GetByPartialDesc(partialDesc, _userSession.BearerToken);

                var materias = _mapper.Map<IEnumerable<MvcMateriaModel>>(entities);

                var viewModel = new ShowMateriasViewModel(materias: materias, parcialDesc: partialDesc);

                return View("Index", viewModel);
            } catch (UnauthorizedRequestException) {
                return RedirectToAction("AccessDenied", "Error");
            } catch (Exception ex) {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }
        }

        // Delete - DELETE Materia/ID
        public async Task<ActionResult> Delete(int id) {
            try
            {
                await _materiaEndpoint.Delete(id, _userSession.BearerToken);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDeniedPartial", "Error");
            }
            catch (NotFoundRequestException ex)
            {
                return Content($"{ex.StatusCode}: Elemento no encontrado");
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificErrorPartial", "Error", new { error = ex.Message });
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
                IEnumerable<Departamento> entities = await _departamentoEndpoint.GetAll(_userSession.BearerToken);

                var departamentos = _mapper.Map<IEnumerable<MvcDepartamentoModel>>(entities);

                var viewModel = new CreateMateriaViewModel(departamentos);

                return PartialView("_Create", viewModel);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDeniedPartial", "Error");
            }      
            catch (Exception ex)
            {
                return RedirectToAction("SpecificErrorPartial", "Error", new { error = ex.Message });
            }
        }

        // Create - Post Materia
        [HttpPost]
        public async Task<ActionResult> Create(CreateMateriaViewModel viewModel) {
            try
            {
                var entity = _mapper.Map<Materia>(viewModel.Materia);

                await _materiaEndpoint.Post(entity, _userSession.BearerToken);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDeniedPartial", "Error");
            }
            catch (BadRequestException ex)
            {
                    IEnumerable<Departamento> entities = await _departamentoEndpoint.GetAll(_userSession.BearerToken);

                    var departamentos = _mapper.Map< IEnumerable<MvcDepartamentoModel>>(entities);

                    viewModel.SetDepartamentosAsSelectList(departamentos);

                    ModelState.AddModelErrors(ex.Errors);

                    return PartialView("_Create", viewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificErrorPartial", "Error", new { error = ex.Message });
            }

            return Content("OK");
        }

        // Edit - GET Materia/ID
        [HttpGet]
        public async Task<ActionResult> Edit(int? id) {
            if (id == null) {
                return Content("Debe incluir el id");
            }

            try
            {
                var materiaTask = _materiaEndpoint.Get(id, _userSession.BearerToken);
                var departamentosTask = _departamentoEndpoint.GetAll(_userSession.BearerToken);

                await Task.WhenAll(materiaTask, departamentosTask);

                var materia = _mapper.Map<MvcMateriaModel>(materiaTask.Result);
                var departamentos = _mapper.Map<IEnumerable<MvcDepartamentoModel>>(departamentosTask.Result);

                var viewModel = new EditMateriaViewModel(materia: materia, departamentos: departamentos);

                return PartialView("_Edit", viewModel);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDeniedPartial", "Error");
            }
            catch (NotFoundRequestException ex)
            {
                return Content($"{ex.StatusCode}: Elemento no encontrado");
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificErrorPartial", "Error", new { error = ex.Message });
            }
        }

        // Edit - PUT Materia/ID (Secured)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditMateriaViewModel viewModel) {
            try
            {
                var entity = _mapper.Map<Materia>(viewModel.Materia);

                await _materiaEndpoint.Put(entity, _userSession.BearerToken);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDeniedPartial", "Error");
            }
            catch (BadRequestException ex)
            {
                IEnumerable<Departamento> entities = await _departamentoEndpoint.GetAll(_userSession.BearerToken);

                var departamentos = _mapper.Map< IEnumerable<MvcDepartamentoModel>>(entities);
                viewModel.SetDepartamentosAsSelectList(departamentos);

                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_Edit", viewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificErrorPartial", "Error", new { error = ex.Message });
            }

            return Content("OK");
        }
    }
}
