using Presentation.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Library.Api.Endpoints.Interfaces
{
    public interface IAuthenticationEndpoint
    {
        Task<Token> GetToken(LoginModel model);
        Task RegisterAccount(RegisterModel model, string token);
        Task<IEnumerable<string>> GetUserRoles(string token);
    }
}
