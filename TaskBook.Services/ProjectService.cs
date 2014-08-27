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

namespace TaskBook.Services
{
    public sealed class ProjectService: IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<TbUser> _userManager;

        public ProjectService(IUnitOfWork unitOfWork,
            IUserStore<TbUser> userStore)
        {
            _unitOfWork = unitOfWork;
            _userManager = new UserManager<TbUser>(userStore);
        }

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

            var notAssignedUserId = _userManager.FindByName("NotAssigned").Id;
            var projectRepository = _unitOfWork.ProjectRepository;
            var projectAccessService = new ProjectAccessService(_unitOfWork);

            using(var transaction = new TransactionScope())
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

        public void DeleteProject(long id)
        {
            var projectRepository = _unitOfWork.ProjectRepository;
            var existing = projectRepository.GetById(id);
            if(existing == null)
            {
                throw new Exception(string.Format("Unable to find project to be deleted. Project ID {0}", id));
            }

            string managerId = string.Empty;
            using(var readerRepository = _unitOfWork.ReaderRepository)
            {
                managerId = readerRepository.GetProjectsAndManagers(id).FirstOrDefault().ManagerId;
            }

            var userService = new UserService(_unitOfWork, _userManager);
            using(var transaction = new TransactionScope())
            {
                projectRepository.Delete(existing);
                userService.DeleteUser(managerId);
                _unitOfWork.Commit();
                transaction.Complete();
            }
        }

        public void Dispose()
        {
            _userManager.Dispose();
            _unitOfWork.Dispose();
        }
    }
}
