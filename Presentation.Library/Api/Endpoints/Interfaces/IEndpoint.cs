using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Library.Api.Endpoints.Interfaces
{
    public interface IEndpoint<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll(string token);
        Task<TEntity> Get(object id, string token);
        Task Delete(object id, string token);
        Task Post(TEntity entity, string token);
        Task Put(TEntity entity, string token);
    }
}
