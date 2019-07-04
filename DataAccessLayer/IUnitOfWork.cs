using System;
using DataAccessLayer.Repositories;

namespace DataAccessLayer
{
    public interface IUnitOfWork : IDisposable
    {
        IMateriaRepository Materias { get; }
        int Complete();
    }
}
