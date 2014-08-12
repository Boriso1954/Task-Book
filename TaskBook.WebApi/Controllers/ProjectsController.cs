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
using TaskBook.DomainModel;
using TaskBook.Services.Interfaces;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Projects")]
    public class ProjectsController : ApiController
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET api/Projects/GetProjectsAndManagers
        [Route("GetProjectsAndManagers")]
        [ResponseType(typeof(IQueryable<ProjectManagerVm>))]
        public IHttpActionResult GetProjectsAndManagers()
        {
            var projectsAndManagers = _projectService.GetProjectsAndManagers();
            if(projectsAndManagers == null)
            {
                return BadRequest("Unable to return list of projects and managers.");
            }
            return Ok(projectsAndManagers);
        }

        // GET api/Projects/GetProjectsAndManagers/{projectId}
        [Route("GetProjectsAndManagers/{projectId:long}")]
        [ResponseType(typeof(ProjectManagerVm))]
        public IHttpActionResult GetProjectsAndManagers(long projectId)
        {
            var project = _projectService.GetProjectsAndManagers(projectId);
            if(project == null)
            {
                return BadRequest(string.Format("Unable to return the project anf int amanger for project ID '{0}'", projectId));
            }
            return Ok(project);
        }

        // PUT api/Projects/GetProjectById/{id}
        [Route("GetProjectById/{id:long}")]
        [ResponseType(typeof(ProjectVm))]
        public IHttpActionResult GetProjectById(long id)
        {
            try
            {
                var projectVm = _projectService.GetById(id);
                return Ok(projectVm);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

            try
            {
                _projectService.AddProject(projectVm);
            }
            catch(DataAccessLayerException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
           
            return Ok();
        }

        // PUT api/Projects/UpdateProject/{id}
        [Route("UpdateProject/{id:long}")]
        [HttpPut]
        public IHttpActionResult UpdateProject(long id, ProjectManagerVm projectVm)
        {
             if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _projectService.UpdateProject(id, projectVm);
            }
            catch(DataAccessLayerException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
            catch(Exception ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
            
            return Ok();
        }

         // DELETE api/Projects/DeleteProject/{id}
        [Route("DeleteProject/{id:long}")]
        [HttpDelete]
        public IHttpActionResult DeleteProject(long id)
        {
            try
            {
                _projectService.DeleteProject(id);
            }
            catch(DataAccessLayerException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
            catch(Exception ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
            
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            _projectService.Dispose();
            base.Dispose(disposing);
        }
    }
}
