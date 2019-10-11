using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Presentation.Library.Api.Exceptions
{
    public class UnauthorizedRequestException : Exception, IApiException
    {
        public UnauthorizedRequestException(HttpResponseMessage response)
        {
            Response = response;
        }

        public HttpResponseMessage Response { get; }

        public HttpStatusCode StatusCode
        {
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
    }
}