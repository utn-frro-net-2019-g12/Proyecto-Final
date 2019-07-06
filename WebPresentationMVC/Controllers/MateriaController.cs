using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            var response = GlobalApi.WebApiClient.GetAsync("materias/").Result;

            IEnumerable<MvcMateriaModel> materias = response.Content.ReadAsAsync<IEnumerable<MvcMateriaModel>>().Result;

            return View(materias);
        }


        public ActionResult Details(int id)
        {
            var response = GlobalApi.WebApiClient.GetAsync("materias/" + id.ToString()).Result;

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
            var response = GlobalApi.WebApiClient.DeleteAsync("materias/" + id.ToString()).Result;

            // Search what is TempData!
            TempData["SuccessMessage"] = "Deleted Sucessfully";

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(MvcMateriaModel materias)
        {
            var response = GlobalApi.WebApiClient.PostAsJsonAsync("materias", materias).Result;

            // Move this to an action filter
            if (!response.IsSuccessStatusCode)
            {
                ModelStateApi.AddErrors(response, ModelState);

                return View(materias);
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var response = GlobalApi.WebApiClient.GetAsync("materias/" + id).Result;

            if (!response.IsSuccessStatusCode)
            {
                return HttpNotFound();
            }

            MvcMateriaModel materia = response.Content.ReadAsAsync<MvcMateriaModel>().Result;

            return View(materia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind(Include = "...") is used to avoid overposting attacks
        public ActionResult Edit([Bind(Include = "Id, Name, Year, IsElectiva")]MvcMateriaModel materia)
        {
            var response = GlobalApi.WebApiClient.PutAsJsonAsync("materias/" + materia.Id, materia).Result;

            if (!response.IsSuccessStatusCode)
            {
                ModelStateApi.AddErrors(response, ModelState);

                return View(materia);
            }

            return RedirectToAction("Index");
        }
    }
}