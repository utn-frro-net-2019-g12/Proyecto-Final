using System;
using DataAccessLayer.Repositories;

namespace DataAccessLayer {
    public interface IUnitOfWork : IDisposable {
        IMateriaRepository Materias { get; }
        IDepartamentoRepository Departamentos { get; }
        IUsuarioRepository Usuarios { get; }
        IHorarioConsultaRepository HorariosConsulta { get; }
        IInscripcionRepository Inscripciones { get; }
        int Complete();
    }
}
