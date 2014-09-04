using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DomainModel;
using TaskBook.DomainModel.ViewModels;
using TaskBook.Services.AuthManagers;

namespace TaskBook.Services.Interfaces
{
    public interface IProjectService: IDisposable
    {
        TbUserManager UserManager { get; set; }
        ProjectVm GetById(long id);
        IQueryable<ProjectManagerVm> GetProjectsAndManagers();
        ProjectManagerVm GetProjectsAndManagers(long projectId);
        void AddProject(ProjectVm projectVm);
        void UpdateProject(long id, ProjectManagerVm projectVm);
        void DeleteProject(long id, bool softDeleted = false);
    }
}
