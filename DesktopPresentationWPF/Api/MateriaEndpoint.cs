using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopPresentationWPF.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ComponentModel;

namespace DesktopPresentationWPF.Api
{
    public class MateriaEndpoint : IMateriaEndpoint
    {
        private IApiHelper _apiHelper;

        public MateriaEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<BindingList<WpfMateriaModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/materias/departamento"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<BindingList<WpfMateriaModel>>();

                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
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

        public async Task Post(WpfMateriaModel materia)
        {
            using (var response = await _apiHelper.ApiClient.PostAsJsonAsync("api/materias", materia))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var ex = await _apiHelper.CreateApiErrorsException(response);

                    throw ex; 
                }
            }
        }

        public async Task Put(WpfMateriaModel materia)
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
