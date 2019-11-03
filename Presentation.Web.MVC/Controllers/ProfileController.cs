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

namespace Presentation.Web.MVC.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUsuarioEndpoint _usuarioEndpoint;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public ProfileController(IUsuarioEndpoint usuarioEndpoint, IUserSession userSession, IMapper mapper)
        {
            _usuarioEndpoint = usuarioEndpoint;
            _userSession = userSession;
            _mapper = mapper;
        }

        // GET: Profile
        public ActionResult Index()
        {
            return RedirectToAction("Details");
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            try
            {
                Usuario entity = await _usuarioEndpoint.GetCurrentUsuario(_userSession.BearerToken);

                var currentUsuario = _mapper.Map<MvcUsuarioModel>(entity);

                return View(currentUsuario);
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

        [HttpPost]
        public async Task<ActionResult> Edit(MvcUsuarioModel current)
        {
            try
            {
                var entity = _mapper.Map<Usuario>(current);

                await _usuarioEndpoint.UpdateCurrent(entity, _userSession.BearerToken);
            }
            catch (UnauthorizedRequestException)
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            catch(BadRequestException ex)
            {
                ModelState.AddModelErrors(ex.Errors);

                return View(current);
            }
            catch (Exception ex)
            {
                return RedirectToAction("SpecificError", "Error", new { error = ex.Message });
            }

            return View(current);
        }
    }
}
