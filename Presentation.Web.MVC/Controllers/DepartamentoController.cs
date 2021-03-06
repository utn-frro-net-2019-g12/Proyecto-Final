﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Presentation.Web.MVC.Models;
using Presentation.Library.Models;
using Presentation.Library.Api.Endpoints.Interfaces;
using Presentation.Library.Api.Exceptions;
using System.Threading.Tasks;
using AutoMapper;
using Presentation.Web.MVC.Filters;
using Presentation.Web.MVC.ViewModels;

namespace Presentation.Web.MVC.Controllers {

    [AuthorizeSelected(Roles = "Admin")]
    public class DepartamentoController : Controller {
        private readonly IDepartamentoEndpoint _departamentoEndpoint;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public DepartamentoController(IDepartamentoEndpoint departamentoEndpoint, IUserSession userSession, IMapper mapper)
        {
            _departamentoEndpoint = departamentoEndpoint;
            _userSession = userSession;
            _mapper = mapper;
        }

        // Index - GET Departamento
        public async Task<ActionResult> Index()
        {
            try
            {
                IEnumerable<Departamento> entities = await _departamentoEndpoint.GetAll(_userSession.BearerToken);

                var departamentos = _mapper.Map<IEnumerable<MvcDepartamentoModel>>(entities);

                var viewModel = new ShowDepartamentosViewModel(departamentos: departamentos);

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

        // Search - GET Departamento by Partial Descripcion
        public async Task<ActionResult> Search(string partialDesc) {
            try {
                IEnumerable<Departamento> entities = await _departamentoEndpoint.GetByPartialDesc(partialDesc, _userSession.BearerToken);

                var departamentos = _mapper.Map<IEnumerable<MvcDepartamentoModel>>(entities);

                var viewModel = new ShowDepartamentosViewModel(departamentos: departamentos, parcialDesc: partialDesc);

                return View("Index", viewModel);
            } catch (UnauthorizedRequestException) {
                return RedirectToAction("AccessDenied", "Error");
            } catch (Exception ex) {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }
        }

        // Delete - DELETE Departamento/ID
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _departamentoEndpoint.Delete(id, _userSession.BearerToken);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDeniedPartial", "Error");
            }
            catch (NotFoundRequestException ex)
            {
                return Content($"{ex.StatusCode}: Elemento no encontrado");
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificErrorPartial", "Error", new { error = ex.Message });
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
        public async Task<ActionResult> Create(MvcDepartamentoModel departamento) {
            try
            {
                var entity = _mapper.Map<Departamento>(departamento);

                await _departamentoEndpoint.Post(entity, _userSession.BearerToken);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDeniedPartial", "Error");
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_Create", departamento);
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificErrorPartial", "Error", new { error = ex.Message });
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
                Departamento entity = await _departamentoEndpoint.Get(id, _userSession.BearerToken);

                var departamento = _mapper.Map<MvcDepartamentoModel>(entity);

                return PartialView("_Edit", departamento);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDeniedPartial", "Error");
            }
            catch (NotFoundRequestException ex)
            {
                return Content($"{ex.StatusCode}: Elemento no encontrado");
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificErrorPartial", "Error", new { error = ex.Message });
            }
        }

        // Edit - PUT Departamento/ID (Secured)
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind(Include = "...") is used to avoid overposting attacks
        public async Task<ActionResult> Edit(MvcDepartamentoModel departamento)
        {
            try
            {
                var entity = _mapper.Map<Departamento>(departamento);

                await _departamentoEndpoint.Put(entity, _userSession.BearerToken);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDeniedPartial", "Error");
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelErrors(ex.Errors);

                return PartialView("_Edit", departamento);
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificErrorPartial", "Error", new { error = ex.Message });
            }

            return Content("OK");
        }
    }
}
