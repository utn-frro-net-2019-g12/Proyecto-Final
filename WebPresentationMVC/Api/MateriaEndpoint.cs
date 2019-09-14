using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPresentationMVC.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ComponentModel;

namespace WebPresentationMVC.Api
{
    public class MateriaEndpoint : IMateriaEndpoint
    {
        private IApiHelper _apiHelper;
        private readonly IUserSession _userSession;

        public MateriaEndpoint(IApiHelper apiHelper, IUserSession userSession)
        {
            _apiHelper = apiHelper;
            _userSession = userSession;

            // This happens per request
            _apiHelper.AddTokenToHeaders(userSession.BearerToken);
        }

        public async Task<IEnumerable<MvcMateriaModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/materias/departamento"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<IEnumerable<MvcMateriaModel>>();

                    return result;
                }
                else
                {
                    ApiErrorsException ex = await _apiHelper.CreateApiErrorsException(response);

                    throw ex;
                }
            }
        }

        public async Task<MvcMateriaModel> Get(object id)
        {
            using(var response = await _apiHelper.ApiClient.GetAsync("api/materias/" + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<MvcMateriaModel>();

                    return result;
                }
                else
                {
                    ApiErrorsException ex = await _apiHelper.CreateApiErrorsException(response);

                    throw ex;
                }
            }
        }

        public async Task Delete(object id)
        {
            using (HttpResponseMessage response= await _apiHelper.ApiClient.DeleteAsync("api/materias/" + id))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var ex = await _apiHelper.CreateApiErrorsException(response);

                    throw ex;
                }
            }
        }

        public async Task Post(MvcMateriaModel materia)
        {
            using (var response = await _apiHelper.ApiClient.PostAsJsonAsync("api/materias", materia))
            {
                if (!response.IsSuccessStatusCode)
                {
                    ApiErrorsException ex = await _apiHelper.CreateApiErrorsException(response);

                    throw ex; 
                }
            }
        }

        public async Task Put(MvcMateriaModel materia)
        {
            using(var response = await _apiHelper.ApiClient.PutAsJsonAsync("api/materias/" + materia.Id, materia))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var ex = await _apiHelper.CreateApiErrorsException(response);

                    throw ex;
                }
            }
        }
    }
}
