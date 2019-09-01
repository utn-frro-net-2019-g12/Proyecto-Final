using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DesktopPresentationWPF.Api;
using DesktopPresentationWPF.Models;

namespace DesktopPresentationWPF.Api
{
    class DepartamentoEndpoint : IDepartamentoEndpoint
    {
        private IApiHelper _apiHelper;

        public DepartamentoEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<BindingList<WpfDepartamentoModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/departamentos"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<BindingList<WpfDepartamentoModel>>();

                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
