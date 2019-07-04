using DataAccessLayer.Repositories;
using DataAccessLayer.Persistence;
using System.Data.Entity.Infrastructure;

namespace DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ConsultaUTNContext _context;

        public IMateriaRepository Materias { get; private set; }

        public UnitOfWork(ConsultaUTNContext context)
        {
            _context = context;
            Materias = new MateriasRepository(_context);
        }

        // Add DBConcurrencyException here
        public int Complete()
        {
            try
            {
                return _context.SaveChanges();
            }
            // Its possible that this doesn't affect any rows(that why the exception) because of some concurrrency problem or the product doesn't exist in db actually
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
