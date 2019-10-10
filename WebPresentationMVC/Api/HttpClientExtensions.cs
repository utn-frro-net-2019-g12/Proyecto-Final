using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace WebPresentationMVC.Api
{
    public static class HttpClientExtensions
    {
        // Action is a delegate, it is specially useful for setting headers for each request 
        public static Task<HttpResponseMessage> GetAsync
            (this HttpClient httpClient, string uri, Action<HttpRequestMessage> preAction)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            // Applies delegate to that request object
            preAction(httpRequestMessage);

            return httpClient.SendAsync(httpRequestMessage);
        }

        public static Task<HttpResponseMessage> DeleteAsync
            (this HttpClient httpClient, string uri, Action<HttpRequestMessage> preAction)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            preAction(httpRequestMessage);

            return httpClient.SendAsync(httpRequestMessage); 
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync<T>
            (this HttpClient httpClient, string uri, T value, Action<HttpRequestMessage> preAction)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new ObjectContent<T>
                    (value, new JsonMediaTypeFormatter(), (MediaTypeHeaderValue)null)
            };

            preAction(httpRequestMessage);

            return httpClient.SendAsync(httpRequestMessage);
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync<T>
            (this HttpClient httpClient, string uri, T value, Action<HttpRequestMessage> preAction)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, uri)
            {
                Content = new ObjectContent<T>
                    (value, new JsonMediaTypeFormatter(), (MediaTypeHeaderValue)null)
            };

            preAction(httpRequestMessage);

            return httpClient.SendAsync(httpRequestMessage);
        }
    }
}