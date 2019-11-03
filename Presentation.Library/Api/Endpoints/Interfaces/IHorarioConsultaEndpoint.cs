using Presentation.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Library.Api.Endpoints.Interfaces {
    public interface IHorarioConsultaEndpoint : IEndpoint<HorarioConsulta> {
        Task<IEnumerable<HorarioConsulta>> GetByPartialDesc(string partialDesc, string token);
        Task<IEnumerable<HorarioConsulta>> GetByCurrentUserProfessor(string token);
        Task<IEnumerable<HorarioConsulta>> GetByDeptoSorted(int deptoId ,string token);
    }
}
