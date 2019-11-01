using AutoMapper;
using Presentation.Library.Api.Endpoints.Interfaces;
using Presentation.Library.Api.Exceptions;
using Presentation.Library.Models;
using Presentation.Web.MVC.Models;
using Presentation.Web.MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Presentation.Web.MVC.Controllers
{
    public class ProfesorController : Controller
    {
        private readonly IHorarioConsultaEndpoint _horarioConsultaEndpoint;
        private readonly IHorarioConsultaFechadoEndpoint _horarioConsultaFechadoEndpoint;
        private readonly IInscripcionEndpoint _inscripcionEndpoint;
        private readonly IMateriaEndpoint _materiaEndpoint;
        private readonly IUsuarioEndpoint _usuarioEndpoint;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public ProfesorController(IHorarioConsultaEndpoint horarioConsultaEndpoint, IHorarioConsultaFechadoEndpoint horarioConsultaFechadoEndpoint,
            IInscripcionEndpoint inscripcionEndpoint, IMateriaEndpoint materiaEndpoint, IUsuarioEndpoint usuarioEndpoint, IUserSession userSession,
            IMapper mapper)
        {
            _horarioConsultaEndpoint = horarioConsultaEndpoint;
            _horarioConsultaFechadoEndpoint = horarioConsultaFechadoEndpoint;
            _inscripcionEndpoint = inscripcionEndpoint;
            _materiaEndpoint = materiaEndpoint;
            _usuarioEndpoint = usuarioEndpoint;
            _userSession = userSession;
            _mapper = mapper;
        }

        // GET: Profesor
        public ActionResult Index()
        {
            return RedirectToAction("InscripcionesRecibidas");
        }

        // Index Mis Horarios - GET HorariosConsulta (By Current User Profesor)
        public async Task<ActionResult> MisHorariosConsulta()
        {
            try
            {
                IEnumerable<HorarioConsulta> entities = await _horarioConsultaEndpoint.GetByCurrentUserProfessor(_userSession.BearerToken);

                var horariosConsulta = _mapper.Map<IEnumerable<MvcHorarioConsultaModel>>(entities);

                return View(horariosConsulta);
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

        // Index Inscripciones - GET Inscripciones (By Current Profesor User)
        public async Task<ActionResult> InscripcionesRecibidas()
        {
            try
            {
                IEnumerable<Inscripcion> entities = await _inscripcionEndpoint.GetByCurrentProfesorUser(_userSession.BearerToken);

                var inscripciones = _mapper.Map<IEnumerable<MvcInscripcionModel>>(entities);

                return View(inscripciones);
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

        // Search --> Mis HC & Insc Recibidas

        // Delete - DELETE Profesor/DeleteHorario/ID
        public async Task<ActionResult> DeleteHorario(int id) {
            try {
                await _horarioConsultaEndpoint.Delete(id, _userSession.BearerToken);
            } catch (UnauthorizedRequestException) {
                return RedirectToAction("AccessDenied", "Error");
            } catch (NotFoundRequestException ex) {
                return Content($"{ex.StatusCode}: Elemento no encontrado");
            } catch (Exception ex) {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }

            // TempData may be used to check in the view whether the deletion was successful or not
            TempData["SuccessMessage"] = "Deleted Sucessfully";

            return Content("OK");
        }

        // Create HorarioConsulta (Default)
        [HttpGet]
        public async Task<ActionResult> CreateHorario()
        {
            try
            {
                IEnumerable<Materia> entities = await _materiaEndpoint.GetAll(_userSession.BearerToken);

                var materias = _mapper.Map<IEnumerable<MvcMateriaModel>>(source: entities);
                var viewModel = new CreateOwnHorarioConsultaViewModel(materias: materias);

                return PartialView("_CreateOwnHorario", viewModel);
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

        // Create - Post Profesor/CreateHorario
        [HttpPost]
        public async Task<ActionResult> CreateHorario(CreateOwnHorarioConsultaViewModel viewModel)
        {
            try
            {
                Usuario user = await _usuarioEndpoint.GetCurrentUsuario(token: _userSession.BearerToken);

                viewModel.SetProfesor(user.Id);

                var horarioConsulta = _mapper.Map<HorarioConsulta>(source: viewModel.HorarioConsulta);

                await _horarioConsultaEndpoint.Post(horarioConsulta, _userSession.BearerToken);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            catch (BadRequestException ex)
            {
                IEnumerable<Materia> entities = await _materiaEndpoint.GetAll(_userSession.BearerToken);;

                var materias = _mapper.Map<IEnumerable<MvcMateriaModel>>(source: entities);
                viewModel.SetMateriasAsSelectList(materias);
                viewModel.SetDiasSemanaAsSelectList();

                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_CreateOwnHorario", viewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }

            return Content("OK");
        }

        // Edit - GET Profesor/EditHorario/ID
        [HttpGet]
        public async Task<ActionResult> EditHorario(int? id)
        {
            if (id == null)
            {
                return Content("Debe incluir el id");
            }

            try
            {
                var horarioConsultaTask = _horarioConsultaEndpoint.Get(id, _userSession.BearerToken);
                var materiasTask = _materiaEndpoint.GetAll(_userSession.BearerToken);

                await Task.WhenAll(horarioConsultaTask, materiasTask);

                var horarioConsulta = _mapper.Map<MvcHorarioConsultaModel>(source: horarioConsultaTask.Result);
                var materias = _mapper.Map<IEnumerable<MvcMateriaModel>>(source: materiasTask.Result);

                var viewModel = new EditOwnHorarioConsultaViewModel(horarioConsulta: horarioConsulta, materias: materias);

                return PartialView("_EditOwnHorario", viewModel);
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

        // Edit - PUT Profesor/EditHorario/ID (Secured)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditHorario(EditOwnHorarioConsultaViewModel viewModel)
        {
            try
            {
                Usuario user = await _usuarioEndpoint.GetCurrentUsuario(_userSession.BearerToken);

                viewModel.SetProfesor(user.Id);

                var entity = _mapper.Map<HorarioConsulta>(viewModel.HorarioConsulta);

                await _horarioConsultaEndpoint.Put(entity, _userSession.BearerToken);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            catch (BadRequestException ex)
            {
                var profesoresTask = _usuarioEndpoint.GetAll(_userSession.BearerToken);
                var materiasTask = _materiaEndpoint.GetAll(_userSession.BearerToken); // May throw an exception, so that is why the modal is not showing in nico user

                await Task.WhenAll(profesoresTask, materiasTask);

                var profesores = _mapper.Map<IEnumerable<MvcUsuarioModel>>(source: profesoresTask.Result);
                var materias = _mapper.Map<IEnumerable<MvcMateriaModel>>(source: materiasTask.Result);

                viewModel.SetMateriasAsSelectList(materias);
                viewModel.SetDiasSemanaAsSelectList();

                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_EditOwnHorario", viewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }

            return Content("OK");
        }

        // Edit - GET Profesor/EditInscripcion/ID
        [HttpGet]
        public async Task<ActionResult> EditInscripcion(int? id) {
            {
                if (id == null) {
                    return Content("Debe incluir el id");
                }

                try {
                    var inscripcionTask = _inscripcionEndpoint.Get(id, _userSession.BearerToken);
                    var alumnosTask = _usuarioEndpoint.GetAll(_userSession.BearerToken);
                    var horariosConsultaFechadosTask = _horarioConsultaFechadoEndpoint.GetAll(_userSession.BearerToken);

                    await Task.WhenAll(inscripcionTask, alumnosTask, horariosConsultaFechadosTask);

                    var inscripcion = _mapper.Map<MvcInscripcionModel>(source: inscripcionTask.Result);
                    var alumnos = _mapper.Map<IEnumerable<MvcUsuarioModel>>(source: alumnosTask.Result);
                    var horariosConsultaFechados = _mapper.Map<IEnumerable<MvcHorarioConsultaFechadoModel>>(source: horariosConsultaFechadosTask.Result);

                    var viewModel = new EditInscripcionRecibidaViewModel(inscripcion: inscripcion, horariosConsultaFechados: horariosConsultaFechados, alumnos: alumnos);

                    return PartialView("_Edit", viewModel);
                } catch (UnauthorizedRequestException) {
                    return RedirectToAction("AccessDenied", "Error");
                } catch (NotFoundRequestException ex) {
                    return Content($"{ex.StatusCode}: Elemento no encontrado");
                } catch (Exception ex) {
                    return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
                }
            }
        }

        // Edit - PUT Profesor/EditInscripcion/ID (Secured)
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind(Include = "...") is used to avoid overposting attacks
        public async Task<ActionResult> EditInscripcion(EditInscripcionRecibidaViewModel viewModel) {
            try {
                var entity = _mapper.Map<Inscripcion>(viewModel.Inscripcion);

                if (entity.State == null) { entity.State = Inscripcion.InscripcionStates.Active; }

                await _inscripcionEndpoint.Put(entity, _userSession.BearerToken);
            } catch (UnauthorizedRequestException) {
                return RedirectToAction("AccessDenied", "Error");
            } catch (BadRequestException ex) {
                var profesoresTask = _usuarioEndpoint.GetAll(_userSession.BearerToken);
                var horariosConsultaFechadosTask = _horarioConsultaFechadoEndpoint.GetAll(_userSession.BearerToken); // May throw an exception, so that is why the modal is not showing in nico user

                await Task.WhenAll(profesoresTask, horariosConsultaFechadosTask);

                var alumnos = _mapper.Map<IEnumerable<MvcUsuarioModel>>(source: profesoresTask.Result);
                var horariosConsultaFechado = _mapper.Map<IEnumerable<MvcHorarioConsultaFechadoModel>>(source: horariosConsultaFechadosTask.Result);

                viewModel.SetAlumnosAsSelectList(alumnos);
                viewModel.SetHorariosConsultaFechadosAsSelectList(horariosConsultaFechado);
                viewModel.SetEstadosAsSelectList();

                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_Edit", viewModel);
            } catch (Exception ex) {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }

            return Content("OK");
        }
    }
}
