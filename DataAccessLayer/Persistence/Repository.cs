using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using DataAccessLayer.Repositories;

namespace DataAccessLayer.Persistence
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        private DbSet<TEntity> _entities;

        public Repository(DbContext _context)
        {
            Context = _context;
            _entities = Context.Set<TEntity>();
        }


        public TEntity GetById(object id)
        {
            return _entities.Find(id);
        }

        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filterExp = null
            )
        {
            IQueryable<TEntity> query = _entities;

            if (filterExp != null)
            {
                query = query.Where(filterExp);
            }

            return query.ToList();
        }

        public IEnumerable<TEntity> GetOrdered<TKey>(
            Expression<Func<TEntity, TKey>> orderByExp,
            Expression<Func<TEntity, bool>> filterExp = null
            )
        {
            Context.Database.Log = message => Trace.Write(message);
            IQueryable<TEntity> query = _entities;

            if (filterExp != null)
            {
                query = query.Where(filterExp);
            }

            return query.OrderBy(orderByExp).ToList();
        }

        public void Insert(TEntity entity)
        {
            _entities.Add(entity);
        }

        public void InsertRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = _entities.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                _entities.Attach(entity);
            }

            _entities.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                if (Context.Entry(entity).State == EntityState.Detached) // REV This may not be necessary
                {
                    _entities.Attach(entity);
                }
            }
            _entities.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            _entities.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }
    }
}
