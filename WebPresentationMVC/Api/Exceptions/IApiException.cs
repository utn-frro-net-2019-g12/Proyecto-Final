using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace WebPresentationMVC.Api
{
    public interface IApiException
    {
        HttpResponseMessage Response { get; }
        HttpStatusCode StatusCode { get; }
        string ReasonPhrase { get; }
    }
}