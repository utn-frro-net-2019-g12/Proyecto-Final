﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presentation.Library.Api.Exceptions;
using Presentation.Library.Api.Endpoints.Interfaces;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Presentation.Library.Models;

namespace Presentation.Library.Api.Endpoints.Implementations {
    public class UsuarioEndpoint : IUsuarioEndpoint {
        private IApiHelper _apiHelper;

        public UsuarioEndpoint(IApiHelper apiHelper) {
            _apiHelper = apiHelper;
        }

        public async Task<IEnumerable<Usuario>> GetAll(string token) {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/usuarios", x => x.SetAuthHeaders(token))) {
                if (!response.IsSuccessStatusCode) {
                    switch (response.StatusCode) {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedRequestException(response);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }

                var result = await response.Content.ReadAsAsync<IEnumerable<Usuario>>();

                return result;
            }
        }

        public async Task<IEnumerable<Usuario>> GetAllProfesores(string token) {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/usuarios/profesores", x => x.SetAuthHeaders(token))) {
                if (!response.IsSuccessStatusCode) {
                    switch (response.StatusCode) {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedRequestException(response);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }

                var result = await response.Content.ReadAsAsync<IEnumerable<Usuario>>();

                return result;
            }
        }

        public async Task<IEnumerable<Usuario>> GetAllAlumnos(string token) {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/usuarios/alumnos", x => x.SetAuthHeaders(token))) {
                if (!response.IsSuccessStatusCode) {
                    switch (response.StatusCode) {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedRequestException(response);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }

                var result = await response.Content.ReadAsAsync<IEnumerable<Usuario>>();

                return result;
            }
        }

        public async Task<IEnumerable<Usuario>> GetByPartialDesc(string partialDesc, string token) {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"api/usuarios/search/?desc={partialDesc}", x => x.SetAuthHeaders(token))) {
                if (!response.IsSuccessStatusCode) {
                    switch (response.StatusCode) {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedRequestException(response);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }

                var result = await response.Content.ReadAsAsync<IEnumerable<Usuario>>();

                return result;
            }
        }

        public async Task<IEnumerable<Usuario>> GetProfesoresByPartialDesc(string partialDesc, string token) {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"api/usuarios/profesores/search/?desc={partialDesc}", x => x.SetAuthHeaders(token))) {
                if (!response.IsSuccessStatusCode) {
                    switch (response.StatusCode) {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedRequestException(response);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }

                var result = await response.Content.ReadAsAsync<IEnumerable<Usuario>>();

                return result;
            }
        }

        public async Task<IEnumerable<Usuario>> GetAlumnosByPartialDesc(string partialDesc, string token) {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"api/usuarios/alumnos/search/?desc={partialDesc}", x => x.SetAuthHeaders(token))) {
                if (!response.IsSuccessStatusCode) {
                    switch (response.StatusCode) {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedRequestException(response);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }

                var result = await response.Content.ReadAsAsync<IEnumerable<Usuario>>();

                return result;
            }
        }

        public async Task<Usuario> Get(object id, string token) {
            using (var response = await _apiHelper.ApiClient.GetAsync($"api/usuarios/{id}", x => x.SetAuthHeaders(token))) {
                if (!response.IsSuccessStatusCode) {
                    switch (response.StatusCode) {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedRequestException(response);
                        case HttpStatusCode.NotFound:
                            throw new NotFoundRequestException(response, id);
                        default:
                            throw new Exception($"{response.ReasonPhrase}: Contacte a soporte para mas detalles");
                    }
                }

                var result = await response.Content.ReadAsAsync<Usuario>();

                return result;
            }
        }

        public async Task Delete(object id, string token) {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync($"api/usuarios/{id}", x => x.SetAuthHeaders(token))) {
                if (!response.IsSuccessStatusCode) {
                    switch (response.StatusCode) {
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

        public async Task Post(Usuario entity, string token) {
            using (var response = await _apiHelper.ApiClient.PostAsJsonAsync("api/usuarios", entity, x => x.SetAuthHeaders(token))) {
                if (!response.IsSuccessStatusCode) {
                    switch (response.StatusCode) {
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

        public async Task Put(Usuario entity, string token) {
            using (var response = await _apiHelper.ApiClient.PutAsJsonAsync($"api/usuarios/{entity.Id}", entity, x => x.SetAuthHeaders(token))) {
                if (!response.IsSuccessStatusCode) {
                    switch (response.StatusCode) {
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


        public async Task UpdateCurrent(Usuario current, string token)
        {
            using (var response = await _apiHelper.ApiClient.PutAsJsonAsync("api/usuarios/current", current, x => x.SetAuthHeaders(token)))
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

        public async Task<Usuario> GetCurrentUsuario(string token)
        {
            using (var response = await _apiHelper.ApiClient.GetAsync("api/usuarios/current", x => x.SetAuthHeaders(token)))
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

                var result = await response.Content.ReadAsAsync<Usuario>();

                return result;
            }
        }
    }
}