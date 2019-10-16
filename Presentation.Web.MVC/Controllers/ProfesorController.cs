using AutoMapper;
using Presentation.Library.Api.Endpoints.Interfaces;
using Presentation.Library.Api.Exceptions;
using Presentation.Library.Models;
using Presentation.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebPresentationMVC.Controllers
{
    public class ProfesorController : Controller
    {
        private readonly IHorarioConsultaEndpoint _horariosConsultaEndpoint;
        private readonly IInscripcionEndpoint _inscripcionEndpoint;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public ProfesorController(IHorarioConsultaEndpoint horarioConsultaEndpoint, IInscripcionEndpoint inscripcionEndpoint
            , IUserSession userSession, IMapper mapper)
        {
            _horariosConsultaEndpoint = horarioConsultaEndpoint;
            _inscripcionEndpoint = inscripcionEndpoint;
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
                IEnumerable<HorarioConsulta> entities = await _horariosConsultaEndpoint.GetByCurrentUserProfessor(_userSession.BearerToken);

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
    }
}