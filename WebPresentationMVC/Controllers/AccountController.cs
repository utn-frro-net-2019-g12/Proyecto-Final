using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Presentation.Library.Models;
using Presentation.Library.Api.Endpoints.Interfaces;
using Presentation.Library.Api.Exceptions;
using WebPresentationMVC.Models;

namespace WebPresentationMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IAuthenticationEndpoint _authenticationEndpoint;
        private IUserSession _userSession;

        public AccountController(IAuthenticationEndpoint authenticationEndpoint, IUserSession userSession)
        {
            _authenticationEndpoint = authenticationEndpoint;
            _userSession = userSession;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            var usuarioDefaultModel = new LoginModel { EmailAddress = "ale@example.com", Password = "Example1?" };

            return View(usuarioDefaultModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            try
            {
                var token = await _authenticationEndpoint.GetToken(model);

                AuthenticationProperties options = new AuthenticationProperties();

                options.AllowRefresh = true;
                options.IsPersistent = true;
                options.ExpiresUtc = DateTime.UtcNow.AddSeconds(int.Parse(token.expires_in));

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, model.EmailAddress),
                    new Claim("AcessToken", string.Format("Bearer {0}", token.access_token)),
                };

                var identity = new ClaimsIdentity(claims, "ApplicationCookie");

                Request.GetOwinContext().Authentication.SignIn(options, identity);

                return RedirectToAction("Dashboard", "Home");
            }
            catch(BadRequestException ex)
            {
                ModelState.AddModelErrors(ex.Errors);

                return View(model);
            }
        }

        [Authorize]
        public ActionResult LogOut()
        {
            Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");

            return RedirectToAction("Login");
        }
    }
}