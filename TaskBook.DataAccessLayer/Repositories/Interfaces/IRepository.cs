using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.Repositories.Interfaces
{
    /// <summary>
    /// Defines common repository operations
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public interface IRepository<T> where T: Entity
    {
        /// <summary>
        /// Adds an entity to the repository
        /// </summary>
        /// <param name="t">Entity object to be added</param>
        /// <returns>Entity ID</returns>
        object Add(T t);

        /// <summary>
        /// Adds an entity to the repository asynchronously
        /// </summary>
        /// <param name="t">Entity object to be added</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task<object> AddAsync(T t);

        /// <summary>
        /// Marks entity as deleted
        /// </summary>
        /// <param name="t">Entity object to be deleted</param>
        /// <returns>Entity ID</returns>
        object Delete(T t);

        /// <summary>
        /// Marks entities as deleted based on a predicate
        /// </summary>
        /// <param name="predicate">Method that defines a filter criterion</param>
        void DeleteByPredicate(Func<T, bool> predicate);

        /// <summary>
        /// Changes entity proprties and marks an entity as modified
        /// </summary>
        /// <param name="t">Entity object to be updated</param>
        void Update(T t);

        /// <summary>
        /// Get an entity from the repository by ID
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>Entity object</returns>
        T GetById(object id);

        /// <summary>
        /// Returns the entity collection from the repository according to the specified predicate
        /// </summary>
        /// <param name="predicate">Method that defines a filter criterion</param>
        /// <returns>Entities collection met the predicate</returns>
        IEnumerable<T> Get(Func<T, bool> predicate);

        /// <summary>
        /// Checks if an entity exists in the repository
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>TRUE if the entity exists</returns>
        bool Exists(object id);

        /// <summary>
        /// Returns all the entities
        /// </summary>
        /// <returns>Entity collection</returns>
        IEnumerable<T> GetAll();
    }
}
