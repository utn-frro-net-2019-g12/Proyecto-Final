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
            JObject httpError = response.Content.ReadAsAsync<JObject>().Result;

            JToken errors = httpError["ModelState"];

            if(errors != null)
            {
                foreach (JProperty error in errors.Skip(1))
                {
                    foreach (JArray messages in error)
                    {
                        foreach (string message in messages)
                        {
                            modelState.AddModelError(error.Name.Substring(error.Name.IndexOf('.') + 1), message.ToString());
                        }
                    }
                }
            }

            JToken otherError = httpError["error_description"];

            if(otherError != null)
            {
                modelState.AddModelError("", otherError.ToString());
            }
            else
            {
                modelState.AddModelError("", "Contacte a soporte para mas detalles");
            }
        }
    }
}