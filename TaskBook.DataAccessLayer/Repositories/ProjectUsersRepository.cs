using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.Repositories
{
    public class ProjectUsersRepository: Repository<ProjectUsers>, IProjectUsersRepository
    {
        public ProjectUsersRepository(TaskBookDbContext database)
            : base(database)
        {
        }
    }
}
