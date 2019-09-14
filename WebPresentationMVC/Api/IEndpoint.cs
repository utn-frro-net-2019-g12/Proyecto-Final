using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPresentationMVC.Api
{
    public interface IEndpoint<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Get(object id);
        Task Delete(object id);
        Task Post(TEntity materia);
        Task Put(TEntity materia);
    }
}
