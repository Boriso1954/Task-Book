using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.Repositories
{
    /// <summary>
    /// Provides data repository for TaskBook project
    /// </summary>
    public sealed class ProjectRepository: Repository<Project>, IProjectRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="database">Database context</param>
        public ProjectRepository(TaskBookDbContext database)
            : base(database)
        {
        }
    }
}
