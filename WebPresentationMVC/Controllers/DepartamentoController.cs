using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebPresentationMVC.Models;
using WebPresentationMVC.Api.Endpoints.Interfaces;
using WebPresentationMVC.Api.Exceptions;
using System.Threading.Tasks;

namespace WebPresentationMVC.Controllers {

    [Authorize]
    public class DepartamentoController : Controller {
        private IDepartamentoEndpoint _departamentoEndpoint;

        public DepartamentoController(IDepartamentoEndpoint departamentoEndpoint)
        {
            _departamentoEndpoint = departamentoEndpoint;
        }

        // Index - GET Departamento
        public async Task<ActionResult> Index()
        {
            try
            {
                IEnumerable<MvcDepartamentoModel> entities = await _departamentoEndpoint.GetAll();

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

        // Details - GET Departamento/ID
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                MvcDepartamentoModel entity = await _departamentoEndpoint.Get(id);

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

        // Delete - DELETE Departamento/ID
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _departamentoEndpoint.Delete(id);
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

        // Create - POST Departamento
        [HttpPost]
        public async Task<ActionResult> Create(MvcDepartamentoModel entity) {
            try
            {
                await _departamentoEndpoint.Post(entity);
            }
            catch (UnauthorizedRequestException)
            {
                return Content("No tiene acceso");
            }
            catch (BadRequestException ex)
            {
                    ModelState.AddModelErrors(ex.Errors);

                    return PartialView("_Create", entity);
            }
            catch (Exception ex)
            {
                return Content($"{ex.Message} Ha ocurrido un error. Por favor contacte a soporte");
            }

            return Content("OK");
        }

        // Edit - GET Departamento/ID
        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return Content("Debe incluir el id");
            }

            try
            {
                MvcDepartamentoModel entity = await _departamentoEndpoint.Get(id);

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

        // Edit - PUT Departamento/ID (Secured)
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind(Include = "...") is used to avoid overposting attacks
        public async Task<ActionResult> Edit(MvcDepartamentoModel entity)
        {
            try
            {
                await _departamentoEndpoint.Put(entity);
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
