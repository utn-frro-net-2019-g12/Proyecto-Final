using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace WebPresentationMVC
{
    public static class GlobalApi
    {
        public static HttpClient WebApiClient = new HttpClient();

        static GlobalApi()
        {
            WebApiClient.BaseAddress = new Uri("http://localhost:2021/api/");
            WebApiClient.DefaultRequestHeaders.Clear();
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}