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
    public abstract class Repository<T>: IDisposable, IRepository<T> where T: Entity
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<T> _dbset;

        protected Repository(IUnitOfWork unitOfWork)
        {
            if(unitOfWork == null)
            {
                throw new ArgumentNullException("UnitOfWork");
            }
            _unitOfWork = unitOfWork;
            _dbset = unitOfWork.Database.Set<T>();
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

        public virtual void Update(T entity)
        {
             _dbset.Attach(entity);
            _unitOfWork.Database.Entry(entity).State = EntityState.Modified;
        }

        public virtual object Delete(T entity)
        {
            if(_unitOfWork.Database.Entry(entity).State == EntityState.Detached)
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

        public void SaveChanges()
        {
            _unitOfWork.Database.SaveChanges();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
