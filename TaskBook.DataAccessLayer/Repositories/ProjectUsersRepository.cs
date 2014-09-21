using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.Repositories
{
    /// <summary>
    /// Provides data repository for ProjectUsers entity
    /// </summary>
    public class ProjectUsersRepository: Repository<ProjectUsers>, IProjectUsersRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="database">Database context</param>
        public ProjectUsersRepository(TaskBookDbContext database)
            : base(database)
        {
        }
    }
}
