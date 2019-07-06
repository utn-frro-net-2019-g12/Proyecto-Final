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
                new Materia{ Name = "Algoritmos Genéticos", Year = 3, IsElectiva = true },
                new Materia{ Name = "Diseño de Sistemas", Year = 3, IsElectiva = false },
                new Materia{ Name = "Análisis Matemático 2", Year = 2, IsElectiva = false }
                // Agregar los departamentos que pertenecen cada Materia
            };

            var departamentos = new List<Departamento> {
                new Departamento{ Name = "Ingeniería en Sistemas de Información" },
                new Departamento{ Name = "Ingeniería Mecánica" },
                new Departamento{ Name = "Ingeniería Química" },
                new Departamento{ Name = "Ingeniería Civil" },
                new Departamento{ Name = "Ingeniería Eléctrica" },
                new Departamento{ Name = "Ciencias Básicas" },
            };

            unitOfWork.Materias.AddRange(materias);
            unitOfWork.Departamentos.AddRange(departamentos);

            unitOfWork.Complete();
        }
    }
}
