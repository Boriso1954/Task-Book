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
using Microsoft.AspNet.Identity.Owin;
using TaskBook.DataAccessLayer;
using TaskBook.DataAccessLayer.Exceptions;
using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DataAccessLayer.Reader;
using TaskBook.DomainModel;
using TaskBook.Services.Interfaces;
using TaskBook.DomainModel.ViewModels;
using TaskBook.WebApi.Attributes;
using TaskBook.Services;
using TaskBook.DataAccessLayer.AuthManagers;
using System.Web;
using Microsoft.Practices.Unity;
using System.Web.Http.Tracing;
using NLog.Mvc;
using System.Text;

namespace TaskBook.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Projects")]
    public class ProjectsController : ApiController
    {
        private readonly IProjectService _projectService;
        private TbUserManager _userManager;
        private readonly bool _softDeleted = false;
        private readonly ILogger _logger;

        public ProjectsController(IProjectService projectService,
            TbUserManager userManager,
            ILogger logger)
        {
            _projectService = projectService;
            _projectService.UserManager = userManager;
            _logger = logger;
        }

        [InjectionConstructor]
        public ProjectsController(IProjectService projectService,
            ILogger logger)
        {
            _projectService = projectService;
            _projectService.UserManager = UserManager;
            _logger = logger;
        }
        
        public TbUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<TbUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET api/Projects/GetProjectsAndManagers
        [Route("GetProjectsAndManagers")]
        [ResponseType(typeof(IQueryable<ProjectManagerVm>))]
        [AuthorizeRoles(RoleKey.Admin)]
        public IHttpActionResult GetProjectsAndManagers()
        {
            try
            {
                var projectsAndManagers = _projectService.GetProjectsAndManagers();
                if(projectsAndManagers == null)
                {
                    string msg = "Unable to return list of projects and managers.";
                    _logger.Warning(msg);
                    return BadRequest(msg);
                }
                return Ok(projectsAndManagers);
            }
            catch(DataAccessReaderException ex)
            {
                _logger.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        // GET api/Projects/GetProjectsAndManagers/{projectId}
        [Route("GetProjectsAndManagers/{projectId:long}")]
        [ResponseType(typeof(ProjectManagerVm))]
        public IHttpActionResult GetProjectsAndManagers(long projectId)
        {
            try
            {
                var project = _projectService.GetProjectsAndManagers(projectId);
                if(project == null)
                {
                    string msg = string.Format("Unable to return the project and its manager for project ID '{0}'", projectId);
                    _logger.Warning(msg);
                    return BadRequest(msg);
                }
                return Ok(project);
            }
            catch(DataAccessReaderException ex)
            {
                _logger.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        // GET api/Projects/GetProjectById/{id}
        [Route("GetProjectById/{id:long}")]
        [ResponseType(typeof(ProjectVm))]
        [AuthorizeRoles(RoleKey.Admin)]
        public IHttpActionResult GetProjectById(long id)
        {
            try
            {
                var projectVm = _projectService.GetById(id);
                if(projectVm == null)
                {
                    string msg = string.Format("Unable to return project's details for project ID '{0}'", id);
                    _logger.Warning(msg);
                    return BadRequest(msg);
                }
                return Ok(projectVm);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        // POST api/Projects/AddProject
        [Route("AddProject")]
        [HttpPost]
        [AuthorizeRoles(RoleKey.Admin)]
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
                _logger.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // PUT api/Projects/UpdateProject/{id}
        [Route("UpdateProject/{id:long}")]
        [HttpPut]
        [AuthorizeRoles(RoleKey.Admin)]
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
                _logger.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
            
            return Ok();
        }

        // DELETE api/Projects/DeleteProject/{id}
        [Route("DeleteProject/{id:long}")]
        [HttpDelete]
        [AuthorizeRoles(RoleKey.Admin)]
        public IHttpActionResult DeleteProject(long id)
        {
            try
            {
                _projectService.DeleteProject(id, _softDeleted);
            }
            catch(DataAccessLayerException ex)
            {
                _logger.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch(DataAccessReaderException ex)
            {
                
                _logger.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch(TbIdentityException ex)
            {
                string msg = GetErrorResult(ex.TbIdentityResult);
                if(string.IsNullOrEmpty(msg))
                {
                    _logger.Error(ex.Message);
                    return InternalServerError(ex);
                }
                else
                {
                    msg = ex.Message + ": " + msg;
                    _logger.Error(msg);
                    return BadRequest(msg);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        private string GetErrorResult(IdentityResult result)
        {
            if(result == null)
            {
                return string.Empty;
            }
            else // !result.Succeeded
            {
                if(result.Errors != null)
                {
                    var sb = new StringBuilder();
                    foreach(string error in result.Errors)
                    {
                        sb.AppendLine(error);
                    }
                    return sb.ToString();
                }
                return string.Empty;
            }
        }

        protected override void Dispose(bool disposing)
        {
            _projectService.Dispose();
            base.Dispose(disposing);
        }
    }
}
