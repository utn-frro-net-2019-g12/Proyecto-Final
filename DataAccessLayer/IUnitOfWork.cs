﻿using System;
using DataAccessLayer.Repositories;

namespace DataAccessLayer
{
    public interface IUnitOfWork : IDisposable
    {
        IMateriaRepository Materias { get; }
        IDepartamentoRepository Departamentos { get; }
        int Complete();
    }
}
