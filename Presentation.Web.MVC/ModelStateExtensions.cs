using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

namespace Presentation.Web.MVC
{
    public static class ModelStateExtensions
    {
        public static void AddModelErrors(this ModelStateDictionary modelState, Dictionary<string,string> errorItems)
        {
            foreach(KeyValuePair<string,string> item in errorItems)
            {
                // This decouples the name sent by the api from the one that's used to add a modelError
                string errorKey = item.Key == "" ? "" : modelState.Keys.Where(e => e.Contains(item.Key.Substring(item.Key.IndexOf('.') + 1))).FirstOrDefault();

                modelState.AddModelError(errorKey ?? "", item.Value);
            }
        }
    }
}