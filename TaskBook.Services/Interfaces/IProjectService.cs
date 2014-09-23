using System;
using System.Linq;
using TaskBook.DataAccessLayer.AuthManagers;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.Services.Interfaces
{
    /// <summary>
    /// Defines the ProjectService operations
    /// </summary>
    public interface IProjectService: IDisposable
    {
        /// <summary>
        /// Accessor to the UserManager object
        /// </summary>
        TbUserManager UserManager { get; set; }

        /// <summary>
        /// Returns project data
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>Project data</returns>
        ProjectVm GetById(long id);

        /// <summary>
        /// Returns projects and project managers
        /// </summary>
        /// <returns>Projects and project managers</returns>
        IQueryable<ProjectManagerVm> GetProjectsAndManagers();

        /// <summary>
        /// Returns project data including project manager
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>project data including project manager</returns>
        ProjectManagerVm GetProjectsAndManagers(long projectId);

        /// <summary>
        /// Adds a project to the database
        /// </summary>
        /// <param name="projectVm">Project data</param>
        void AddProject(ProjectVm projectVm);

        /// <summary>
        /// Updates project data in the database
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <param name="projectVm">Input project data</param>
        void UpdateProject(long id, ProjectManagerVm projectVm);

        /// <summary>
        /// Removes project prom the database
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <param name="softDeleted">TRUE if the project should be only marked as deleted</param>
        /// <remarks>During project deletion all the dependent records (users in the project and their tasks) must be deleted as well</remarks>
        void DeleteProject(long id, bool softDeleted = false);
    }
}
