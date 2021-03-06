﻿using Presentation.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Library.Api.Endpoints.Interfaces {
    public interface IInscripcionEndpoint : IEndpoint<Inscripcion> {
        Task<IEnumerable<Inscripcion>> GetByPartialDesc(string partialDesc, string token);
        Task<IEnumerable<Inscripcion>> GetByCurrentProfesorUser(string token);
        Task<IEnumerable<Inscripcion>> GetByCurrentAlumnoUser(string token);
    }
}
