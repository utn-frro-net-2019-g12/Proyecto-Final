using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Presentation.Library.Api.Exceptions;
using Presentation.Library.Api.Endpoints.Interfaces;
using System.Net.Http.Headers;
using Presentation.Library.Models;

namespace Presentation.Library.Api.Endpoints.Implementations
{
    public class AuthenticationEndpoint : IAuthenticationEndpoint
    {
        private IApiHelper _apiHelper;

        public AuthenticationEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<Token> GetToken(LoginModel model)
        {
            HttpContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", model.EmailAddress),
                    new KeyValuePair<string, string>("password", model.Password)
                });

            using(var response = await _apiHelper.ApiClient.PostAsync("/Token", content))
            {
                if (!response.IsSuccessStatusCode)
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.BadRequest:
                            throw new BadRequestException(response);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }

                var result = await response.Content.ReadAsAsync<Token>();

                return result;
            }
        }

        public async Task RegisterAccount(RegisterModel model, string token)
        {
            using (var response = await _apiHelper.ApiClient.PostAsJsonAsync("api/account/register", model, x => x.SetAuthHeaders(token)))
            {
                if (!response.IsSuccessStatusCode)
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedRequestException(response);
                        case HttpStatusCode.BadRequest:
                            throw new BadRequestException(response);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }
            }
        }

        public async Task<IEnumerable<string>> GetUserRoles(string token)
        {
            using (var response = await _apiHelper.ApiClient.GetAsync("api/account/getUserRoles", x => x.SetAuthHeaders(token)))
            {
                var result = await response.Content.ReadAsAsync<IEnumerable<string>>();

                return result;
            }
        }


        private void SetToken(HttpRequestMessage r, string token)
        {
            r.Headers.Authorization = AuthenticationHeaderValue.Parse(token);
        }
    }
}
