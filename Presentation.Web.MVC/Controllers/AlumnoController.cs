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
    public class AlumnoController : Controller
    {
        private readonly IInscripcionEndpoint _inscripcionEndpoint;
        private readonly IUsuarioEndpoint _usuarioEndpoint;
        private readonly IHorarioConsultaFechadoEndpoint _horarioConsultaFechadoEndpoint;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public AlumnoController(IInscripcionEndpoint inscripcionEndpoint, IUsuarioEndpoint usuarioEndpoint,
            IHorarioConsultaFechadoEndpoint horarioConsultaFechadoEndpoint, IUserSession userSession, IMapper mapper) {
            _inscripcionEndpoint = inscripcionEndpoint;
            _usuarioEndpoint = usuarioEndpoint;
            _horarioConsultaFechadoEndpoint = horarioConsultaFechadoEndpoint;
            _userSession = userSession;
            _mapper = mapper;
        }

        // GET: Alumno
        public ActionResult Index()
        {
            return RedirectToAction("MisInscripciones");
        }

        // Index Nueva Inscripción (By Current Alumno User)
        public ActionResult NuevaConsulta() {
            return View();
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

        // Search --> Búsquedas de Nueva Inscripción & Mis Insc

        // Delete - DELETE Alumno/DeleteInscripcion/ID
        public async Task<ActionResult> DeleteInscripcion(int id) {
            try {
                await _inscripcionEndpoint.Delete(id, _userSession.BearerToken);
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

        // Edit - GET Alumno/EditInscripcion/ID
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

                    var viewModel = new EditInscripcionViewModel(inscripcion: inscripcion, horariosConsultaFechados: horariosConsultaFechados, alumnos: alumnos);

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

        // Edit - PUT Alumno/EditInscripcion/ID (Secured)
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind(Include = "...") is used to avoid overposting attacks
        public async Task<ActionResult> EditInscripcion(EditInscripcionViewModel viewModel) {
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
