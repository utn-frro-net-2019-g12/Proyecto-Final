﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebPresentationMVC.Controllers
{
    [Authorize]
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PageNotFound()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult AccessDeniedPartial()
        {
            return PartialView("_AccessDenied");
        }

        public ActionResult SpecificError(string error)
        {
            ViewBag.ErrorMessage = error;
            return View();
        }

        public ActionResult SpecificErrorPartial(string error)
        {
            ViewBag.ErrorMessage = error;
            return PartialView("_SpecificError");
        }
    }
}