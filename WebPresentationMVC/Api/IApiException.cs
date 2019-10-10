using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace WebPresentationMVC.Api
{
    public interface IApiException
    {
        HttpStatusCode StatusCode { get; }
        string ReasonPhrase { get; }
    }
}