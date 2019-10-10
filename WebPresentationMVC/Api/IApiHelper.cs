using WebPresentationMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebPresentationMVC.Api
{
    public interface IApiHelper
    { 
        HttpClient ApiClient { get;}
        Task<BadRequestException> CreateBadRequestException(HttpResponseMessage response);
    }
}
