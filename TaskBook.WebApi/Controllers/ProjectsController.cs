using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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

        // GET api/ProjectsAndManagers
        [Route("~/api/ProjectsAndManagers")]
        public IEnumerable<ProjectManagerVm> GetProjectsAndManagers()
        {
            return _readerRepository.GetProjectsAndManagers();
        }

        protected override void Dispose(bool disposing)
        {
            _readerRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
