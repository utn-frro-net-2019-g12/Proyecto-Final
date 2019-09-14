using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebPresentationMVC.Api;
using WebPresentationMVC.Models;

namespace WebPresentationMVC.Api
{
    class DepartamentoEndpoint : IDepartamentoEndpoint
    {
        private IApiHelper _apiHelper;

        public DepartamentoEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IEnumerable<MvcDepartamentoModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/departamentos"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<IEnumerable<MvcDepartamentoModel>>();

                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public Task<MvcDepartamentoModel> Get(object id)
        {
            throw new NotImplementedException();
        }

        public Task Post(MvcDepartamentoModel materia)
        {
            throw new NotImplementedException();
        }

        public Task Put(MvcDepartamentoModel materia)
        {
            throw new NotImplementedException();
        }

        public Task Delete(object id)
        {
            throw new NotImplementedException();
        }
    }
}
