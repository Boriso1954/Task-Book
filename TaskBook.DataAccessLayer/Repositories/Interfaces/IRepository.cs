using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.Repositories.Interfaces
{
    public interface IRepository<T> where T: Entity
    {
        object Add(T t);
        Task<object> AddAsync(T t);
        object Delete(T entity);
        void DeleteByPredicate(Func<T, bool> predicate);
        void Update(T t);
        T GetById(object id);
    }
}
