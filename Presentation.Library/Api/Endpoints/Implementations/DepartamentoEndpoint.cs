using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Presentation.Library.Api.Exceptions;
using Presentation.Library.Api.Endpoints.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using Presentation.Library.Models;

namespace Presentation.Library.Api.Endpoints.Implementations
{
    public class DepartamentoEndpoint : IDepartamentoEndpoint
    {
        private IApiHelper _apiHelper;

        public DepartamentoEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }


        public async Task<IEnumerable<Departamento>> GetAll(string token)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/departamentos", x => SetToken(x, token)))
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

                var result = await response.Content.ReadAsAsync<IEnumerable<Departamento>>();

                return result;
            }
        }

        public async Task<Departamento> Get(object id, string token)
        {
            using (var response = await _apiHelper.ApiClient.GetAsync("api/departamentos/" + id, x => SetToken(x, token)))
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

                var result = await response.Content.ReadAsAsync<Departamento>();

                return result;
            }
        }

        public async Task Delete(object id, string token)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync("api/departamentos/" + id, x => SetToken(x, token)))
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

        public async Task Post(Departamento entity, string token)
        {
            using (var response = await _apiHelper.ApiClient.PostAsJsonAsync("api/departamentos", entity, x => SetToken(x, token)))
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

        public async Task Put(Departamento entity, string token)
        {
            using (var response = await _apiHelper.ApiClient.PutAsJsonAsync("api/departamentos/" + entity.Id, entity, x => SetToken(x, token)))
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

        private void SetToken(HttpRequestMessage r, string token)
        {
            r.Headers.Authorization = AuthenticationHeaderValue.Parse(token);
        }
    }
}
