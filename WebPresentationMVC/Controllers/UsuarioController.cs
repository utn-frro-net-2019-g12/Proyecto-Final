using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebPresentationMVC.Models;
using WebPresentationMVC.ViewModels;
using WebPresentationMVC.Api.Exceptions;
using WebPresentationMVC.Api.Endpoints.Interfaces;
using System.Threading.Tasks;

namespace WebPresentationMVC.Controllers {

    [Authorize]
    public class UsuarioController : Controller {
        private IAuthenticationEndpoint _authenticationEndpoint;
        private IUsuarioEndpoint _usuarioEndpoint;

        public UsuarioController(IUsuarioEndpoint usuarioEndpoint, IAuthenticationEndpoint authenticationEndpoint)
        {
            _authenticationEndpoint = authenticationEndpoint;
            _usuarioEndpoint = usuarioEndpoint;
        }

        // Index - GET Usuario
        public async Task<ActionResult> Index()
        {
            try
            {
                IEnumerable<MvcUsuarioModel> entities = await _usuarioEndpoint.GetAll();

                return View(entities);
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

        // Details - GET Usuario/ID
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                MvcUsuarioModel entity = await _usuarioEndpoint.Get(id);

                return View(entity);
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

        // Delete - DELETE Usuario/ID
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _usuarioEndpoint.Delete(id);
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
        public async Task<ActionResult> Create(MvcUsuarioModel entity) {

            var account = new RegisterModel()
            {
                Email = entity.Email,
                Password = "Default1?",
                ConfirmPassword = "Default1?"
            };

            bool errorBadRequest = false;

            try
            {
                try
                {
                    await _authenticationEndpoint.RegisterAccount(account);
                }
                catch (BadRequestException ex)
                {
                    ModelState.AddModelErrors(ex.Errors);

                    errorBadRequest = true;
                }

                try
                {
                    await _usuarioEndpoint.Post(entity);
                }
                catch (BadRequestException ex)
                {
                    ModelState.AddModelErrors(ex.Errors);

                    errorBadRequest = true;
                }
            }
            catch (UnauthorizedRequestException)
            {
                return Content("No tiene acceso");
            }
            catch (Exception ex)
            {
                return Content($"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte");
            }


            if (errorBadRequest)
            {
                return PartialView("_Create", entity);
            }

            return Content("OK");
        }

        // Edit - GET Usuario/ID
        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return Content("Debe incluir el id");
            }

            try
            {
                MvcUsuarioModel entity = await _usuarioEndpoint.Get(id);

                return PartialView("_Edit", entity);
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

        // Edit - PUT Usuario/ID (Secured)
        [HttpPost]
        // Bind(Include = "...") is used to avoid overposting attacks
        public async Task<ActionResult> Edit(MvcUsuarioModel entity)
        {
            try
            {
                await _usuarioEndpoint.Put(entity);
            }
            catch (UnauthorizedRequestException)
            {
                return Content("No tiene acceso");
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_Edit", entity);
            }
            catch (Exception ex)
            {
                return Content($"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte");
            }

            return Content("OK");
        }
    }
}
