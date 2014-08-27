using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.Repositories
{
    public abstract class Repository<T>: IRepository<T> where T: Entity
    {
        private readonly TaskBookDbContext _db;
        private readonly IDbSet<T> _dbset;

        protected Repository(TaskBookDbContext database)
        {

            if(database == null)
            {
                throw new ArgumentNullException("Database");
            }
            _db = database;
            _dbset = database.Set<T>();
        }

        public virtual T GetById(object id)
        {
            return _dbset.Find(id);
        }

        public virtual IEnumerable<T> Get(Func<T, bool> predicate)
        {
            return _dbset.Where(predicate);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbset.AsEnumerable();
        }

        public bool Exists(object id)
        {
            return _dbset.Find(id) == null;
        }

        public virtual object Add(T entity)
        {
            T obj = _dbset.Add(entity);
            return obj.Id;
        }

        public virtual Task<object> AddAsync(T t)
        {
            return Task.Run(() =>
            {
                var key = Add(t);
                return key;
            });
        }

        public virtual void Update(T entity)
        {
             _dbset.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
        }

        public virtual object Delete(T entity)
        {
            if(_db.Entry(entity).State == EntityState.Detached)
            {
                _dbset.Attach(entity);
            }
            T obj = _dbset.Remove(entity);
            return obj.Id;
        }

        public virtual void DeleteByPredicate(Func<T, bool> predicate)
        {
            IEnumerable<T> entities = _dbset.Where(predicate);
            foreach(T t in entities)
            {
                Delete(t);
            }
        }
    }
}
