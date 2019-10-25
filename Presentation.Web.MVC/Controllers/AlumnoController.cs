using AutoMapper;
using Presentation.Library.Api.Endpoints.Interfaces;
using Presentation.Library.Api.Exceptions;
using Presentation.Library.Models;
using Presentation.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Presentation.Web.MVC.Controllers
{
    public class AlumnoController : Controller
    {
        private readonly IInscripcionEndpoint _inscripcionEndpoint;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public AlumnoController(IInscripcionEndpoint inscripcionEndpoint, IUserSession userSession, IMapper mapper)
        {
            _inscripcionEndpoint = inscripcionEndpoint;
            _userSession = userSession;
            _mapper = mapper;
        }

        // GET: Alumno
        public ActionResult Index()
        {
            return RedirectToAction("MisInscripciones");
        }

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

        public async Task<ActionResult> NuevaConsulta()
        {
            return View();
        }
    }
}