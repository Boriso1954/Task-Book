using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.Repositories
{
    /// <summary>
    /// Provides data repository for Task entity
    /// </summary>
    public sealed class TaskRepository: Repository<TbTask>, ITaskRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="database"></param>
        public TaskRepository(TaskBookDbContext database)
            : base(database)
        {
        }
    }
}
