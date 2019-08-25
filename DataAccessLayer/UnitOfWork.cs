using DataAccessLayer.Repositories;
using DataAccessLayer.Persistence;
using System.Data.Entity.Infrastructure;
using System;

namespace DataAccessLayer {
    public class UnitOfWork : IUnitOfWork {
        private readonly ConsultaUTNContext _context;
        private bool _disposed = false;

        public IMateriaRepository Materias { get; private set; }
        public IDepartamentoRepository Departamentos { get; private set; }
        public IUsuarioRepository Usuarios { get; private set; }
        public IHorarioConsultaRepository HorariosConsulta { get; private set; }
        public IInscripcionRepository Inscripciones { get; private set; }

        public UnitOfWork(ConsultaUTNContext context) {
            _context = context;
            Materias = new MateriaRepository(_context);
            Departamentos = new DepartamentoRepository(_context);
            Usuarios = new UsuarioRepository(_context);
            HorariosConsulta = new HorarioConsultaRepository(_context);
            Inscripciones = new InscripcionRepository(_context);
        }

        // Add DBConcurrencyException here
        public int Complete() {
            try {
                return _context.SaveChanges();
            }
            // Its possible that this doesn't affect any rows(that why the exception) because of some concurrrency problem or the product doesn't exist in db actually
            catch (DbUpdateConcurrencyException) {
                throw;
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (!this._disposed) {
                if (disposing) {
                    _context.Dispose();
                }
            }

            this._disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
