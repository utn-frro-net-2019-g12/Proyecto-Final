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

namespace Presentation.Web.MVC.Controllers {

    // Note: This Controller Communicates with ViewModels (CreateHorarioConsultaFechadoViewModel and EditHorarioConsultaFechadoViewModel)
    [Authorize]
    public class HorarioConsultaFechadoController : Controller {
        private readonly IHorarioConsultaEndpoint _horarioConsultaEndpoint;
        private readonly IHorarioConsultaFechadoEndpoint _horarioConsultaFechadoEndpoint;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;


        public HorarioConsultaFechadoController(IHorarioConsultaEndpoint horarioConsultaEndpoint
            , IHorarioConsultaFechadoEndpoint horarioConsultaFechadoEndpoint, IUserSession userSession, IMapper mapper)
        {
            _horarioConsultaEndpoint = horarioConsultaEndpoint;
            _horarioConsultaFechadoEndpoint = horarioConsultaFechadoEndpoint;
            _userSession = userSession;
            _mapper = mapper;
        }

        // Index - GET HorarioConsultaFechado
        public async Task<ActionResult> Index()
        {
            try
            {
                IEnumerable<HorarioConsultaFechado> entities = await _horarioConsultaFechadoEndpoint.GetAll(_userSession.BearerToken);

                var horariosConsultaFechados = _mapper.Map<IEnumerable<MvcHorarioConsultaFechadoModel>>(entities);

                return View(horariosConsultaFechados);
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

        // Details - GET HorarioConsultaFechado/ID
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                HorarioConsultaFechado entity = await _horarioConsultaFechadoEndpoint.Get(id, _userSession.BearerToken);

                var horarioConsultaFechado = _mapper.Map<MvcHorarioConsultaFechadoModel>(entity);

                return View(horarioConsultaFechado);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            catch (NotFoundRequestException ex)
            {
                return Content($"{ex.StatusCode}: Elemento no encontrado");
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }
        }

        // Delete - DELETE HorarioConsultaFechado/ID
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _horarioConsultaFechadoEndpoint.Delete(id, _userSession.BearerToken);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            catch (NotFoundRequestException ex)
            {
                return Content($"{ex.StatusCode}: Elemento no encontrado");
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
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
                var horariosConsultaTask = _horarioConsultaEndpoint.GetAll(_userSession.BearerToken);

                await Task.WhenAll(horariosConsultaTask);

                var horariosConsulta = _mapper.Map<IEnumerable<MvcHorarioConsultaModel>>(source: horariosConsultaTask.Result);

                var viewModel = new CreateHorarioConsultaFechadoViewModel(horariosConsulta: horariosConsulta);

                return PartialView("_Create", viewModel);
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

        // Create - Post HorarioConsultaFechado
        [HttpPost]
        public async Task<ActionResult> Create(CreateHorarioConsultaFechadoViewModel viewModel) {
            try
            {
                var entity = _mapper.Map<HorarioConsultaFechado>(source: viewModel.HorarioConsultaFechado);

                await _horarioConsultaFechadoEndpoint.Post(entity, _userSession.BearerToken);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            catch (BadRequestException ex)
            {
                var horariosConsultaTask = _horarioConsultaEndpoint.GetAll(_userSession.BearerToken);

                await Task.WhenAll(horariosConsultaTask);

                var horariosConsulta = _mapper.Map<IEnumerable<MvcHorarioConsultaModel>>(source: horariosConsultaTask.Result);

                viewModel.SetHorariosConsultaAsSelectList(horariosConsulta);
                viewModel.SetEstadosAsSelectList();

                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_Create", viewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }

            return Content("OK");
        }

        // Edit - GET HorarioConsultaFechado/ID
        [HttpGet]
        public async Task<ActionResult> Edit(int? id) {
            if (id == null)
            {
                return Content("Debe incluir el id");
            }

            try
            {
                var horarioConsultaTask = _horarioConsultaEndpoint.Get(id, _userSession.BearerToken);
                var horarioConsultaFechadoTask = _horarioConsultaFechadoEndpoint.Get(id, _userSession.BearerToken);

                await Task.WhenAll(horarioConsultaTask, horarioConsultaFechadoTask);

                var horariosConsulta = _mapper.Map<IEnumerable<MvcHorarioConsultaModel>>(source: horarioConsultaTask.Result);
                var horariosConsultaFechado = _mapper.Map<MvcHorarioConsultaFechadoModel>(source: horarioConsultaFechadoTask.Result);

                var viewModel = new EditHorarioConsultaFechadoViewModel(horarioConsultaFechado: horariosConsultaFechado, horariosConsulta: horariosConsulta);

                return PartialView("_Edit", viewModel);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            catch (NotFoundRequestException ex)
            {
                return Content($"{ex.StatusCode}: Elemento no encontrado");
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }
        }

        // Edit - PUT HorarioConsultaFechado/ID (Secured)
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind(Include = "...") is used to avoid overposting attacks
        public async Task<ActionResult> Edit(EditHorarioConsultaFechadoViewModel viewModel) {
            try
            {
                var entity = _mapper.Map<HorarioConsultaFechado>(viewModel.HorarioConsultaFechado);

                await _horarioConsultaFechadoEndpoint.Put(entity, _userSession.BearerToken);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            catch (BadRequestException ex)
            {
                var horariosConsultaTask = _horarioConsultaEndpoint.GetAll(_userSession.BearerToken);

                await Task.WhenAll(horariosConsultaTask);

                var horariosConsulta = _mapper.Map<IEnumerable<MvcHorarioConsultaModel>>(source: horariosConsultaTask.Result);

                viewModel.SetHorariosConsultaAsSelectList(horariosConsulta);
                viewModel.SetEstadosAsSelectList();

                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_Edit", viewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }

            return Content("OK");
        }
    }
}
