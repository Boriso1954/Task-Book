using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using TaskBook.DataAccessLayer;
using TaskBook.DataAccessLayer.AuthManagers;
using TaskBook.DomainModel;
using TaskBook.DomainModel.ViewModels;
using TaskBook.Services.Interfaces;

namespace TaskBook.Services
{
    /// <summary>
    /// Service to manage TaskBook projects in the database
    /// </summary>
    public sealed class ProjectService: IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor; uses by the Unity dependency injector
        /// </summary>
        /// <param name="unitOfWork">Represents UnitOfWork object</param>
        [InjectionConstructor]
        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork">Represents UnitOfWork object</param>
        /// <param name="userManager">Represents Identity UserManager object</param>
        public ProjectService(IUnitOfWork unitOfWork,
            TbUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            UserManager = userManager;
        }

        /// <summary>
        /// Accessor to the UserManager object
        /// </summary>
        public TbUserManager UserManager { get; set; }

        /// <summary>
        /// Returns projects and project managers
        /// </summary>
        /// <returns>Projects and project managers</returns>
        public IQueryable<ProjectManagerVm> GetProjectsAndManagers()
        {
            var readerRepository = _unitOfWork.ReaderRepository;
            var projectsAndManagers = readerRepository.GetProjectsAndManagers();
            return projectsAndManagers;
        }

        /// <summary>
        /// Returns project data including project manager
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>project data including project manager</returns>
        public ProjectManagerVm GetProjectsAndManagers(long projectId)
        {
            var readerRepository = _unitOfWork.ReaderRepository;
            var projectAndManager = readerRepository.GetProjectsAndManagers(projectId).FirstOrDefault();
            return projectAndManager;
        }

        /// <summary>
        /// Returns project data
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>Project data</returns>
        public ProjectVm GetById(long id)
        {
            var projectRepository = _unitOfWork.ProjectRepository;
            var project = projectRepository.GetById(id);
            if(project == null)
            {
                throw new Exception(string.Format("Unable to return project with ID'{0}'", id));
            }

            var projectVm = new ProjectVm()
            {
                ProjectId = id,
                Title = project.Title
            };
            return projectVm;
        }

        /// <summary>
        /// Adds a project to the database
        /// </summary>
        /// <param name="projectVm">Project data</param>
        public void AddProject(ProjectVm projectVm)
        {
            var project = new Project()
            {
                Title = projectVm.Title,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            // "NotAssigned" system account uses as a manager for the new project
            var notAssignedUserId = UserManager.FindByName("NotAssigned").Id;
            var projectRepository = _unitOfWork.ProjectRepository;
            var projectAccessService = new ProjectAccessService(_unitOfWork);

            // Envelop the sequence of the db operations in the transaction scope
            using(var transaction = TransactionProvider.GetTransactionScope())
            {
                projectRepository.Add(project);
                projectAccessService.AddUserToProject(project.Id, notAssignedUserId);
                _unitOfWork.Commit();
                transaction.Complete();
            }
        }

        /// <summary>
        /// Updates project data in the database
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <param name="projectVm">Input project data</param>
        public void UpdateProject(long id, ProjectManagerVm projectVm)
        {
            if(id != projectVm.ProjectId)
            {
                throw new Exception("Project ID conflict.");
            }

            var projectRepository = _unitOfWork.ProjectRepository;
            var toBeUpdated = projectRepository.GetById(id);

            if(toBeUpdated == null)
            {
                throw new Exception(string.Format("Unable to find project '{0}'.", projectVm.ProjectTitle));
            }

            toBeUpdated.Title = projectVm.ProjectTitle;
            projectRepository.Update(toBeUpdated);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Removes project prom the database
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <param name="softDeleted">TRUE if the project should be only marked as deleted</param>
        /// <remarks>During project deletion all the dependent records (users in the project and their tasks) must be deleted as well</remarks>
        public void DeleteProject(long id, bool softDeleted = false)
        {
            var projectRepository = _unitOfWork.ProjectRepository;
            var project = projectRepository.GetById(id);
            if(project == null)
            {
                throw new Exception(string.Format("Unable to find project to be deleted. Project ID {0}", id));
            }

            // Get all project users and delete them
            IList<UserProjectVm> users;
            using(var readerRepository = _unitOfWork.ReaderRepository)
            {
                users = readerRepository.GetUsersByProjectId(id).ToList();
            }

            if(users.Any())
            {
                // To delete a user only this simple constructor is needed
                var userService = new UserService(_unitOfWork, UserManager);
                foreach(var u in users)
                {
                    // Delete user's tasks and mark a user as deleted
                    userService.DeleteUser(u.UserId, true);
                }
            }

            // Mark the project as deleted
            project.DeletedDate = DateTimeOffset.UtcNow;
            _unitOfWork.Commit();

            if(!softDeleted)
            {
                // Delete users which marked as deleted
                IList<TbUserVm> deletedUsers;
                using(var readerRepository = _unitOfWork.ReaderRepository)
                {
                    deletedUsers = readerRepository.GetDeletedUsers().ToList();
                }
               
                foreach(var u in deletedUsers)
                {
                    var deletedUser = UserManager.FindById(u.UserId);

                    // Delete user (cascade deletion from UserRoles and ProgectUsers)
                    UserManager.Delete(deletedUser);
                }

                // Delete projects which marked as deleted
                IList<ProjectVm> deletedProjects;
                using(var readerRepository = _unitOfWork.ReaderRepository)
                {
                    deletedProjects = readerRepository.GetDeletedProjects().ToList();
                }

                foreach(var p in deletedProjects)
                {
                    var deletedProject = projectRepository.GetById(p.ProjectId);

                    // Delete project
                    projectRepository.Delete(deletedProject);
                }
                _unitOfWork.Commit();
            }
        }

        public void Dispose()
        {
            UserManager.Dispose();
            _unitOfWork.Dispose();
        }
    }
}
