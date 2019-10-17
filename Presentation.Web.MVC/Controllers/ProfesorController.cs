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
        private readonly IInscripcionEndpoint _inscripcionEndpoint;
        private readonly IMateriaEndpoint _materiaEndpoint;
        private readonly IUsuarioEndpoint _usuarioEndpoint;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public ProfesorController(IHorarioConsultaEndpoint horarioConsultaEndpoint, IInscripcionEndpoint inscripcionEndpoint
            , IMateriaEndpoint materiaEndpoint, IUsuarioEndpoint usuarioEndpoint, IUserSession userSession, IMapper mapper)
        {
            _horarioConsultaEndpoint = horarioConsultaEndpoint;
            _inscripcionEndpoint = inscripcionEndpoint;
            _materiaEndpoint = materiaEndpoint;
            _usuarioEndpoint = usuarioEndpoint;
            _userSession = userSession;
            _mapper = mapper;
        }

        // GET: ProfesorActions
        public ActionResult Index()
        {
            return RedirectToAction("InscripcionesRecibidas");
        }

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
                return Content("No tiene acceso");
            }
            catch (Exception ex)
            {
                return Content($"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte");
            }   
        }

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
                return Content("No tiene acceso");
            }
            catch (Exception ex)
            {
                return Content($"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte");
            }   
        }

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
                return Content("No tiene acceso");
            }
            catch (Exception ex)
            {
                return Content($"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte");
            }
        }

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
                return Content("No tiene acceso");
            }
            catch (BadRequestException ex)
            {
                IEnumerable<Materia> entities = await _materiaEndpoint.GetAll(_userSession.BearerToken);;

                var materias = _mapper.Map<IEnumerable<MvcMateriaModel>>(source: entities);
                viewModel.SetMateriasAsSelectList(materias);

                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_CreateOwnHorario", viewModel);
            }
            catch (Exception ex)
            {
                return Content($"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte");
            }

            return Content("OK");
        }


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
                return Content("No tiene acceso");
            }
            catch (BadRequestException ex)
            {
                var profesoresTask = _usuarioEndpoint.GetAll(_userSession.BearerToken);
                var materiasTask = _materiaEndpoint.GetAll(_userSession.BearerToken); // May throw an exception, so that is why the modal is not showing in nico user

                await Task.WhenAll(profesoresTask, materiasTask);

                var profesores = _mapper.Map<IEnumerable<MvcUsuarioModel>>(source: profesoresTask.Result);
                var materias = _mapper.Map<IEnumerable<MvcMateriaModel>>(source: materiasTask.Result);

                viewModel.SetMateriasAsSelectList(materias);

                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_EditOwnHorario", viewModel);
            }
            catch (Exception ex)
            {
                return Content($"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte");
            }

            return Content("OK");
        }
    }
}