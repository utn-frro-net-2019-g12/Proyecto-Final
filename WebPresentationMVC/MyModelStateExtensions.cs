using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

namespace WebPresentationMVC
{
    public static class MyModelStateExtensions
    {
        public static void AddModelErrorsFromResponse(this ModelStateDictionary modelState, HttpResponseMessage response)
        {
            modelState.AddModelError("", "Por favor corrija las entradas");

            JObject httpError = response.Content.ReadAsAsync<JObject>().Result;

            JToken errors = httpError["ModelState"];

            foreach (JProperty error in errors.Skip(1))
            {
                // This decouples the name sent by the api from the one that's used to add a modelError
                string errorKey = modelState.Keys.Where(e => e.Contains(error.Name.Substring(error.Name.IndexOf('.') + 1))).FirstOrDefault();
                foreach (JArray messages in error)
                {
                    foreach (string message in messages)
                    {
                        modelState.AddModelError(errorKey, message.ToString());
                    }
                }
            }
        }
    }
}