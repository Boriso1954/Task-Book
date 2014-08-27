using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.DataAccessLayer.Reader
{
    public interface IReaderRepository: IDisposable
    {
        IQueryable<ProjectManagerVm> GetProjectsAndManagers(long? projectId = null);
        IQueryable<TbUserRoleVm> GetUserByUserName(string userName);
        IQueryable<PermissionVm> GetPermissionsByRole(string roleName);
        IQueryable<ProjectManagerVm> GetProjectByManagerName(string managerName);
        IQueryable<TaskVm> GetTasks(long? projectId = null);
        IQueryable<TaskVm> GetTask(long id);
        IQueryable<UserProjectVm> GetUsersByProjectId(long projectId);
        IQueryable<TbUserRoleVm> GetUsersWithRolesByProjectId(long projectId);
        IQueryable<TaskUserVm> GetUserTasksByUserName(string userName);
        IQueryable<TbUserVm> GetDeletedUsers();
    }
}
