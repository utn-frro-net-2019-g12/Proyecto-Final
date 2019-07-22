using System.Collections.Generic;

namespace DataAccessLayer
{
    public class ConsultaUTNInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ConsultaUTNContext>
    {
        protected override void Seed(ConsultaUTNContext context)
        {
            var unitOfWork = new UnitOfWork(new ConsultaUTNContext());

            var departamentos = new List<Departamento> {
                new Departamento{ Name = "Ingeniería en Sistemas de Información" },
                new Departamento{ Name = "Ingeniería Mecánica" },
                new Departamento{ Name = "Ingeniería Química" },
                new Departamento{ Name = "Ingeniería Civil" },
                new Departamento{ Name = "Ingeniería Eléctrica" },
                new Departamento{ Name = "Ciencias Básicas" },
            };

            unitOfWork.Departamentos.AddRange(departamentos);
            unitOfWork.Complete();

            var materias = new List<Materia>
            {
                new Materia{ Name = "Algoritmos Genéticos", Year = 3, IsElectiva = true, DepartamentoId = departamentos[0].Id, Departamento=departamentos[0] },
                new Materia{ Name = "Diseño de Sistemas", Year = 3, IsElectiva = false, DepartamentoId = departamentos[0].Id, Departamento=departamentos[0] },
                new Materia{ Name = "Análisis Matemático 2", Year = 2, IsElectiva = false, DepartamentoId = departamentos[5].Id, Departamento=departamentos[5] }
                // Agregar los departamentos que pertenecen cada Materia
            };

            unitOfWork.Materias.AddRange(materias);
            unitOfWork.Complete();
        }
    }
}
