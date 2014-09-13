using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Tracing;
using TaskBook.DataAccessLayer.Exceptions;
using TaskBook.DomainModel.ViewModels;
using TaskBook.Services.Interfaces;

namespace TaskBook.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Permissions")]
    public class PermissionsController : ApiController
    {
        private readonly IPermissionService _permissionService;
        private readonly ITraceWriter _logger;

        public PermissionsController(IPermissionService permissionService,
            ITraceWriter logger)
        {
            _permissionService = permissionService;
            _logger = logger;
        }

        // GET api//Permissions/GetByRole/{roleName}
        [Route("GetByRole/{roleName}")]
        [ResponseType(typeof(IQueryable<PermissionVm>))]
        public IHttpActionResult GetByRole(string roleName)
        {
            try
            {
                var permissions = _permissionService.GetByRole(roleName);

                if(permissions == null)
                {
                    string msg = string.Format("Unable to return permissions for role '{0}'", roleName);
                    _logger.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, msg);
                    return BadRequest(msg);
                }
                return Ok(permissions);
            }
            catch(DataAccessReaderException ex)
            {
                _logger.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, ex);
                return BadRequest(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _permissionService.Dispose();
            base.Dispose(disposing);
        }

    }
}
