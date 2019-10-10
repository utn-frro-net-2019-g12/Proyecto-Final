using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebPresentationMVC.Api.Exceptions
{
    public class BadRequestException : Exception, IApiException
    {
        public BadRequestException(HttpResponseMessage response)
        {
            Response = response;
            Errors = SetErrors(response);
        }

        public HttpResponseMessage Response { get; }

        public HttpStatusCode StatusCode {
            get
            {
                return Response.StatusCode;
            }
        }

        public string ReasonPhrase
        {
            get
            {
                return Response.ReasonPhrase;
            }
        }

        public Dictionary<string, string> Errors { get; }

        public Dictionary<string, string> SetErrors(HttpResponseMessage response)
        {
            var errors = new Dictionary<string, string>();

            var result = response.Content.ReadAsAsync<ErrorResponse>().Result;

            if (result.ModelState != null)
            {
                foreach (KeyValuePair<string, string[]> item in result.ModelState)
                {
                    errors.Add(item.Key, string.Join(".", item.Value));
                }
            }
            else if (result.Error_description != null)
            {
                errors.Add("", result.Error_description);
            }
            else
            {
                errors.Add("", "Contacte a soporte para mas detalles");
            }

            return errors;
        }
    }
}
