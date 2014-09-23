using System;
using System.Linq;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.DataAccessLayer.Reader
{
    /// <summary>
    /// Defines the direct data read operations
    /// </summary>
    public interface IReaderRepository: IDisposable
    {
        /// <summary>
        /// Returns list of projects and project managers 
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns>List of projects and managers</returns>
        /// <remarks>If project ID is null returns all the projects</remarks>
        IQueryable<ProjectManagerVm> GetProjectsAndManagers(long? projectId = null);

        /// <summary>
        /// Returns user's deatils by user name
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>User's details including role</returns>
        IQueryable<TbUserRoleVm> GetUserByUserName(string userName);

        /// <summary>
        /// Returns role permissions
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>List of the role permissions</returns>
        IQueryable<PermissionVm> GetPermissionsByRole(string roleName);

        /// <summary>
        /// Returns list of projects and project managers
        /// </summary>
        /// <param name="managerName">Manager name</param>
        /// <returns>List of projects and managers</returns>
        IQueryable<ProjectManagerVm> GetProjectByManagerName(string managerName);

         /// <summary>
        /// Returns list of tasks
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>If project ID is null returns all the tasks</returns>
        IQueryable<TaskVm> GetTasks(long? projectId = null);

        /// <summary>
        /// Returns task details
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>List of tasks</returns>
        IQueryable<TaskVm> GetTask(long id);

        /// <summary>
        /// Returns list of users related to the specified project
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>List of users</returns>
        IQueryable<UserProjectVm> GetUsersByProjectId(long projectId);

        /// <summary>
        /// Returns list of users including role related to specified project
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>List of users with roles</returns>
        IQueryable<TbUserRoleVm> GetUsersWithRolesByProjectId(long projectId);

         /// <summary>
        /// Returns list of user's tasks
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>List of user's tasks</returns>
        /// <remarks>In comparison with GetTasksByUserName this method returns less data</remarks>
        IQueryable<TaskUserVm> GetUserTasks(string userName);

        /// <summary>
        /// Returns list of soft deleted users (marked as deleted)
        /// </summary>
        /// <returns>List of soft deleted users</returns>
        IQueryable<TbUserVm> GetDeletedUsers();

         /// <summary>
        /// Returns list of soft deleted projects (marked as deleted)
        /// </summary>
        /// <returns>List of soft deleted projects</returns>
        IQueryable<ProjectVm> GetDeletedProjects();

        /// <summary>
        /// Returns list of user's tasks 
        /// </summary>
        /// <param name="userName">User nam</param>
        /// <returns>List of user's tasks</returns>
        IQueryable<TaskVm> GetTasksByUserName(string userName);
    }
}
