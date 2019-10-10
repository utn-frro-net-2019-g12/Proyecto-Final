using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPresentationMVC.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ComponentModel;
using System.Net;
using WebPresentationMVC.Api.Exceptions;
using WebPresentationMVC.Api.Endpoints.Interfaces;

namespace WebPresentationMVC.Api.Endpoints.Implementations
{
    public class MateriaEndpoint : IMateriaEndpoint
    {
        private IApiHelper _apiHelper;
        private readonly IUserSession _userSession;

        public MateriaEndpoint(IApiHelper apiHelper, IUserSession userSession)
        {
            _apiHelper = apiHelper;
            _userSession = userSession;
        }

        public async Task<IEnumerable<MvcMateriaModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/materias/departamento", x => SetToken(x)))
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

                var result = await response.Content.ReadAsAsync<IEnumerable<MvcMateriaModel>>();

                return result;
            }
        }

        public async Task<MvcMateriaModel> Get(object id)
        {
            using (var response = await _apiHelper.ApiClient.GetAsync("api/materias/" + id, x => SetToken(x)))
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

                var result = await response.Content.ReadAsAsync<MvcMateriaModel>();

                return result;
            }
        }

        public async Task Delete(object id)
        {
            using (HttpResponseMessage response= await _apiHelper.ApiClient.DeleteAsync("api/materias/" + id, x => SetToken(x)))
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

        public async Task Post(MvcMateriaModel entity)
        {
            using (var response = await _apiHelper.ApiClient.PostAsJsonAsync("api/materias", entity, x => SetToken(x)))
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

        public async Task Put(MvcMateriaModel entity)
        {
            using (var response = await _apiHelper.ApiClient.PutAsJsonAsync("api/materias/" + entity.Id, entity, x => SetToken(x)))
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
