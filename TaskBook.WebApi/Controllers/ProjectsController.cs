using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

        public ProjectsController(ReaderRepository readerRepository, IProjectRepository projectRepository)
        {
            _readerRepository = readerRepository;
            _projectRepository = projectRepository;
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
        public async Task<IHttpActionResult> AddProject(ProjectVm projectVm)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = new Project()
            {
                Title = projectVm.Title,
                CreatedDate = DateTimeOffset.UtcNow
            };

            try
            {
               await _projectRepository.AddAsync(project);
                _projectRepository.SaveChanges();
            }
            catch(DataAccessLayerException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
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
            base.Dispose(disposing);
        }
    }
}
