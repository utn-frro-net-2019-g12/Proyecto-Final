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
    public class InscripcionEndpoint : IInscripcionEndpoint
    {
        private IApiHelper _apiHelper;

        public InscripcionEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IEnumerable<Inscripcion>> GetAll(string token)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/inscripciones", x => x.SetAuthHeaders(token)))
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

                var result = await response.Content.ReadAsAsync<IEnumerable<Inscripcion>>();

                return result;
            }
        }

        public async Task<IEnumerable<Inscripcion>> GetByPartialDesc(string partialDesc, string token) {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"api/inscripciones/search/?desc={partialDesc}", x => x.SetAuthHeaders(token))) {
                if (!response.IsSuccessStatusCode) {
                    switch (response.StatusCode) {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedRequestException(response);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }

                var result = await response.Content.ReadAsAsync<IEnumerable<Inscripcion>>();

                return result;
            }
        }

        public async Task<IEnumerable<Inscripcion>> GetByCurrentProfesorUser(string token)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/inscripciones/profesores/current", x => x.SetAuthHeaders(token)))
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

                var result = await response.Content.ReadAsAsync<IEnumerable<Inscripcion>>();

                return result;
            }
        }

        public async Task<IEnumerable<Inscripcion>> GetByCurrentAlumnoUser(string token)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/inscripciones/activas/alumnos/current", x => x.SetAuthHeaders(token)))
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

                var result = await response.Content.ReadAsAsync<IEnumerable<Inscripcion>>();

                return result;
            }
        }

        public async Task<Inscripcion> Get(object id, string token)
        {
            using (var response = await _apiHelper.ApiClient.GetAsync("api/inscripciones/" + id, x => x.SetAuthHeaders(token)))
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

                var result = await response.Content.ReadAsAsync<Inscripcion>();

                return result;
            }
        }

        public async Task Delete(object id, string token)
        {
            using (HttpResponseMessage response= await _apiHelper.ApiClient.DeleteAsync("api/inscripciones/" + id, x => x.SetAuthHeaders(token)))
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

        public async Task Post(Inscripcion entity, string token)
        {
            using (var response = await _apiHelper.ApiClient.PostAsJsonAsync("api/inscripciones", entity, x => x.SetAuthHeaders(token)))
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

        public async Task Put(Inscripcion entity, string token)
        {
            using (var response = await _apiHelper.ApiClient.PutAsJsonAsync("api/inscripciones/" + entity.Id, entity, x => x.SetAuthHeaders(token)))
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
    }
}
