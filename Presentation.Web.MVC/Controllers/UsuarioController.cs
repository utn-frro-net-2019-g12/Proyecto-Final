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
using Presentation.Library.Api.Exceptions;
using Presentation.Library.Api.Endpoints.Interfaces;
using System.Threading.Tasks;
using AutoMapper;
using Presentation.Web.MVC.Filters;

namespace Presentation.Web.MVC.Controllers {

    [AuthorizeSelected(Roles = "Admin")]
    public class UsuarioController : Controller {
        private readonly IAuthenticationEndpoint _authenticationEndpoint;
        private readonly IUsuarioEndpoint _usuarioEndpoint;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioEndpoint usuarioEndpoint, IAuthenticationEndpoint authenticationEndpoint
            , IUserSession userSession, IMapper mapper)
        {
            _authenticationEndpoint = authenticationEndpoint;
            _usuarioEndpoint = usuarioEndpoint;
            _userSession = userSession;
            _mapper = mapper;
        }

        // Index - GET Usuario
        public async Task<ActionResult> Index()
        {
            try
            {
                IEnumerable<Usuario> entities = await _usuarioEndpoint.GetAll(_userSession.BearerToken);

                var usuarios = _mapper.Map<IEnumerable<MvcUsuarioModel>>(entities);

                return View(usuarios);
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

        // Search - GET Usuario by Partial Descripcion
        public async Task<ActionResult> Search(string partialDesc) {
            try {
                IEnumerable<Usuario> entities = await _usuarioEndpoint.GetByPartialDesc(partialDesc, _userSession.BearerToken);

                var usuarios = _mapper.Map<IEnumerable<MvcUsuarioModel>>(entities);

                return View("Index", usuarios);
            } catch (UnauthorizedRequestException) {
                return RedirectToAction("AccessDenied", "Error");
            } catch (Exception ex) {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }
        }

        // Search - GET Usuario (Profesor) by Partial Descripcion
        public async Task<ActionResult> SearchProfesor(string partialDesc) {
            try {
                IEnumerable<Usuario> entities = await _usuarioEndpoint.GetProfesoresByPartialDesc(partialDesc, _userSession.BearerToken);

                var profesores = _mapper.Map<IEnumerable<MvcUsuarioModel>>(entities);

                return View("Index", profesores);
            } catch (UnauthorizedRequestException) {
                return RedirectToAction("AccessDenied", "Error");
            } catch (Exception ex) {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }
        }

        // Search - GET Usuario (Alumno) by Partial Descripcion
        public async Task<ActionResult> SearchAlumno(string partialDesc) {
            try {
                IEnumerable<Usuario> entities = await _usuarioEndpoint.GetAlumnosByPartialDesc(partialDesc, _userSession.BearerToken);

                var alumnos = _mapper.Map<IEnumerable<MvcUsuarioModel>>(entities);

                return View("Index", alumnos);
            } catch (UnauthorizedRequestException) {
                return RedirectToAction("AccessDenied", "Error");
            } catch (Exception ex) {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }
        }

        // Details - GET Usuario/ID
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                Usuario entity = await _usuarioEndpoint.Get(id, _userSession.BearerToken);

                var usuario = _mapper.Map<MvcUsuarioModel>(entity);

                return View(usuario);
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

        // Delete - DELETE Usuario/ID
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _usuarioEndpoint.Delete(id, _userSession.BearerToken);
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
        public ActionResult Create() {
            return PartialView("_Create");
        }


        // Create - POST Usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MvcUsuarioModel usuario) {

            var account = new RegisterModel()
            {
                Email = usuario.Email,
                Password = "Default1?",
                ConfirmPassword = "Default1?"
            };

            bool errorBadRequest = false;

            try
            {
                try
                {
                    await _authenticationEndpoint.RegisterAccount(account, _userSession.BearerToken);
                }
                catch (BadRequestException ex)
                {
                    ModelState.AddModelErrors(ex.Errors);

                    errorBadRequest = true;
                }

                try
                {
                    var entity = _mapper.Map<Usuario>(usuario);

                    await _usuarioEndpoint.Post(entity, _userSession.BearerToken);
                }
                catch (BadRequestException ex)
                {
                    ModelState.AddModelErrors(ex.Errors);

                    errorBadRequest = true;
                }
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }


            if (errorBadRequest)
            {
                return PartialView("_Create", usuario);
            }

            return Content("OK");
        }

        // Edit - GET Usuario/ID
        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            try
            {
                Usuario entity = await _usuarioEndpoint.Get(id, _userSession.BearerToken);

                var usuario = _mapper.Map<MvcUsuarioModel>(entity);

                return PartialView("_Edit",usuario);
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

        // Edit - PUT Usuario/ID (Secured)
        [HttpPost]
        // Bind(Include = "...") is used to avoid overposting attacks
        public async Task<ActionResult> Edit(MvcUsuarioModel usuario)
        {
            try
            {
                var entity = _mapper.Map<Usuario>(usuario);

                await _usuarioEndpoint.Put(entity, _userSession.BearerToken);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_Edit", usuario);
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }

            return Content("OK");
        }
    }
}
