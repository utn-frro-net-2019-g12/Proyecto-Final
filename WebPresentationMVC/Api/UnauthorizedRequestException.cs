using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace WebPresentationMVC.Api
{
    public class UnauthorizedRequestException : Exception, IApiException
    {
        private HttpResponseMessage _response;

        public UnauthorizedRequestException(HttpResponseMessage response)
        {
            _response = response;
        }

        public HttpStatusCode StatusCode
        {
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
    }
}