using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Presentation.Library.Api.Exceptions
{
    public class NotFoundRequestException : Exception, IApiException
    {
        public NotFoundRequestException(HttpResponseMessage response, object id)
        {
            Response = response;
            NotFoundElement = id.ToString();
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

        public string NotFoundElement { get; }
    }
}