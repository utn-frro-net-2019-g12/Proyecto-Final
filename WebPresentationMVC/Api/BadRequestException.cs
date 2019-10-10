using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebPresentationMVC.Api
{
    public class BadRequestException : Exception, IApiException
    {
        private HttpResponseMessage _response;

        public BadRequestException(HttpResponseMessage response)
        {
            _response = response;
            Errors = SetErrors(response);
        }

        public HttpStatusCode StatusCode {
            get
            {
                return _response.StatusCode;
            }
        }

        public string ReasonPhrase
        {
            get
            {
                return _response.ReasonPhrase;
            }
        }

        public Dictionary<string, string> Errors { get; set; }

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
