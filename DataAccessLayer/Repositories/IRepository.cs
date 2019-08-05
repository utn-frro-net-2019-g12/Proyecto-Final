using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(object id);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filterExp = null);
        IEnumerable<TEntity> GetOrdered<TKey>(Expression<Func<TEntity, TKey>> orderByExp, Expression<Func<TEntity, bool>> filterExp = null);

        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);

        void Delete(object id);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
    }
}
