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
    public class ProjectRepository: Repository<Project>, IProjectRepository
    {
        public ProjectRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public async Task AddAsync(Project entity)
        {
            await Task.Run(() => Add(entity));
        }
    }
}
