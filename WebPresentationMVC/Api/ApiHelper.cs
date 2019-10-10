using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebPresentationMVC.Models;
using Newtonsoft.Json.Linq;

namespace WebPresentationMVC.Api
{
    public class ApiHelper : IApiHelper
    {
        public HttpClient _apiClient;

        public ApiHelper()
        {
            InitializeClient();
        }

        public HttpClient ApiClient
        {
            get
            {
                return _apiClient;
            }
        }

        public void InitializeClient()
        {
            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri("http://localhost:2021/");
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
