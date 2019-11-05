using Presentation.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Library.Api.Endpoints.Interfaces {
    public interface IHorarioConsultaEndpoint : IEndpoint<HorarioConsulta> {
        Task<IEnumerable<HorarioConsulta>> GetByPartialDescAndDepto(string partialDesc, int? deptoId, string token);
        Task<IEnumerable<HorarioConsulta>> GetByCurrentUserProfessor(string token);
    }
}
