using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Presentation.Library.Models;
using Presentation.Library.Api.Endpoints.Interfaces;
using Presentation.Library.Api.Exceptions;
using Presentation.Web.MVC.Models;
using Presentation.Web.MVC.ViewModels;
using AutoMapper;
using Rotativa;
using System.IO;

namespace Presentation.Web.MVC.Controllers {

    // Note: This Controller Communicates with ViewModels (CreateHorarioConsultaViewModel and EditHorarioConsultaViewModel)
    public class HorarioConsultaController : Controller {
        private readonly IMateriaEndpoint _materiaEndpoint;
        private readonly IUsuarioEndpoint _usuarioEndpoint;
        private readonly IHorarioConsultaEndpoint _horarioConsultaEndpoint;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;


        public HorarioConsultaController(IMateriaEndpoint materiaEndpoint, IUsuarioEndpoint usuarioEndpoint
            , IHorarioConsultaEndpoint horarioConsultaEndpoint, IUserSession userSession, IMapper mapper)
        {
            _materiaEndpoint = materiaEndpoint;
            _usuarioEndpoint = usuarioEndpoint;
            _horarioConsultaEndpoint = horarioConsultaEndpoint;
            _userSession = userSession;
            _mapper = mapper;
        }

        // Index - GET HorarioConsulta
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            try
            {
                IEnumerable<HorarioConsulta> entities = await _horarioConsultaEndpoint.GetAll(_userSession.BearerToken);

                var horariosConsulta = _mapper.Map<IEnumerable<MvcHorarioConsultaModel>>(entities);

                var viewModel = new ShowHorariosConsultaViewModel()
                {
                    HorariosConsulta = horariosConsulta,
                    DescParcial = null,
                    DepartamentoId = null
                };

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

        [HttpGet]
        public async Task<ActionResult> Search(string desc, int? deptoId) {
            try {
                IEnumerable<HorarioConsulta> entities = await _horarioConsultaEndpoint.GetByPartialDescAndDepto(partialDesc:desc, deptoId:deptoId, token:_userSession.BearerToken);

                var horariosConsulta = _mapper.Map<IEnumerable<MvcHorarioConsultaModel>>(entities);

                var viewModel = new ShowHorariosConsultaViewModel()
                {
                    HorariosConsulta = horariosConsulta,
                    DescParcial = desc,
                    DepartamentoId = deptoId
                };

                return View("Index", viewModel);
            } catch (UnauthorizedRequestException) {
                return RedirectToAction("AccessDenied", "Error");
            } catch (Exception ex) {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }
        }

        // Delete - DELETE HorarioConsulta/ID
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _horarioConsultaEndpoint.Delete(id, _userSession.BearerToken);
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

        // Logic Delete - POST HorarioConsulta/ID
        public async Task<ActionResult> LogicDelete(int? id) {
            if (id == null) {
                return Content("Debe incluir el id");
            }
            
            try {
                await _horarioConsultaEndpoint.Get(id, _userSession.BearerToken);

                var horarioConsultaTask = _horarioConsultaEndpoint.Get(id, _userSession.BearerToken);
                await Task.WhenAll(horarioConsultaTask);

                var horarioConsulta = _mapper.Map<MvcHorarioConsultaModel>(source: horarioConsultaTask.Result);

                if (horarioConsulta.EliminationDate == null) {
                    horarioConsulta.EliminationDate = DateTime.Today.Date;
                } else {
                    return Content("Este elemento ya estaba eliminado lógicamente");
                }

                var entity = _mapper.Map<HorarioConsulta>(source: horarioConsulta);
                await _horarioConsultaEndpoint.Put(entity, _userSession.BearerToken);
            } catch (UnauthorizedRequestException) {
                return RedirectToAction("AccessDenied", "Error");
            } catch (NotFoundRequestException ex) {
                return Content($"{ex.StatusCode}: Elemento no encontrado");
            } catch (Exception ex) {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }

            // TempData may be used to check in the view whether the deletion was successful or not
            TempData["SuccessMessage"] = "Logic Deleted Sucessfully";
            // return Content("OK");
            return Content("OK");
        }

        // Create (Default)
        [HttpGet]
        public async Task<ActionResult> Create() {
            try
            {
                // These tasks run in parallel until they are awaited by Task.WhenAll method
                var profesoresTask = _usuarioEndpoint.GetAll(_userSession.BearerToken);
                var materiasTask = _materiaEndpoint.GetAll(_userSession.BearerToken);

                await Task.WhenAll(profesoresTask, materiasTask);

                var profesores = _mapper.Map<IEnumerable<MvcUsuarioModel>>(source: profesoresTask.Result);
                var materias = _mapper.Map<IEnumerable<MvcMateriaModel>>(source: materiasTask.Result);

                var viewModel = new CreateHorarioConsultaViewModel(materias: materias, profesores: profesores);

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

        // Create - Post HorarioConsulta
        [HttpPost]
        public async Task<ActionResult> Create(CreateHorarioConsultaViewModel viewModel) {
            try
            {
                var entity = _mapper.Map<HorarioConsulta>(source: viewModel.HorarioConsulta);

                await _horarioConsultaEndpoint.Post(entity, _userSession.BearerToken);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDeniedPartial", "Error");
            }
            catch (BadRequestException ex)
            {
                var profesoresTask = _usuarioEndpoint.GetAll(_userSession.BearerToken);
                var materiasTask = _materiaEndpoint.GetAll(_userSession.BearerToken); 

                await Task.WhenAll(profesoresTask, materiasTask);

                var profesores = _mapper.Map<IEnumerable<MvcUsuarioModel>>(source: profesoresTask.Result);
                var materias = _mapper.Map<IEnumerable<MvcMateriaModel>>(source: materiasTask.Result);

                viewModel.SetProfesoresAsSelectList(profesores);
                viewModel.SetMateriasAsSelectList(materias);
                viewModel.SetDiasSemanaAsSelectList();

                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_Create", viewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificErrorPartial", "Error", new { error = ex.Message });
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
                var horarioConsultaTask = _horarioConsultaEndpoint.Get(id, _userSession.BearerToken);
                var profesoresTask = _usuarioEndpoint.GetAll(_userSession.BearerToken);
                var materiasTask = _materiaEndpoint.GetAll(_userSession.BearerToken);

                await Task.WhenAll(horarioConsultaTask, profesoresTask, materiasTask);

                var horarioConsulta = _mapper.Map<MvcHorarioConsultaModel>(source: horarioConsultaTask.Result);
                var profesores = _mapper.Map<IEnumerable<MvcUsuarioModel>>(source: profesoresTask.Result);
                var materias = _mapper.Map<IEnumerable<MvcMateriaModel>>(source: materiasTask.Result);

                var viewModel = new EditHorarioConsultaViewModel(horarioConsulta: horarioConsulta, materias: materias, profesores: profesores);

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

        // Edit - PUT HorarioConsulta/ID (Secured)
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind(Include = "...") is used to avoid overposting attacks
        public async Task<ActionResult> Edit(EditHorarioConsultaViewModel viewModel) {
            try
            {
                var entity = _mapper.Map<HorarioConsulta>(viewModel.HorarioConsulta);

                await _horarioConsultaEndpoint.Put(entity, _userSession.BearerToken);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDeniedPartial", "Error");
            }
            catch (BadRequestException ex)
            {
                var profesoresTask = _usuarioEndpoint.GetAll(_userSession.BearerToken);
                var materiasTask = _materiaEndpoint.GetAll(_userSession.BearerToken); // May throw an exception, so that is why the modal is not showing in nico user

                await Task.WhenAll(profesoresTask, materiasTask);

                var profesores = _mapper.Map<IEnumerable<MvcUsuarioModel>>(source: profesoresTask.Result);
                var materias = _mapper.Map<IEnumerable<MvcMateriaModel>>(source: materiasTask.Result);

                viewModel.SetProfesoresAsSelectList(profesores);
                viewModel.SetMateriasAsSelectList(materias);
                viewModel.SetDiasSemanaAsSelectList();

                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_Edit", viewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificErrorPartial", "Error", new { error = ex.Message });
            }

            return Content("OK");
        }

        [HttpGet]
        public async Task<ActionResult> Report(string desc, int? deptoId)
        {
            try
            {
                IEnumerable<HorarioConsulta> entities = await _horarioConsultaEndpoint.GetByPartialDescAndDepto(partialDesc: desc, deptoId: deptoId, token: _userSession.BearerToken);

                var horarios = _mapper.Map<IEnumerable<MvcHorarioConsultaModel>>(entities);

                var report = new ViewAsPdf(viewName: "Report", model: horarios) { FileName = $"horarios_{DateTime.Now}.pdf" };

                report.FormsAuthenticationCookieName = System.Web.Security.FormsAuthentication.FormsCookieName;
                report.CustomSwitches = "--load-error-handling ignore";

                return report;
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
    }
}
