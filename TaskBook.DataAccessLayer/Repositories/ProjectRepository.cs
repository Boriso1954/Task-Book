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
    /// Provides data repository for TaskBook project
    /// </summary>
    public class ProjectRepository: Repository<Project>, IProjectRepository
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
