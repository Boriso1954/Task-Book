using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TaskBook.DataAccessReader;
using TaskBook.DataAccessReader.ViewModels;

namespace TaskBook.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Projects")]
    public class ProjectsController : ApiController
    {
        private readonly ReaderRepository _readerRepository;

        public ProjectsController(ReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
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


        protected override void Dispose(bool disposing)
        {
            _readerRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
