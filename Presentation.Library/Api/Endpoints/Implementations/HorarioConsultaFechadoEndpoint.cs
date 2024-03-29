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
    public class HorarioConsultaFechadoEndpoint : IHorarioConsultaFechadoEndpoint
    {
        private IApiHelper _apiHelper;
        
        public HorarioConsultaFechadoEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IEnumerable<HorarioConsultaFechado>> GetAll(string token)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/horariosConsultaFechados", x => x.SetAuthHeaders(token)))
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

                var result = await response.Content.ReadAsAsync<IEnumerable<HorarioConsultaFechado>>();

                return result;
            }
        }

        public async Task<IEnumerable<HorarioConsultaFechado>> GetByDeptoAndMateriaAndProfe(int? deptoId, int? materiaId, int? profeId, string token) {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"api/horariosConsultaFechados/search/?deptoId={deptoId}&materiaId={materiaId}&profeId={profeId}",
                x => x.SetAuthHeaders(token))) {

                if (!response.IsSuccessStatusCode) {
                    switch (response.StatusCode) {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedRequestException(response);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }

                var result = await response.Content.ReadAsAsync<IEnumerable<HorarioConsultaFechado>>();

                return result;
            }
        }

        public async Task<HorarioConsultaFechado> Get(object id, string token)
        {
            using (var response = await _apiHelper.ApiClient.GetAsync("api/horariosConsultaFechados/" + id, x => x.SetAuthHeaders(token)))
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

                var result = await response.Content.ReadAsAsync<HorarioConsultaFechado>();

                return result;
            }
        }

        public async Task Delete(object id, string token)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync("api/horariosConsultaFechados/" + id, x => x.SetAuthHeaders(token)))
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

        public async Task Post(HorarioConsultaFechado entity, string token)
        {
            using (var response = await _apiHelper.ApiClient.PostAsJsonAsync("api/horariosConsultaFechado", entity, x => x.SetAuthHeaders(token)))
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

        public async Task Put(HorarioConsultaFechado entity, string token)
        {
            using (var response = await _apiHelper.ApiClient.PutAsJsonAsync("api/horariosConsultaFechados/" + entity.Id, entity, x => x.SetAuthHeaders(token)))
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
