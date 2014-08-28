using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DomainModel;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.Services.Interfaces
{
    public interface IProjectService: IDisposable
    {
        ProjectVm GetById(long id);
        IQueryable<ProjectManagerVm> GetProjectsAndManagers();
        ProjectManagerVm GetProjectsAndManagers(long projectId);
        void AddProject(ProjectVm projectVm);
        void UpdateProject(long id, ProjectManagerVm projectVm);
        void DeleteProject(long id, bool softDeleted = false);
    }
}
