using System.Data.Entity;

namespace DataAccessLayer
{
    public class ConsultaUTNContext : DbContext
    {
        public virtual DbSet<Materia> Materias { get; set; }
        public virtual DbSet<Departamento> Departamentos { get; set; }

        public ConsultaUTNContext() : base("name=ConsultaUTNContext")
        {
            Database.SetInitializer<ConsultaUTNContext>(new ConsultaUTNInitializer());
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }
    }
}
