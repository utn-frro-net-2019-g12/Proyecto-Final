using Presentation.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Library.Api.Endpoints.Interfaces {
    public interface IUsuarioEndpoint : IEndpoint<Usuario> {
        Task<IEnumerable<Usuario>> GetByPartialDesc(string partialDesc, string token);
        Task<IEnumerable<Usuario>> GetProfesoresByPartialDesc(string partialDesc, string token);
        Task<IEnumerable<Usuario>> GetAlumnosByPartialDesc(string partialDesc, string token);
        Task<Usuario> GetCurrentUsuario(string token);
    }
}
