using System;
using TaskBook.DataAccessLayer.Reader;
using TaskBook.DataAccessLayer.Repositories.Interfaces;

namespace TaskBook.DataAccessLayer
{
    /// <summary>
    /// Defines the UnitOfWork operations
    /// </summary>
    public interface IUnitOfWork: IDisposable
    {
        /// <summary>
        /// Returns the reference to the ProjectRepository object
        /// </summary>
        IProjectRepository ProjectRepository { get; }

        /// <summary>
        /// Returns the reference to the ProjectUsersRepository object
        /// </summary>
        IProjectUsersRepository ProjectUsersRepository { get; }

        /// <summary>
        /// Returns the reference to the TaskRepository object
        /// </summary>
        ITaskRepository TaskRepository { get; }

        /// <summary>
        /// Returns the reference to the ReaderRepository object
        /// </summary>
        IReaderRepository ReaderRepository { get; }

        /// <summary>
        /// Returns the reference to the database context object
        /// </summary>
        TaskBookDbContext DbContext { get; }

        /// <summary>
        /// Commits the unit of work
        /// </summary>
        void Commit();
    }
}
