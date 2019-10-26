using Presentation.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Library.Api.Endpoints.Interfaces {
    public interface IMateriaEndpoint : IEndpoint<Materia> {
        Task<IEnumerable<Materia>> GetByPartialDesc(string partialDesc, string token);
        Task<IEnumerable<Materia>> GetByDepto(int id_departamento, string token);
    }
}
