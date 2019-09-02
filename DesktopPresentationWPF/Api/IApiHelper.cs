using DesktopPresentationWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPresentationWPF.Api
{
    public interface IApiHelper
    { 
        HttpClient ApiClient { get;}
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
        Task<ApiErrorsException> CreateApiErrorsException(HttpResponseMessage response);
    }
}
