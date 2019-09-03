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

            // ModelState errors that are sent by the API, most endpoints send these
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

            // Login error, sent from /Token endpoint
            JToken otherError = httpError["error_description"];

            if(otherError != null)
            {
                modelState.AddModelError("", otherError.ToString());
            }

            // These types of error are not supposed to happen in parallel. Other errors may be ignored and not deserialized
            if (errors == null && otherError == null)
            {
                modelState.AddModelError("", "Contacte a soporte para mas detalles");
            }
        }
    }
}