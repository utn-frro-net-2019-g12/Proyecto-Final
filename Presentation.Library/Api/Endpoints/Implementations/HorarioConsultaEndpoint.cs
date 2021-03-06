﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Presentation.Library.Api.Endpoints.Interfaces;
using Presentation.Library.Api.Exceptions;
using Presentation.Library.Models;

namespace Presentation.Library.Api.Endpoints.Implementations
{
    public class HorarioConsultaEndpoint : IHorarioConsultaEndpoint
    {
        private IApiHelper _apiHelper;

        public HorarioConsultaEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IEnumerable<HorarioConsulta>> GetAll(string token)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/horariosConsulta", x => x.SetAuthHeaders(token)))
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

                var result = await response.Content.ReadAsAsync<IEnumerable<HorarioConsulta>>();

                return result;
            }
        }

        public async Task<IEnumerable<HorarioConsulta>> GetByPartialDescAndDepto(string partialDesc, int? deptoId, string token) {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"api/horariosConsulta/search/?desc={partialDesc}&&deptoId={deptoId}", x => x.SetAuthHeaders(token))) {
                if (!response.IsSuccessStatusCode) {
                    switch (response.StatusCode) {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedRequestException(response);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }

                var result = await response.Content.ReadAsAsync<IEnumerable<HorarioConsulta>>();

                return result;
            }
        }

        public async Task<IEnumerable<HorarioConsulta>> GetByCurrentUserProfessor(string token)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/horariosConsulta/profesores/current", x => x.SetAuthHeaders(token)))
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

                var result = await response.Content.ReadAsAsync<IEnumerable<HorarioConsulta>>();

                return result;
            }
        }

        public async Task<HorarioConsulta> Get(object id, string token)
        {
            using (var response = await _apiHelper.ApiClient.GetAsync("api/horariosConsulta/" + id, x => x.SetAuthHeaders(token)))
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

                var result = await response.Content.ReadAsAsync<HorarioConsulta>();

                return result;
            }
        }

        public async Task Delete(object id, string token)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync("api/horariosConsulta/" + id, x => x.SetAuthHeaders(token)))
            {
                if (!response.IsSuccessStatusCode)
                {
                    // BadRequest might be received as well
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

        public async Task Post(HorarioConsulta entity, string token)
        {
            using (var response = await _apiHelper.ApiClient.PostAsJsonAsync("api/horariosConsulta", entity, x => x.SetAuthHeaders(token)))
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

        public async Task Put(HorarioConsulta entity, string token)
        {
            using (var response = await _apiHelper.ApiClient.PutAsJsonAsync("api/horariosConsulta/" + entity.Id, entity, x => x.SetAuthHeaders(token)))
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
