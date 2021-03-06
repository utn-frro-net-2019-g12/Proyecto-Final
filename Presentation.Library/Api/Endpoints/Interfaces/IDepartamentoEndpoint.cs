﻿using Presentation.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Library.Api.Endpoints.Interfaces {
    public interface IDepartamentoEndpoint : IEndpoint<Departamento> {
        Task<IEnumerable<Departamento>> GetByPartialDesc(string partialDesc, string token);
    }
}
