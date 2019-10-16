using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ComponentModel;
using System.Net;
using Presentation.Library.Api.Exceptions;
using Presentation.Library.Api.Endpoints.Interfaces;
using Presentation.Library.Models;

namespace Presentation.Library.Api.Endpoints.Implementations
{
    public class MateriaEndpoint : IMateriaEndpoint
    {
        private IApiHelper _apiHelper;

        public MateriaEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IEnumerable<Materia>> GetAll(string token)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/materias/departamentos", x => SetToken(x, token)))
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

                var result = await response.Content.ReadAsAsync<IEnumerable<Materia>>();

                return result;
            }
        }

        public async Task<IEnumerable<Materia>> GetByDepto(int id_departamento, string token)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"api/materias/departamentos/{id_departamento}", x => SetToken(x, token)))
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

                var result = await response.Content.ReadAsAsync<IEnumerable<Materia>>();

                return result;
            }
        }

        public async Task<Materia> Get(object id, string token)
        {
            using (var response = await _apiHelper.ApiClient.GetAsync($"api/materias/{id}", x => SetToken(x, token)))
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

                var result = await response.Content.ReadAsAsync<Materia>();

                return result;
            }
        }

        public async Task Delete(object id, string token)
        {
            using (HttpResponseMessage response= await _apiHelper.ApiClient.DeleteAsync($"api/materias/{id}", x => SetToken(x, token)))
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
                            throw new NotFoundRequestException(response, id);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }
            }
        }

        public async Task Post(Materia entity, string token)
        {
            using (var response = await _apiHelper.ApiClient.PostAsJsonAsync("api/materias", entity, x => SetToken(x, token)))
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

        public async Task Put(Materia entity, string token)
        {
            using (var response = await _apiHelper.ApiClient.PutAsJsonAsync($"api/materias/{entity.Id}", entity, x => SetToken(x, token)))
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
