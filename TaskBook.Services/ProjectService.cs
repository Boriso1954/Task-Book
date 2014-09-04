using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNet.Identity;
using TaskBook.DataAccessLayer.Exceptions;
using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DomainModel;
using TaskBook.Services.Interfaces;
using TaskBook.DomainModel.ViewModels;
using TaskBook.DataAccessLayer.Reader;
using TaskBook.DataAccessLayer;
using TaskBook.Services.AuthManagers;
using Microsoft.Practices.Unity;

namespace TaskBook.Services
{
    public sealed class ProjectService: IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        [InjectionConstructor]
        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ProjectService(IUnitOfWork unitOfWork,
            TbUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            UserManager = userManager;
        }

        public TbUserManager UserManager { get; set; }

        public IQueryable<ProjectManagerVm> GetProjectsAndManagers()
        {
            var readerRepository = _unitOfWork.ReaderRepository;
            var projectsAndManagers = readerRepository.GetProjectsAndManagers();
            return projectsAndManagers;
        }

        public ProjectManagerVm GetProjectsAndManagers(long projectId)
        {
            var readerRepository = _unitOfWork.ReaderRepository;
            var projectAndManager = readerRepository.GetProjectsAndManagers(projectId).FirstOrDefault();
            return projectAndManager;
        }

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

        public void AddProject(ProjectVm projectVm)
        {
            var project = new Project()
            {
                Title = projectVm.Title,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            var notAssignedUserId = UserManager.FindByName("NotAssigned").Id;
            var projectRepository = _unitOfWork.ProjectRepository;
            var projectAccessService = new ProjectAccessService(_unitOfWork);

            using(var transaction = TransactionProvider.GetTransactionScope())
            {
                projectRepository.Add(project);
                projectAccessService.AddUserToProject(project.Id, notAssignedUserId);
                _unitOfWork.Commit();
                transaction.Complete();
            }
        }

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
                var userService = new UserService(_unitOfWork, UserManager, new EmailService());
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
