using System.Collections.Generic;

namespace DataAccessLayer
{
    public class ConsultaUTNInitializer : System.Data.Entity.DropCreateDatabaseAlways<ConsultaUTNContext>
    {
        protected override void Seed(ConsultaUTNContext context)
        {
            var unitOfWork = new UnitOfWork(new ConsultaUTNContext());

            var materias = new List<Materia>
            {
                new Materia{ Name = "Algoritmos Geneticos", Year = 3, IsElectiva = true},
                new Materia{ Name = "Diseño de Sistemas", Year = 3, IsElectiva = false},
                new Materia{ Name = "Analisis Matematico 2", Year = 2, IsElectiva = false}

            };

            unitOfWork.Materias.AddRange(materias);

            unitOfWork.Complete();
        }
    }
}
