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

        public override object Delete(Project project)
        {
            project.DeletedDate = DateTimeOffset.UtcNow;
            return project.Id;
        }
    }
}
