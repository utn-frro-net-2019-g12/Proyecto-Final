using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using WebPresentationMVC.Models;

namespace WebPresentationMVC.Api
{
    public class AuthenticationEndpoint : IAuthenticationEndpoint
    {
        IApiHelper _apiHelper;

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
    }
}