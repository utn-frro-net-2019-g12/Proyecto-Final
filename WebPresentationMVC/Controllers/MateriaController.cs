using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebPresentationMVC.Models;

namespace WebPresentationMVC.Controllers
{
    public class MateriaController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            var response = GlobalVariables.WebApiClient.GetAsync("materias/").Result;

            IEnumerable<MvcMateriaModel> materias = response.Content.ReadAsAsync<IEnumerable<MvcMateriaModel>>().Result;

            return View(materias);
        }


        public ActionResult Details(int id)
        {
            var response = GlobalVariables.WebApiClient.GetAsync("materias/" + id.ToString()).Result;

            if (!response.IsSuccessStatusCode)
            {
                return View(response.Content.ReadAsAsync<ModelState>().Result);
            }

            var materia = response.Content.ReadAsAsync<MvcMateriaModel>().Result;
    
            return View(materia);
        }


        // DELETE Product/5
        public ActionResult Delete(int id)
        {
            var response = GlobalVariables.WebApiClient.DeleteAsync("materias/" + id.ToString()).Result;

            // Search what is TempData!
            TempData["SuccessMessage"] = "Deleted Sucessfully";

            return RedirectToAction("Index");
        }
    }
}