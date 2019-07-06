using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

namespace WebPresentationMVC
{
    public static class ModelStateApi
    {
        public static void AddErrors(HttpResponseMessage response, ModelStateDictionary modelState)
        {
            var httpError = response.Content.ReadAsAsync<JObject>().Result;

            var errors = httpError["ModelState"];

            foreach (var error in errors.Skip(1))
            {
                foreach (var message in error)
                {
                    modelState.AddModelError("", message.ToString().Trim('[', ']').Replace('"', ' '));
                }
            }
        }
    }
}