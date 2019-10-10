using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPresentationMVC.Api.Exceptions;
using WebPresentationMVC.Api.Endpoints.Interfaces;
using System.Threading.Tasks;
using WebPresentationMVC.Models;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace WebPresentationMVC.Api.Endpoints.Implementations
{
    public class UsuarioEndpoint : IUsuarioEndpoint
    {
        private IApiHelper _apiHelper;
        private readonly IUserSession _userSession;

        public UsuarioEndpoint(IApiHelper apiHelper, IUserSession userSession)
        {
            _apiHelper = apiHelper;
            _userSession = userSession;
        }

        public async Task<IEnumerable<MvcUsuarioModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/usuarios", x => SetToken(x)))
            {
                if (!response.IsSuccessStatusCode)
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedRequestException(response);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }

                var result = await response.Content.ReadAsAsync<IEnumerable<MvcUsuarioModel>>();

                return result;
            }
        }

        public async Task<MvcUsuarioModel> Get(object id)
        {
            using (var response = await _apiHelper.ApiClient.GetAsync("api/usuarios/" + id, x => SetToken(x)))
            {
                if (!response.IsSuccessStatusCode)
                {
                    switch (response.StatusCode) // Add NotFound Case
                    {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedRequestException(response);
                        case HttpStatusCode.NotFound:
                            throw new NotFoundRequestException(response, id);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }

                var result = await response.Content.ReadAsAsync<MvcUsuarioModel>();

                return result;
            }
        }

        public async Task Delete(object id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync("api/usuarios/" + id, x => SetToken(x)))
            {
                if (!response.IsSuccessStatusCode)
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedRequestException(response);
                        case HttpStatusCode.NotFound:
                            throw new NotFoundRequestException(response, id);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }
            }
        }



        public async Task Post(MvcUsuarioModel entity)
        {
            using (var response = await _apiHelper.ApiClient.PostAsJsonAsync("api/usuarios", entity, x => SetToken(x)))
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

        public async Task Put(MvcUsuarioModel entity)
        {
            using (var response = await _apiHelper.ApiClient.PutAsJsonAsync("api/usuarios/" + entity.Id, entity, x => SetToken(x)))
            {
                if (!response.IsSuccessStatusCode)
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedRequestException(response);
                        case HttpStatusCode.BadRequest:
                            throw new BadRequestException(response);
                        case HttpStatusCode.NotFound:
                            throw new NotFoundRequestException(response, entity.Id);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }
            }
        }

        private void SetToken(HttpRequestMessage r)
        {
            r.Headers.Authorization = AuthenticationHeaderValue.Parse(_userSession.BearerToken);
        }
    }
}