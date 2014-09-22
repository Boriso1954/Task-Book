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
    /// <summary>
    /// Abstract generic repository for data access; provides main CRUD operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Repository<T>: IRepository<T> where T: Entity
    {
        private readonly TaskBookDbContext _db;
        private readonly IDbSet<T> _dbset;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="database">Database context</param>
        protected Repository(TaskBookDbContext database)
        {

            if(database == null)
            {
                throw new ArgumentNullException("Database");
            }
            _db = database;
            _dbset = database.Set<T>();
        }

        /// <summary>
        /// Get an entity from data store by ID
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>Entity object to be returned</returns>
        public virtual T GetById(object id)
        {
            return _dbset.Find(id);
        }

        /// <summary>
        /// Returns the entity collection according to the specified predicate
        /// </summary>
        /// <param name="predicate">Method that defines a filter criterion</param>
        /// <returns>Entities collection met the predicate</returns>
        public virtual IEnumerable<T> Get(Func<T, bool> predicate)
        {
            return _dbset.Where(predicate);
        }


        /// <summary>
        /// Returns all the entities
        /// </summary>
        /// <returns>Entity collection</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return _dbset.AsEnumerable();
        }

        /// <summary>
        /// Checks if an entity exists
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>Entity object to be returned</returns>
        public bool Exists(object id)
        {
            return _dbset.Find(id) == null;
        }

        /// <summary>
        /// Adds an entity to the collection
        /// </summary>
        /// <param name="t">Entity object to be added</param>
        public virtual object Add(T t)
        {
            T obj = _dbset.Add(t);
            return obj.Id;
        }

        /// <summary>
        /// Adds an entity to the collection asynchronously
        /// </summary>
        /// <param name="t">Entity object to be added</param>
        public virtual Task<object> AddAsync(T t)
        {
            return Task.Run(() =>
            {
                var key = Add(t);
                return key;
            });
        }

        /// <summary>
        /// Marks an entity as modified
        /// </summary>
        /// <param name="t">Entity object to be updated</param>
        public virtual void Update(T t)
        {
             _dbset.Attach(t);
            _db.Entry(t).State = EntityState.Modified;
        }

        /// <summary>
        /// Marks entities as deleted
        /// </summary>
        /// <param name="t">Entity object to be deleted</param>
        public virtual object Delete(T t)
        {
            if(_db.Entry(t).State == EntityState.Detached)
            {
                _dbset.Attach(t);
            }
            T obj = _dbset.Remove(t);
            return obj.Id;
        }

        /// <summary>
        /// Marks entities as deleted based on a predicate
        /// </summary>
        /// <param name="predicate">Method that defines a filter criterion</param>
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
