using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using TaskBook.DataAccessLayer;
using TaskBook.DataAccessLayer.Exceptions;
using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DataAccessReader;
using TaskBook.DataAccessReader.ViewModels;
using TaskBook.DomainModel;

namespace TaskBook.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Projects")]
    public class ProjectsController : ApiController
    {
        private readonly ReaderRepository _readerRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<TbUser> _userManager;
        private readonly IProjectUsersRepository _projectUsersRepository;

        public ProjectsController(ReaderRepository readerRepository, 
            IProjectRepository projectRepository,
            IProjectUsersRepository projectUsersRepository,
            IUserStore<TbUser> userStore)
        {
            _readerRepository = readerRepository;
            _projectRepository = projectRepository;
            _projectUsersRepository = projectUsersRepository;
            _userManager = new UserManager<TbUser>(userStore);

        }

        // GET api/Projects/GetProjectsAndManagers
        [Route("GetProjectsAndManagers")]
        [ResponseType(typeof(IQueryable<ProjectManagerVm>))]
        public IHttpActionResult GetProjectsAndManagers()
        {
            var projectsAndManagers = _readerRepository.GetProjectsAndManagers();
            //return BadRequest("Error in GetProjectsAndManagers");
            return Ok(projectsAndManagers);
        }

        // GET api/Projects/GetProjectsAndManagers/{projectId}
        [Route("GetProjectsAndManagers/{projectId:long}")]
        [ResponseType(typeof(ProjectManagerVm))]
        public IHttpActionResult GetProjectsAndManagers(long projectId)
        {
            var project = _readerRepository.GetProjectsAndManagers(projectId).FirstOrDefault();
            
            if(project == null)
            {
                return BadRequest("Unable to find the project.");
            }
            return Ok(project);
        }

        // PUT api/Projects/GetProjectById/{id}
        [Route("GetProjectById/{id:long}")]
        [ResponseType(typeof(ProjectVm))]
        public IHttpActionResult GetProjectById(long id)
        {
            var project = _projectRepository.GetById(id);

            if(project == null)
            {
                return BadRequest(string.Format("The project ID '{0}' is not found.", id));
            }

            var projectVm = new ProjectVm()
            {
                Title = project.Title
            };
            return Ok(projectVm);
        }

        // PUT api/Projects/UpdateProject/{id}
        [Route("UpdateProject/{id:long}")]
        [HttpPut]
        public IHttpActionResult UpdateProject(long id, ProjectManagerVm projectVm)
        {
            if(id != projectVm.ProjectId)
            {
                return BadRequest("Project ID conflict.");
            }

            var toBeUpdated = _projectRepository.GetById(id);
            
            if(toBeUpdated == null)
            {
                return BadRequest(string.Format("The project {0} is not found.", projectVm.ProjectTitle));
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            toBeUpdated.Title = projectVm.ProjectTitle;

            try
            {
                _projectRepository.Update(toBeUpdated);
                _projectRepository.SaveChanges();
            }
            catch(DataAccessLayerException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT api/Projects/AddProject
        [Route("AddProject")]
        [HttpPost]
        public IHttpActionResult AddProject(ProjectVm projectVm)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = new Project()
            {
                Title = projectVm.Title,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            var user = _userManager.FindByName("NotAssigned");

            using(var transaction = new TransactionScope())
            {
                try
                {
                    _projectRepository.Add(project);
                    _projectRepository.SaveChanges();

                    var projectUsers = new ProjectUsers()
                    {
                        ProjectId = project.Id,
                        UserId = user.Id
                    };

                    _projectUsersRepository.Add(projectUsers);
                    _projectUsersRepository.SaveChanges();
                }
                catch(DataAccessLayerException ex)
                {
                    return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
                }
                transaction.Complete();
            }
            return Ok(project);
        }

         // DELETE api/Projects/DeleteProject/{id}
        [Route("DeleteProject/{id:long}")]
        [HttpDelete]
        public IHttpActionResult DeleteProject(long id)
        {
            var existing = _projectRepository.GetById(id);
            if(existing == null)
            {
                return BadRequest("Project is not found");
            }

            try
            {
                _projectRepository.Delete(existing);
                _projectRepository.SaveChanges();
            }
            catch(DataAccessLayerException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            _readerRepository.Dispose();
            _projectRepository.Dispose();
            _userManager.Dispose();
            base.Dispose(disposing);
        }
    }
}
