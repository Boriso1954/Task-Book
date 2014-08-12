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
using TaskBook.DataAccessReader;

namespace TaskBook.Services
{
    public sealed class ProjectService: IProjectService
    {
        private readonly UserManager<TbUser> _userManager;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectAccessService _projectAccessService;
        private readonly ReaderRepository _readerRepository;

        public ProjectService(IUserStore<TbUser> userStore,
            IProjectRepository projectRepository,
            IProjectAccessService projectAccessService,
            ReaderRepository readerRepository)
        {
            _userManager = new UserManager<TbUser>(userStore);
            _projectRepository = projectRepository;
            _projectAccessService = projectAccessService;
            _readerRepository = readerRepository;
        }

        public IQueryable<ProjectManagerVm> GetProjectsAndManagers()
        {
            var projectsAndManagers = _readerRepository.GetProjectsAndManagers();
            return projectsAndManagers;
        }

        public ProjectManagerVm GetProjectsAndManagers(long projectId)
        {
            var projectAndManager = _readerRepository.GetProjectsAndManagers(projectId).FirstOrDefault();
            return projectAndManager;
        }

        public ProjectVm GetById(long id)
        {
            var project = _projectRepository.GetById(id);
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

            using(var transaction = new TransactionScope())
            {
                try
                {
                    _projectRepository.Add(project);
                    _projectRepository.SaveChanges();

                    _projectAccessService.AddUserToProject(project.Id, notAssignedUserId);
                }
                catch(DataAccessLayerException)
                {
                    throw;
                }
                transaction.Complete();
            }
        }

        public void UpdateProject(long id, ProjectManagerVm projectVm)
        {
            if(id != projectVm.ProjectId)
            {
                throw new Exception("Project ID conflict.");
            }

            var toBeUpdated = _projectRepository.GetById(id);

            if(toBeUpdated == null)
            {
                throw new Exception(string.Format("The project {0} is not found.", projectVm.ProjectTitle));
            }

            toBeUpdated.Title = projectVm.ProjectTitle;

            try
            {
                _projectRepository.Update(toBeUpdated);
                _projectRepository.SaveChanges();
            }
            catch(DataAccessLayerException)
            {
                throw;
            }
        }

        public void DeleteProject(long id)
        {
            var existing = _projectRepository.GetById(id);
            if(existing == null)
            {
                throw new Exception("Project is not found");
            }

            try
            {
                _projectRepository.Delete(existing);
                _projectRepository.SaveChanges();
            }
            catch(DataAccessLayerException)
            {
                throw;
            }
        }

        public void Dispose()
        {
            _userManager.Dispose();
            _projectRepository.Dispose();
            _projectAccessService.Dispose();
        }
    }
}
