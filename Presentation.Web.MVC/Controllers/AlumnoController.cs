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
    [Authorize(Roles = "Alumno")]
    public class AlumnoController : Controller
    {
        private readonly IInscripcionEndpoint _inscripcionEndpoint;
        private readonly IUsuarioEndpoint _usuarioEndpoint;
        private readonly IHorarioConsultaFechadoEndpoint _horarioConsultaFechadoEndpoint;
        private readonly IDepartamentoEndpoint _departamentoEndpoint;
        private readonly IMateriaEndpoint _materiaEndpoint;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public AlumnoController(IInscripcionEndpoint inscripcionEndpoint, IHorarioConsultaFechadoEndpoint horarioConsultaFechadoEndpoint,
            IUsuarioEndpoint usuarioEndpoint, IDepartamentoEndpoint departamentoEndpoint, IMateriaEndpoint materiaEndpoint,
            IUserSession userSession, IMapper mapper) {
            _inscripcionEndpoint = inscripcionEndpoint;
            _horarioConsultaFechadoEndpoint = horarioConsultaFechadoEndpoint;
            _usuarioEndpoint = usuarioEndpoint;
            _departamentoEndpoint = departamentoEndpoint;
            _materiaEndpoint = materiaEndpoint;
            _userSession = userSession;
            _mapper = mapper;
        }

        // GET: Alumno
        public ActionResult Index()
        {
            return RedirectToAction("MisInscripciones");
        }

        // Index Nueva Inscripción (By Current Alumno User)
        public async Task<ActionResult> NuevaConsulta() {
            try
            {
                var departamentosTask = _departamentoEndpoint.GetAll(_userSession.BearerToken);
                var materiasTask = _materiaEndpoint.GetAll(_userSession.BearerToken);
                var profesoresTask = _usuarioEndpoint.GetAllProfesores(_userSession.BearerToken);

                await Task.WhenAll(departamentosTask, materiasTask, profesoresTask);

                var departamentos = _mapper.Map<IEnumerable<MvcDepartamentoModel>>(source: departamentosTask.Result);
                var materias = _mapper.Map<IEnumerable<MvcMateriaModel>>(source: materiasTask.Result);
                var profesores = _mapper.Map<IEnumerable<MvcUsuarioModel>>(source: profesoresTask.Result);

                var viewModel = new ShowHorariosParaInscribirViewModel(
                    departamentos: departamentos, materias: materias, profesores: profesores);

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

        // Index Mis Inscripciones - GET Inscripciones (By Current Alumno User)
        public async Task<ActionResult> MisInscripciones()
        {
            try
            {
                IEnumerable<Inscripcion> entities = await _inscripcionEndpoint.GetByCurrentAlumnoUser(_userSession.BearerToken);

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

        // Search Nueva Inscripción (By Depto, Materia y Profesor)
        public async Task<ActionResult> Search(int? deptoId, int? materiaId, int? profeId) {
            try {
                var horariosConsultaFechadosTask = _horarioConsultaFechadoEndpoint.GetByDeptoAndMateriaAndProfe(deptoId:deptoId, materiaId:materiaId, profeId:profeId ,token:_userSession.BearerToken);
                var departamentosTask = _departamentoEndpoint.GetAll(_userSession.BearerToken);
                var materiasTask = _materiaEndpoint.GetAll(_userSession.BearerToken);
                var profesoresTask = _usuarioEndpoint.GetAllProfesores(_userSession.BearerToken);

                await Task.WhenAll(horariosConsultaFechadosTask ,departamentosTask, materiasTask, profesoresTask);

                var departamentos = _mapper.Map<IEnumerable<MvcDepartamentoModel>>(source: departamentosTask.Result);
                var materias = _mapper.Map<IEnumerable<MvcMateriaModel>>(source: materiasTask.Result);
                var profesores = _mapper.Map<IEnumerable<MvcUsuarioModel>>(source: profesoresTask.Result);
                var horariosConsultaFechados = _mapper.Map<IEnumerable<MvcHorarioConsultaFechadoModel>>(horariosConsultaFechadosTask.Result);

                var viewModel = new ShowHorariosParaInscribirViewModel(
                    horariosConsultaFechados: horariosConsultaFechados,
                    departamentos: departamentos, materias: materias, profesores: profesores,
                    departamentoId: deptoId, materiaId: materiaId, profesorId:profeId);

                return View("NuevaConsulta", viewModel);
            } catch (UnauthorizedRequestException) {
                return RedirectToAction("AccessDenied", "Error");
            } catch (Exception ex) {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }
        }

        // Search --> Búsqueda Mis Inscripciones --> TO-DO

        // Unsubscribe - POST Alumno/Inscripcion/ID
        public async Task<ActionResult> UnsubscribeInscripcion(int? id) {
            if (id == null) {
                return Content("Debe incluir el id");
            }

            try {
                await _inscripcionEndpoint.Get(id, _userSession.BearerToken);

                var inscripcionTask = _inscripcionEndpoint.Get(id, _userSession.BearerToken);
                await Task.WhenAll(inscripcionTask);

                var inscripcion = _mapper.Map<MvcInscripcionModel>(source: inscripcionTask.Result);

                if (inscripcion.State == MvcInscripcionModel.InscripcionStates.Active) {
                    inscripcion.State = MvcInscripcionModel.InscripcionStates.Canceled;
                } else {
                    return Content("Esta inscripción ya fue cancelada, o ya ha finalizado la fecha del horario consulta");
                }

                var entity = _mapper.Map<Inscripcion>(source: inscripcion);
                await _inscripcionEndpoint.Put(entity, _userSession.BearerToken);
            } catch (UnauthorizedRequestException) {
                return RedirectToAction("AccessDenied", "Error");
            } catch (NotFoundRequestException ex) {
                return Content($"{ex.StatusCode}: Elemento no encontrado");
            } catch (Exception ex) {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }

            // TempData may be used to check in the view whether the deletion was successful or not
            TempData["SuccessMessage"] = "Unsubscribe Sucessfully";
            return Content("OK");
        }

        // New Inscripción - GET Alumno/Subscribe/ID
        [HttpGet]
        public async Task<ActionResult> Subscribe(int? id) {
            if (id == null) {
                return Content("Debe incluir el id");
            }

            try {
                HorarioConsultaFechado entity = await _horarioConsultaFechadoEndpoint.Get(id, _userSession.BearerToken);

                var horarioFechado = _mapper.Map<MvcHorarioConsultaFechadoModel>(source: entity);

                var inscripcion = new MvcInscripcionModel()
                {
                    HorarioConsultaFechado = horarioFechado,
                    HorarioConsultaFechadoId = horarioFechado.Id
                };

                return PartialView("_Subscribe", inscripcion);
            } catch (UnauthorizedRequestException) {
                return Content("No esta autorizado");
            } catch (NotFoundRequestException ex) {
                return Content($"{ex.StatusCode}: Elemento no encontrado");
            } catch (Exception ex) {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }
        }

        // New Inscripción - PUT Alumno/Subscribe/ID (Secured)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Subscribe(
            [Bind(Include = "HorarioConsultaFechadoId, Observation, Topic")]MvcInscripcionModel inscripcion)
        {
            try {
                Usuario user = await _usuarioEndpoint.GetCurrentUsuario(token: _userSession.BearerToken);

                inscripcion.State = MvcInscripcionModel.InscripcionStates.Active;
                inscripcion.AlumnoId = user.Id;

                var entity = _mapper.Map<Inscripcion>(source: inscripcion);

                await _inscripcionEndpoint.Post(entity, _userSession.BearerToken);
            } catch (UnauthorizedRequestException) {
                return RedirectToAction("AccessDeniedPartial", "Error");
            } catch (BadRequestException ex) {
                HorarioConsultaFechado entity = await _horarioConsultaFechadoEndpoint.Get(inscripcion.HorarioConsultaFechadoId, _userSession.BearerToken);
                var horarioFechado = _mapper.Map<MvcHorarioConsultaFechadoModel>(source: entity);

                inscripcion.HorarioConsultaFechado = horarioFechado;

                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_Subscribe", inscripcion);
            } catch (Exception ex) {
                return RedirectToAction("SpecificErrorPartial", "Error", new { error = ex.Message });
            }

            return Content("OK");
        }

        // Edit - GET Alumno/EditInscripcion/ID
        [HttpGet]
        public async Task<ActionResult> EditInscripcion(int? id) {
            {
                if (id == null) {
                    return Content("Debe incluir el id");
                }

                try {
                    var inscripcionTask = _inscripcionEndpoint.Get(id, _userSession.BearerToken);

                    await Task.WhenAll(inscripcionTask);

                    var inscripcion = _mapper.Map<MvcInscripcionModel>(source: inscripcionTask.Result);

                    var viewModel = new EditOwnInscripcionViewModel(inscripcion: inscripcion);

                    return PartialView("_EditOwnInscripcion", viewModel);
                } catch (UnauthorizedRequestException) {
                    return RedirectToAction("AccessDeniedPartial", "Error");
                } catch (NotFoundRequestException ex) {
                    return Content($"{ex.StatusCode}: Elemento no encontrado");
                } catch (Exception ex) {
                    return RedirectToAction("SpecificErrorPartial", "Error", new { error = ex.Message });
                }
            }
        }

        // Edit - PUT Alumno/EditInscripcion/ID (Secured)
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind(Include = "...") is used to avoid overposting attacks
        public async Task<ActionResult> EditInscripcion(EditOwnInscripcionViewModel viewModel) {
            try {
                Usuario user = await _usuarioEndpoint.GetCurrentUsuario(token: _userSession.BearerToken);

                viewModel.SetAlumno(user.Id);

                var entity = _mapper.Map<Inscripcion>(viewModel.Inscripcion);

                if (entity.State == null) { entity.State = Inscripcion.InscripcionStates.Active; }

                await _inscripcionEndpoint.Put(entity, _userSession.BearerToken);
            } catch (UnauthorizedRequestException) {
                return RedirectToAction("AccessDeniedPartial", "Error");
            } catch (BadRequestException ex) {
                viewModel.SetEstadosAsSelectList();

                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_EditOwnInscripcion", viewModel);
            } catch (Exception ex) {
                return RedirectToAction("SpecificErrorPartial", "Error", new { error = ex.Message });
            }

            return Content("OK");
        }
    }
}
