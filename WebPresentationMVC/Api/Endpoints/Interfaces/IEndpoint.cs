using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebPresentationMVC.Api.Endpoints.Interfaces
{
    public interface IEndpoint<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Get(object id);
        Task Delete(object id);
        Task Post(TEntity entity);
        Task Put(TEntity entity);
    }
}
