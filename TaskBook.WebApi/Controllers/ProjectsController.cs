﻿using System;
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
using TaskBook.Services.AuthManagers;
using System.Web;
using Microsoft.Practices.Unity;

namespace TaskBook.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Projects")]
    public class ProjectsController : ApiController
    {
        private readonly IProjectService _projectService;
        private TbUserManager _userManager;
        private readonly bool _softDeleted = false;

        public ProjectsController(IProjectService projectService,
            TbUserManager userManager)
        {
            _projectService = projectService;
            _projectService.UserManager = userManager;
        }

        [InjectionConstructor]
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
            _projectService.UserManager = UserManager;
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
                    return BadRequest("Unable to return list of projects and managers.");
                }
                return Ok(projectsAndManagers);
            }
            catch(DataAccessReaderException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
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
                    return BadRequest(string.Format("Unable to return the project anf int amanger for project ID '{0}'", projectId));
                }
                return Ok(project);
            }
            catch(DataAccessReaderException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
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
                return Ok(projectVm);
            }
            catch(Exception ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
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
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
            catch(Exception ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
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
        [AuthorizeRoles(RoleKey.Admin)]
        public IHttpActionResult DeleteProject(long id)
        {
            try
            {
                _projectService.DeleteProject(id, _softDeleted);
            }
            catch(DataAccessLayerException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
            catch(DataAccessReaderException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
            catch(TbIdentityException ex)
            {
                return GetErrorResult(ex.TbIdentityResult);
            }
            catch(Exception ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
            return Ok();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if(result == null)
            {
                return InternalServerError();
            }
            else // !result.Succeeded
            {
                if(result.Errors != null)
                {
                    foreach(string error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
                return BadRequest(ModelState);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _projectService.Dispose();
            base.Dispose(disposing);
        }
    }
}
