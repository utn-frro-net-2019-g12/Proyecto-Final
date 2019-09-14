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

        public void AddTokenToHeaders(string token)
        {
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.DefaultRequestHeaders.Add("Authorization", token);
        }

        public async Task<ApiErrorsException> CreateApiErrorsException(HttpResponseMessage response)
        {
            var ex = new ApiErrorsException(response);

            var result = await response.Content.ReadAsAsync<ErrorResponse>();
            
            if (result.ModelState != null)
            {
                foreach(KeyValuePair<string, string[]> item in result.ModelState)
                {
                    ex.Errors.Add(item.Key, string.Join(".", item.Value));
                }
            }
            else if(result.Error_description != null)
            {
                ex.Errors.Add("", result.Error_description);
            }
            else
            {
                ex.Errors.Add("", "Contacte a soporte para mas detalles");
            }

            return ex;
        }
    }
}
