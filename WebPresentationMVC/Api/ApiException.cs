using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebPresentationMVC.Api
{
    public class ApiErrorsException : Exception
    {
        private HttpResponseMessage _response;

        public ApiErrorsException(HttpResponseMessage response)
        {
            _response = response;
            Errors = new Dictionary<string, string>();
        }

        public HttpStatusCode StatusCode {
            get
            {
                return _response.StatusCode;
            }
        }
        public Dictionary<string,string> Errors { get;}
    }
}
