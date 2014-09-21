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
    /// Provides data repository for Task entity
    /// </summary>
    public class TaskRepository: Repository<TbTask>, ITaskRepository
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
