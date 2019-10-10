using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebPresentationMVC.Api;
using WebPresentationMVC.Models;
using WebPresentationMVC.Api.Exceptions;
using WebPresentationMVC.Api.Endpoints.Interfaces;
using System.Net;
using System.Net.Http.Headers;

namespace WebPresentationMVC.Api.Endpoints.Implementations
{
    class DepartamentoEndpoint : IDepartamentoEndpoint
    {
        private IApiHelper _apiHelper;
        private readonly IUserSession _userSession;

        public DepartamentoEndpoint(IApiHelper apiHelper, IUserSession userSession)
        {
            _apiHelper = apiHelper;
            _userSession = userSession;
        }


        public async Task<IEnumerable<MvcDepartamentoModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/departamentos", x => SetToken(x)))
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

                var result = await response.Content.ReadAsAsync<IEnumerable<MvcDepartamentoModel>>();

                return result;
            }
        }

        public async Task<MvcDepartamentoModel> Get(object id)
        {
            using (var response = await _apiHelper.ApiClient.GetAsync("api/departamentos/" + id, x => SetToken(x)))
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

                var result = await response.Content.ReadAsAsync<MvcDepartamentoModel>();

                return result;
            }
        }

        public async Task Delete(object id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync("api/departamentos/" + id, x => SetToken(x)))
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

        public async Task Post(MvcDepartamentoModel entity)
        {
            using (var response = await _apiHelper.ApiClient.PostAsJsonAsync("api/departamentos", entity, x => SetToken(x)))
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

        public async Task Put(MvcDepartamentoModel entity)
        {
            using (var response = await _apiHelper.ApiClient.PutAsJsonAsync("api/departamentos/" + entity.Id, entity, x => SetToken(x)))
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
